﻿using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WhatsAppBot.Enuns;

namespace WhatsAppBot
{
    public partial class WhatsAppBot : Form
    {
        public WhatsAppBot()
        {
            InitializeComponent();
            KillCrhomeDriver();
        }

        private void ExibirExcecao(Exception ex)
        {
            ExceptionToFile(ex);
            MessageBox.Show(this, ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void carregarContatosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (var ofd = new OpenFileDialog())
                {
                    ofd.Title = "Carregar arquivos de contatos";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        contatosDataGridView.Rows.Clear();
                        var linhas = File.ReadAllLines(ofd.FileName).ToList();
                        linhas.RemoveAt(0);
                        foreach (var linha in linhas)
                        {
                            var contato = new Contato(linha.Split(';').Select(o => (object)o).ToArray());
                            contatosDataGridView.Rows.Add(contato.ToRow());
                        }
                        DefineRowColor();
                    }
                }
            }
            catch (Exception ex) 
            {
                ExibirExcecao(ex);
            }
        }

        private void opçõesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var form = new OpcoesForm();
                form.Show(this);
            }
            catch (Exception ex)
            {
                ExibirExcecao(ex);
            }
        }

        private void gerarArquivoVcfToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                GerarVcf();
            }
            catch (Exception ex)
            {
                ExibirExcecao(ex);
            }
        }

        public void GerarVcf()
        {
            if (contatosDataGridView.Rows.Count > 0)
            {
                var contatos = contatosDataGridView.Rows.Cast<DataGridViewRow>().Select(r =>
                    new { Nome = r?.Cells[1]?.Value?.ToString(), Telefone = r?.Cells[2]?.Value?.ToString() });

                var linhas = new List<string>();
                foreach (var contato in contatos)
                {
                    linhas.Add(AddVcf(contato.Nome, contato.Telefone));
                }
                File.WriteAllLines($"{DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}.vcf", linhas);
                MessageBox.Show("Arquivo CVF gerado!");
            }
        }

        private static string AddVcf(string nome, string telefone)
        {
            var sb = new StringBuilder();
            sb.AppendLine("BEGIN: VCARD");
            sb.AppendLine("VERSION:3.0");
            sb.AppendLine($"N:{nome}");
            sb.AppendLine($"TEL;TYPE=CELL:{telefone}");
            sb.Append("END:VCARD");
            return sb.ToString();
        }

        public static bool CANCELAR_EXECUCAO { get; set; }
        public static DataGridViewRowCollection ROWS { get; set; }
        private void executarToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            Task.Factory.StartNew(() =>
            {
                try
                {
                    Invoke((MethodInvoker)delegate () { executarToolStripMenuItem.Enabled = false; });
                    Invoke((MethodInvoker)delegate () { pararExecuçãoToolStripMenuItem.Enabled = true; });

                    var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
                    using (var driver = new ChromeDriver())
                    {
                        driver.Navigate().GoToUrl("https://web.whatsapp.com/");
                        var seachText = WaitLogin(driver);

                        Invoke((MethodInvoker)delegate () { ROWS = contatosDataGridView.Rows; });

                        for (int r = 0; r < ROWS.Count; r++)
                        {
                            if (CANCELAR_EXECUCAO)
                            {
                                CANCELAR_EXECUCAO = false;
                                MessageBox.Show("Execução cancelada!");
                                return;
                            }

                            var c = CarregarContato(ROWS[r].Cells);
                            if ((!c.ArquivosEnviados || !c.MensagemEnviada) && (c.ContatoEncontrado ?? true))
                            {
                                Invoke((MethodInvoker)delegate () { contatosDataGridView.Rows[r].DefaultCellStyle.BackColor = Color.Yellow; });

                                try
                                {
                                    var tentativas = 3;
                                    while (tentativas > 0)
                                    {
                                        try
                                        {
                                            if (SetarContato(c, driver, seachText, TimeSpan.FromSeconds(config.SegundosDeProcura), config.TipoDeProcura))
                                            {
                                                c.ContatoEncontrado = true;
                                                Thread.Sleep(TimeSpan.FromSeconds(new Random().Next(config.IntervaloMin, config.IntervaloMax + 1)));

                                                if (!string.IsNullOrEmpty(c.DefinirMensagem()) && !c.MensagemEnviada)
                                                {
                                                    EnviadoMensagem(driver, c);
                                                    c.MensagemEnviada = true;
                                                    Invoke((MethodInvoker)delegate () { AtualizarEnvioContato(c, r); });
                                                }

                                                var arquivos = c.BuscarArquivos(config.BuscarArquivos);
                                                foreach (var arquivo in arquivos)
                                                {
                                                    if (arquivo != null && File.Exists(arquivo) && !c.ArquivosEnviados)
                                                    {
                                                        EnviarArquivo(driver, c, arquivo);
                                                    }
                                                    Thread.Sleep(TimeSpan.FromSeconds(1));
                                                }

                                                if (arquivos.Count > 0)
                                                {
                                                    c.ArquivosEnviados = true;
                                                    Invoke((MethodInvoker)delegate () { AtualizarEnvioContato(c, r); });
                                                }
                                                break;
                                            }
                                            else
                                            {
                                                c.ContatoEncontrado = false;
                                                Invoke((MethodInvoker)delegate () { AtualizarEnvioContato(c, r); });
                                                break;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            tentativas--;
                                            try
                                            {
                                                ExceptionToFile(new ContatoSendException($"Erro ao enviar contato: {JsonConvert.SerializeObject(c, Formatting.Indented)}", ex));
                                            }
                                            catch { }
                                        }
                                    }
                                    if (c.ContatoEncontrado.HasValue && !c.ContatoEncontrado.Value)
                                    {
                                        Invoke((MethodInvoker)delegate () { contatosDataGridView.Rows[r].DefaultCellStyle.BackColor = Color.Orange; });
                                    }
                                    else
                                    {
                                        Invoke((MethodInvoker)delegate () { contatosDataGridView.Rows[r].DefaultCellStyle.BackColor = Color.Green; });
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ExceptionToFile(ex);
                                    Invoke((MethodInvoker)delegate () { contatosDataGridView.Rows[r].DefaultCellStyle.BackColor = Color.Red; });
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExibirExcecao(ex);
                }
                finally 
                {
                    Invoke((MethodInvoker)delegate () { executarToolStripMenuItem.Enabled = true; });
                    Invoke((MethodInvoker)delegate () { pararExecuçãoToolStripMenuItem.Enabled = false; });
                    MessageBox.Show("Envios concluídos");
                }
            });
        }
        private static void AnexarButtonClick(IWebDriver driver)
        {
            driver.SecureFindAndClick(By.CssSelector("span[data-icon='clip']"));
        }
        private void AtualizarEnvioContato(Contato c, int indexRow)
        {
            var linhas = File.ReadAllLines("contatos.csv", Encoding.UTF8).ToList();
            var index = linhas.FindIndex(l => l.Split(';')[0] == c.Cpf);
            var contatoEncontrado = "";

            if (c.ContatoEncontrado.HasValue)
            {
                contatoEncontrado = c.ContatoEncontrado.Value ? "1" : "0";
            }

            linhas[index] = $"{c.Cpf};{c.Nome};{c.Telefone};{c?.Mensagem1};{c.Mensagem2};{c.Mensagem3};{(c.MensagemEnviada ? "1" : "0")};{(c.ArquivosEnviados ? "1" : "0")};{contatoEncontrado}";
            File.WriteAllLines("contatos.csv", linhas, Encoding.UTF8);

            contatosDataGridView.Rows[indexRow].SetValues(c.ToRow());  
        }

        private void EnviadoMensagem(ChromeDriver driver, Contato contato)
        {
            //driver.SecureFindAndSendKeys(By.XPath("/html/body/div[1]/div[1]/div[1]/div[4]/div[1]/footer/div[1]/div/div/div[2]/div[1]/div/div[2]"), contato.DefinirMensagem());
            driver.SecureFindAndSendKeys(By.XPath("/html/body/div[1]/div[1]/div[1]/div[4]/div[1]/footer/div[1]/div/span[2]/div/div[2]/div[1]/div/div[2]"), contato.DefinirMensagem());
            driver.SecureFindAndClick(By.CssSelector("span[data-icon='send']"));
        }

        private static void EnviarArquivo(ChromeDriver driver, Contato contato, string arquivo)
        {
            AnexarButtonClick(driver);
            IEnumerable<IWebElement> elements = null;
            while (elements?.Where(e => e.SecureGetAttribute("accept") == "*")?.FirstOrDefault() == null)
            {
                try
                {
                    elements = driver.FindElements(By.TagName("input"));
                }
                catch { }

                Thread.Sleep(TimeSpan.FromSeconds(1));
            }

            var input = driver.SecureFind(By.CssSelector("input[type='file']"));
            input.SendKeys(arquivo);
            //driver.SecureFindAndClick(By.CssSelector("span[data-icon='send']"));
            driver.SecureFindAndClick(By.XPath("//*[@id=\"app\"]/div[1]/div[1]/div[2]/div[2]/span/div[1]/span/div[1]/div/div[2]/div/div[2]/div[2]"));
        }


        private static bool SetarContato(Contato c, ChromeDriver driver, IWebElement seachText, TimeSpan segundosDeProcura, TipoDeProcura tipoDeBusca)
        {
            seachText.Clear();
            if (tipoDeBusca == TipoDeProcura.Nome)
            {
                seachText.SendKeys(c.Nome);
            }
            else if (tipoDeBusca == TipoDeProcura.Telefone)
            {
                seachText.SendKeys(c.Telefone);
            }
            else if (tipoDeBusca == TipoDeProcura.NomeETelefone) 
            {
                seachText.SendKeys(c.Telefone);
                if (!VerificandoContatoSelecionado(driver, c, segundosDeProcura)) 
                {
                    seachText.Clear();
                    seachText.SendKeys(c.Nome);
                }
                else 
                {
                    return true;    
                }
            }
            Thread.Sleep(TimeSpan.FromSeconds(1));
            return VerificandoContatoSelecionado(driver, c, segundosDeProcura);
        }

        private static bool VerificandoContatoSelecionado(IWebDriver driver, Contato c, TimeSpan segundosDeProcura)
        {
            IEnumerable<IWebElement> contatos = null;
            var sair = false;
            while (contatos?.Where(e => e.SecureGetAttribute("Title") == c.Nome)?.FirstOrDefault() == null && !sair)
            {
                try
                {
                    contatos = driver.FindElements(By.TagName("span"));
                }
                catch { }

                Thread.Sleep(TimeSpan.FromSeconds(1));
                segundosDeProcura = segundosDeProcura.Subtract(TimeSpan.FromSeconds(1));

                if (segundosDeProcura.TotalSeconds <= 0)
                {
                    sair = true;
                }
            }

            if (sair)
            {
                return false;
            }

            contatos = driver.FindElements(By.TagName("span"));
            contatos.Where(e => e.SecureGetAttribute("Title") == c.Nome).FirstOrDefault().Click();
            return true;
        }


        public void DefineRowColor() 
        {
            for (int r = 0; r < contatosDataGridView.Rows.Count; r++)
            {
                var contato = CarregarContato(contatosDataGridView.Rows[r].Cells);
                if (contato.MensagemEnviada && contato.ArquivosEnviados)
                {
                    contatosDataGridView.Rows[r].DefaultCellStyle.BackColor = Color.Green;
                }
                else if(contato.ContatoEncontrado.HasValue && !contato.ContatoEncontrado.Value)
                {
                    contatosDataGridView.Rows[r].DefaultCellStyle.BackColor = Color.Orange;
                }
            }
        }

        public static string WriteException(Exception ex)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Exception: {ex.GetType().Name}");
            sb.AppendLine($"Message: {ex.Message}");
            sb.AppendLine($"StackTrace: {ex.StackTrace}");
            if (ex.InnerException != null)
            {
                sb.AppendLine($"Inner Exception: ({WriteException(ex.InnerException)})");
            }
            return sb.ToString();
        }

        public static void ExceptionToFile(Exception ex)
        {
            try
            {
                var file = WriteException(ex);
                Directory.CreateDirectory("Exceptions");
                File.WriteAllText($"Exceptions\\{DateTime.Now.ToString("ddMMyyyyHHmmss")}.txt", file);
            }
            catch { }
        }

        public static IWebElement WaitLogin(IWebDriver driver)
        {
            IWebElement seachText = null;
            while (seachText == null || CANCELAR_EXECUCAO)
            {
                try
                {
                    seachText = driver.SecureFind(By.XPath("/html/body/div[1]/div[1]/div[1]/div[3]/div/div[1]/div/label/div/div[2]"), TimeSpan.FromSeconds(1));
                }
                catch { }

                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            return seachText;
        }
        public Contato CarregarContato(DataGridViewCellCollection cells) 
        {
            return new Contato()
            {
                Cpf = cells[cpfColumn.Index].Value?.ToString(),
                Nome = cells[nomeColumn.Index].Value?.ToString(),
                Telefone = cells[telefoneColumn.Index].Value?.ToString(),
                Mensagem1 = cells[msg1Column.Index].Value?.ToString(),
                Mensagem2 = cells[msg2Column.Index].Value?.ToString(),
                Mensagem3 = cells[msg3Column.Index].Value?.ToString(),
                ArquivosEnviados = (bool)cells[ArquivoEnviadoColumn.Index]?.Value,
                MensagemEnviada = (bool)cells[MensagemEnviadaColumn.Index]?.Value,
                ContatoEncontrado = (bool?)cells[ContatoEncontradoColumn.Index]?.Value ?? null
            };
        }

        private void pararExecuçãoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CANCELAR_EXECUCAO = true; 
            }
            catch (Exception ex)
            {
                ExibirExcecao(ex);
            }

        }

        private void contatosDataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            contatosDataGridView.ClearSelection();
            contatosDataGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = contatosDataGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor;
            contatosDataGridView.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = contatosDataGridView.Rows[e.RowIndex].DefaultCellStyle.ForeColor;
        }

        private void contatosDataGridView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            contatosDataGridView.ClearSelection();
        }

        private void contatosDataGridView_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            contatosDataGridView.ClearSelection();
        }

        private void contatosDataGridView_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            contatosDataGridView.ClearSelection();
        }

        private void contatosDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            contatosDataGridView.ClearSelection();
        }

        private void WhatsAppBot_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                KillCrhomeDriver();
            }
            catch (Exception ex) 
            {
                ExibirExcecao(ex);
            }
        }

        private static void KillCrhomeDriver()
        {
            foreach (var process in Process.GetProcessesByName("chromedriver"))
            {
                try
                {
                    process.Kill();
                }
                catch { }
            }
        }
    }
}
