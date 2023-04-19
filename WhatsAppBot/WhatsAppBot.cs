using Newtonsoft.Json;
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
        public string UltimoArquivoCarregado { get; set; }
        public WhatsAppBot()
        {
            InitializeComponent();
            KillCrhomeDriver();
        }

        private void ExibirExcecao(Exception ex)
        {
            ExceptionToFile(ex);
            MessageBox.Show(null, ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void CarregarContatosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (var ofd = new OpenFileDialog())
                {
                    ofd.Title = "Carregar arquivos de contatos";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        UltimoArquivoCarregado = ofd.FileName;
                        CarregarArquivo();
                    }
                }
            }
            catch (Exception ex) 
            {
                ExibirExcecao(ex);
            }
        }

        private void CarregarArquivo()
        {
            contatosDataGridView.Rows.Clear();
            var linhas = File.ReadAllLines(UltimoArquivoCarregado).ToList();
            linhas.RemoveAt(0);
            foreach (var linha in linhas)
            {
                var contato = new Contato(linha.Split(';').Select(o => (object)o).ToArray());
                contatosDataGridView.Rows.Add(contato.ToRow());
            }
        }

        private void OpcoesToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void GerarArquivoVcfToolStripMenuItem_Click(object sender, EventArgs e)
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
                File.WriteAllLines($"{DateTime.Now:yyyy-MM-dd HH-mm-ss}.vcf", linhas);
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
        private void ExecutarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
            
            Task.Factory.StartNew(() =>
            {
                try
                {
                    Invoke((MethodInvoker)delegate () { executarToolStripMenuItem.Enabled = false; });
                    Invoke((MethodInvoker)delegate () { pararExecuçãoToolStripMenuItem.Enabled = true; });

                    config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
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
                                break;
                            }

                            var c = CarregarContato(ROWS[r].Cells);
                                
                            Invoke((MethodInvoker)delegate () { contatosDataGridView.Rows[r].DefaultCellStyle.BackColor = Color.Yellow; });

                            try
                            {                  
                                try
                                {
                                    if (SetarContato(c, driver, seachText, TimeSpan.FromSeconds(config.SegundosDeProcura)))
                                    {
                                        Thread.Sleep(TimeSpan.FromSeconds(new Random().Next(config.IntervaloMin, config.IntervaloMax + 1)));
                                        EnviadoMensagem(driver, c);

                                        var arquivos = new List<string>();
                                        if (!string.IsNullOrEmpty(config?.BuscarArquivos?.DiretorioArquivos))
                                        {
                                            arquivos = c.BuscarArquivos(config.BuscarArquivos);
                                            foreach (var arquivo in arquivos)
                                            {
                                                if (arquivo != null && File.Exists(arquivo))
                                                {
                                                    EnviarArquivo(driver, arquivo);
                                                }
                                                Thread.Sleep(TimeSpan.FromSeconds(1));
                                            }
                                        }

                                        Invoke((MethodInvoker)delegate ()
                                        {
                                            contatosDataGridView.Rows[r].DefaultCellStyle.BackColor = Color.Green;
                                            contatosDataGridView.Rows[r].DefaultCellStyle.ForeColor = Color.White;
                                        });
                                    }
                                    else 
                                    {
                                        Invoke((MethodInvoker)delegate ()
                                        {
                                            contatosDataGridView.Rows[r].DefaultCellStyle.BackColor = Color.Orange;
                                            contatosDataGridView.Rows[r].DefaultCellStyle.ForeColor = Color.White;
                                        });
                                    }
                                }
                                catch (Exception ex)
                                {
                                    try
                                    {
                                        ExceptionToFile(new ContatoSendException($"Erro ao enviar contato: {JsonConvert.SerializeObject(c, Formatting.Indented)}", ex));
                                    }
                                    catch { }
                                }                                                               
                            }
                            catch (Exception ex)
                            {
                                ExceptionToFile(ex);
                                Invoke((MethodInvoker)delegate ()
                                {
                                    contatosDataGridView.Rows[r].DefaultCellStyle.BackColor = Color.Red;
                                    contatosDataGridView.Rows[r].DefaultCellStyle.ForeColor = Color.White;
                                });
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
                    KillCrhomeDriver();
                }
            });
            
        }
        private static void AnexarButtonClick(IWebDriver driver)
        {
            driver.SecureFindAndClick(By.CssSelector("span[data-icon='clip']"));
        }

        private void EnviadoMensagem(ChromeDriver driver, Contato contato)
        {
            var text = driver.SecureFind(By.XPath("/html/body/div[1]/div/div/div[5]/div/footer/div[1]/div/span[2]/div/div[2]/div[1]/div/div[1]"));
            text.SendKeys(contato.Mensagem);
            text.SendKeys(OpenQA.Selenium.Keys.Enter);
            Thread.Sleep(TimeSpan.FromSeconds(2));
        }

        private static void EnviarArquivo(ChromeDriver driver, string arquivo)
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

            var input = driver.SecureFind(By.XPath("/html/body/div[1]/div/div/div[5]/div/footer/div[1]/div/span[2]/div/div[1]/div[2]/div/span/div/div/ul/li[4]/button/input")); input.SendKeys(arquivo);
            Thread.Sleep(2000);

            //driver.SecureFindAndSendKeys("{Enter}");
            driver.SecureFindAndClick(By.XPath("/html/body/div[1]/div/div/div[3]/div[2]/span/div/span/div/div/div[2]/div/div[2]/div[2]/div/div"));
            //driver.SecureFindAndClick(By.XPath("//*[@id=\"app\"]/div[1]/div[1]/div[2]/div[2]/span/div[1]/span/div[1]/div/div[2]/div/div[2]/div[2]"));
        }


        private static bool SetarContato(Contato c, ChromeDriver driver, IWebElement seachText, TimeSpan segundosDeProcura)
        {
            seachText.ClearTextByKey();
            var ddd = "";
            var telefone = "";

            if (c.Telefone.Length == 8) 
            {
                telefone = c.Telefone;
            }
            else if (c.Telefone.Length == 9)
            {
                telefone = c.Telefone.Substring(1);
            }
            else if (c.Telefone.Length == 10)
            {
                ddd = c.Telefone.Substring(0, 2);
                telefone = c.Telefone.ToString().Substring(2);
            }
            else if (c.Telefone.Length == 11)
            {
                ddd = c.Telefone.Substring(0, 2);
                telefone = c.Telefone.ToString().Substring(3);
            }

            seachText.SendKeys($"{ddd}{telefone}");
            Thread.Sleep(segundosDeProcura);

            var paneSide = driver.FindElement(By.Id("pane-side"));
            var contactFind = !paneSide.Text?.Contains("Nenhuma conversa") ?? true;

            if(contactFind)
                seachText.SendKeys(OpenQA.Selenium.Keys.Enter);

            return contactFind;
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
                File.WriteAllText($"Exceptions\\{DateTime.Now:ddMMyyyyHHmmss}.txt", file);
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
                    seachText = driver.SecureFind(By.XPath("/html/body/div[1]/div/div/div[4]/div/div[1]/div/div/div[2]/div/div[1]"), TimeSpan.FromSeconds(1));
                }
                catch
                {
                    try
                    {
                        if (seachText == null)
                        {
                            seachText = driver.SecureFind(By.XPath("/html/body/div[1]/div/div/div[4]/div/div[1]/div/div/div[2]/div/div[1]"));
                        }
                    }
                    catch { }
                }

                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            return seachText;
        }

        public static IWebElement SearchInput { get; set; }


        public Contato CarregarContato(DataGridViewCellCollection cells) 
        {
            return new Contato()
            {
                Telefone = cells[0].Value?.ToString(),
                Mensagem = cells[1].Value?.ToString()
            };
        }

        private void PararExecuçãoToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void ContatosDataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            contatosDataGridView.ClearSelection();
            contatosDataGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = contatosDataGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor;
            contatosDataGridView.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = contatosDataGridView.Rows[e.RowIndex].DefaultCellStyle.ForeColor;
        }

        private void ContatosDataGridView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            contatosDataGridView.ClearSelection();
        }

        private void ContatosDataGridView_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            contatosDataGridView.ClearSelection();
        }

        private void ContatosDataGridView_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            contatosDataGridView.ClearSelection();
        }

        private void ContatosDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
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

        private void ReiniciarArquivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(UltimoArquivoCarregado))
                {
                    var contatos = new List<string>
                    {
                        "Cpf;Nome;Celular;SucessoEnvioMsg;SucessoEnvioArquivos;ContatoEncontrado"
                    };

                    if (MessageBox.Show(this, "Resetar coluna de contato encontrado?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        contatos.AddRange(contatosDataGridView.Rows.Cast<DataGridViewRow>().Select(r =>
                            $"{r?.Cells[0]?.Value?.ToString()};{r?.Cells[1]?.Value?.ToString()};{r?.Cells[2]?.Value?.ToString()};0;0;").ToList());
                    }
                    else
                    {
                        contatos.AddRange(contatosDataGridView.Rows.Cast<DataGridViewRow>().Select(r =>
                                $"{r?.Cells[0]?.Value?.ToString()};{r?.Cells[1]?.Value?.ToString()};{r?.Cells[2]?.Value?.ToString()};0;0;{(r?.Cells[5]?.Value == null ? "" : (bool.Parse(r?.Cells[5]?.Value?.ToString().ToLower()) ? "1" : "0"))}").ToList());
                    }

                    File.WriteAllLines(UltimoArquivoCarregado, contatos);
                    CarregarArquivo();
                }
            }
            catch (Exception ex)
            {
                ExibirExcecao(ex);
            }
        }

        private void WhatsAppBot_Load(object sender, EventArgs e)
        {
            KillCrhomeDriver();
        }
    }
}
