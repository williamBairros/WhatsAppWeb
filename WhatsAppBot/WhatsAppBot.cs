using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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

        private void carregarContatosToolStripMenuItem_Click(object sender, EventArgs e)
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
            DefineRowColor();
        }

        private void opcõesToolStripMenuItem_Click(object sender, EventArgs e)
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
        [STAThread]
        private void executarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
            config.Mensagens = config.Mensagens.Where(msg => !string.IsNullOrEmpty(msg?.Trim())).ToList();
            var seguir = false;
            for (int r = 0; r < contatosDataGridView.Rows.Count; r++)
            {
                var mensagemEnviada = (bool)contatosDataGridView.Rows[r].Cells[3]?.Value;
                var arquivoEnviado = (bool)contatosDataGridView.Rows[r].Cells[4]?.Value;
                var contatoEncontrado = contatosDataGridView.Rows[r].Cells[5]?.Value?.ToString();

                var contato = false;
                if (string.IsNullOrEmpty(contatoEncontrado))
                {
                    contato = true;
                }
                else
                {
                    contato = bool.Parse(contatoEncontrado.ToLower());
                }

                if (string.IsNullOrEmpty(config?.BuscarArquivos?.DiretorioArquivos)) 
                {
                    arquivoEnviado = true;
                }

                if ((!mensagemEnviada || !arquivoEnviado) && contato) 
                {
                    seguir = true;
                    break;
                }
            }

            if (seguir)
            {
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
                                    MessageBox.Show("ExecuÃ§Ã£o cancelada!");
                                    break;
                                }

                                var c = CarregarContato(ROWS[r].Cells);
                                if ((!c.ArquivosEnviados || !c.MensagemEnviada) && (c.ContatoEncontrado ?? true))
                                {
                                    Invoke((MethodInvoker)delegate () { contatosDataGridView.Rows[r].DefaultCellStyle.BackColor = Color.Yellow; });

                                    try
                                    {
                                        var tentativas = 1;
                                        while (tentativas > 0)
                                        {
                                            try
                                            {
                                                if (SetarContato(c, driver, seachText, TimeSpan.FromSeconds(config.SegundosDeProcura), config.TipoDeProcura))
                                                {
                                                    c.ContatoEncontrado = true;
                                                    Thread.Sleep(TimeSpan.FromSeconds(new Random().Next(config.IntervaloMin, config.IntervaloMax + 1)));

                                                    if (!c.MensagemEnviada && !string.IsNullOrEmpty(c.DefinirMensagem(config.Mensagens)))
                                                    {
                                                        EnviadoMensagem(driver, c, config.Mensagens, config.Iphone);
                                                        c.MensagemEnviada = true;
                                                        Invoke((MethodInvoker)delegate () { AtualizarEnvioContato(c, r, UltimoArquivoCarregado); });
                                                    }

                                                    var arquivos = new List<string>();
                                                    if (!string.IsNullOrEmpty(config?.BuscarArquivos?.DiretorioArquivos))
                                                    {
                                                        arquivos = c.BuscarArquivos(config.BuscarArquivos);
                                                        foreach (var arquivo in arquivos)
                                                        {
                                                            if (arquivo != null && File.Exists(arquivo) && !c.ArquivosEnviados)
                                                            {
                                                                EnviarArquivo(driver, c, arquivo, config.Iphone);
                                                            }
                                                            Thread.Sleep(TimeSpan.FromSeconds(1));
                                                        }
                                                    }

                                                    if (arquivos.Count > 0)
                                                    {
                                                        c.ArquivosEnviados = true;
                                                        Invoke((MethodInvoker)delegate () { AtualizarEnvioContato(c, r, UltimoArquivoCarregado); });
                                                    }
                                                    break;
                                                }
                                                else
                                                {
                                                    c.ContatoEncontrado = false;
                                                    Invoke((MethodInvoker)delegate () { AtualizarEnvioContato(c, r, UltimoArquivoCarregado); });
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
                                            Invoke((MethodInvoker)delegate ()
                                            {
                                                contatosDataGridView.Rows[r].DefaultCellStyle.BackColor = Color.Orange;
                                                contatosDataGridView.Rows[r].DefaultCellStyle.ForeColor = Color.White;
                                            });
                                        }
                                        else
                                        {
                                            Invoke((MethodInvoker)delegate ()
                                            {
                                                contatosDataGridView.Rows[r].DefaultCellStyle.BackColor = Color.Green;
                                                contatosDataGridView.Rows[r].DefaultCellStyle.ForeColor = Color.White;
                                            });
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
                    }
                    catch (Exception ex)
                    {
                        ExibirExcecao(ex);
                    }
                    finally
                    {
                        Invoke((MethodInvoker)delegate () { executarToolStripMenuItem.Enabled = true; });
                        Invoke((MethodInvoker)delegate () { pararExecuçãoToolStripMenuItem.Enabled = false; });
                        MessageBox.Show("Envios concluÃ­dos");
                        KillCrhomeDriver();
                    }
                });
            }
            else 
            {
                MessageBox.Show("Sem casos para enviar");
            }
        }
        private static void AnexarButtonClick(IWebDriver driver)
        {
            driver.SecureFindAndClick(By.CssSelector("span[data-icon='attach-menu-plus']"));
        }
        private void AtualizarEnvioContato(Contato c, int indexRow, string nomeArquivo)
        {
            var linhas = File.ReadAllLines(nomeArquivo, Encoding.UTF8).ToList();
            var index = linhas.FindIndex(l => l.Split(';')[0] == c.Cpf);
            var contatoEncontrado = "";

            if (c.ContatoEncontrado.HasValue)
            {
                contatoEncontrado = c.ContatoEncontrado.Value ? "1" : "0";
            }

            linhas[index] = $"{c.Cpf};{c.Nome};{c.Telefone};{(c.MensagemEnviada ? "1" : "0")};{(c.ArquivosEnviados ? "1" : "0")};{contatoEncontrado}";
            File.WriteAllLines(nomeArquivo, linhas, Encoding.UTF8);

            contatosDataGridView.Rows[indexRow].SetValues(c.ToRow());
            contatosDataGridView.Refresh();
            Application.DoEvents();
        }

        private void EnviadoMensagem(ChromeDriver driver, Contato contato, List<string> mensagens, bool iphone = false)
        {
            //driver.SecureFindAndSendKeys(By.XPath("/html/body/div[1]/div[1]/div[1]/div[4]/div[1]/footer/div[1]/div/div/div[2]/div[1]/div/div[2]"), contato.DefinirMensagem());
            //driver.SecureFindAndSendKeys(By.XPath("/html/body/div[1]/div[1]/div[1]/div[4]/div[1]/footer/div[1]/div/span[2]/div/div[2]/div[1]/div/div[2]"), contato.DefinirMensagem(mensagens));
            //driver.SecureFindAndSendKeys(By.XPath("/html/body/div[1]/div/div/div[4]/div/footer/div[1]/div/span[2]/div/div[2]/div[1]/div/div[2]"), contato.DefinirMensagem(mensagens));

            IWebElement text = null;
            if (iphone)
            {                                  
                text = driver.SecureFind(By.XPath("/html/body/div[1]/div/div[2]/div[4]/div/footer/div[1]/div/span[2]/div/div[2]/div[1]/div/div[1]"));
            }
            else 
            {
                text = driver.SecureFind(By.XPath("/html/body/div[1]/div/div[2]/div[4]/div/footer/div[1]/div/span[2]/div/div[2]/div[1]/div[2]/div[1]"));
            }

            text.SendKeys(contato.DefinirMensagem(mensagens));
            driver.SecureFindAndClick(By.XPath("//*[@id=\"main\"]/footer/div[1]/div/span[2]/div/div[2]/div[2]/button/span"));
            //text.SendKeys(OpenQA.Selenium.Keys.Enter);
            
            Thread.Sleep(TimeSpan.FromSeconds(2));
            
            //var sendButton = driver.FindElement(By.CssSelector("span[data-icon='send']"));
            //var sendButton = driver.FindElement(By.XPath("/html/body/div[1]/div/div/div[4]/div/footer/div[1]/div/span[2]/div/div[2]/div[2]/button/span"));
            //thread.Sleep(TimeSpan.FromSeconds(1));
            //sendButton.Click();
        }

        [STAThread]
        private static void EnviarArquivo(ChromeDriver driver, Contato contato, string arquivo, bool iphone)
        {
            AnexarButtonClick(driver);
            //IEnumerable<IWebElement> elements = null;
            /*while (elements?.Where(e => e.SecureGetAttribute("accept") == "*")?.FirstOrDefault() == null)
            {
                try
                {
                    elements = driver.FindElements(By.TagName("input"));
                }
                catch { }

                Thread.Sleep(TimeSpan.FromSeconds(1));
            }*/

            IWebElement input = null;
            if (iphone)
            {
                input = driver.SecureFind(By.XPath("/html/body/div[1]/div/div[2]/div[4]/div/footer/div[1]/div/span[2]/div/div[1]/div/div/span/div/ul/div/div[1]/li/div/input"));
            }
            else
            {                                 
                input = driver.SecureFind(By.XPath("/html/body/div[1]/div/div[2]/div[4]/div/footer/div[1]/div/span[2]/div/div[1]/div/div/span/div/ul/div/div[1]/li/div/input"));
            }


            input.SendKeys(arquivo);
            Thread.Sleep(2000);

            //driver.SecureFindAndSendKeys("{Enter}");
            //driver.SecureFindAndClick(By.XPath("//*[@id=\"app\"]/div/div/div[2]/div[2]/span/div/span/div/div/div[2]/div/div[2]/div[2]/div/div/span"));
            driver.SecureFindAndClick(By.XPath("/html/body/div[1]/div/div[2]/div[2]/div[2]/span/div/span/div/div/div[2]/div/div[2]/div[2]/div/div"));
            //driver.SecureFindAndClick(By.XPath("/html/body/div[1]/div/div/div[3]/div[2]/span/div/span/div/div/div[2]/div/div[2]/div[2]/div/div"));

            
            //driver.SecureFindAndClick(By.XPath("//*[@id=\"app\"]/div[1]/div[1]/div[2]/div[2]/span/div[1]/span/div[1]/div/div[2]/div/div[2]/div[2]"));
        }


        private static bool SetarContato(Contato c, ChromeDriver driver, IWebElement seachText, TimeSpan segundosDeProcura, TipoDeProcura tipoDeBusca)
        {
            seachText.ClearTextByKey();
            if (tipoDeBusca == TipoDeProcura.Nome)
            {
                seachText.SendKeys(c.Nome);
            }
            else if (tipoDeBusca == TipoDeProcura.Telefone)
            {
                if (c.Telefone.Length == 11)
                {
                    seachText.SendKeys($"{c.Telefone.Substring(0, 2)}{c.Telefone.Substring(3)}");
                    if (VerificandoContatoSelecionado(driver, c, segundosDeProcura))
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                        return true;
                    }
                }
                else if (c.Telefone.Length == 10)
                {
                    seachText.SendKeys(c.Telefone);
                    if (VerificandoContatoSelecionado(driver, c, segundosDeProcura))
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                        return true;
                    }
                }
            }
            else if (tipoDeBusca == TipoDeProcura.NomeETelefone) 
            {
                seachText.ClearTextByKey();
                if (c.Telefone.Length == 11)
                {
                    seachText.SendKeys($"{c.Telefone.Substring(0, 2)}{c.Telefone.Substring(3)}");
                    if (VerificandoContatoSelecionado(driver, c, segundosDeProcura))
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                        return true;
                    }
                }
                else if (c.Telefone.Length == 10)
                {
                    seachText.SendKeys(c.Telefone);
                    if (VerificandoContatoSelecionado(driver, c, segundosDeProcura))
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                        return true;
                    }
                }

                seachText.ClearTextByKey();
                seachText.SendKeys(c.Nome);           
            }
            Thread.Sleep(TimeSpan.FromSeconds(1));
            return VerificandoContatoSelecionado(driver, c, segundosDeProcura);
        }

        private static bool VerificandoContatoSelecionado(IWebDriver driver, Contato c, TimeSpan segundosDeProcura)
        {
            IEnumerable<IWebElement> contatos = null;
            var sair = false;
            while (contatos?.Where(e => e.SecureGetAttribute("title")?.ToLower().Trim() == c.Nome?.ToLower().Trim())?.FirstOrDefault() == null && !sair)
            {
                try
                {
                    var paneSide = driver.FindElement(By.Id("pane-side"));
                    contatos = paneSide?.FindElements(By.TagName("span"));
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

            var pSide = driver.FindElement(By.Id("pane-side"));
            contatos = pSide.FindElements(By.TagName("span"));
            contatos.Where(e => e.SecureGetAttribute("title")?.ToLower()?.Trim() == c.Nome.ToLower().Trim()).FirstOrDefault().Click();
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
                    seachText = driver.SecureFind(By.XPath("/html/body/div[1]/div/div[2]/div[3]/div/div[1]/div/div[2]/div[2]/div/div"), TimeSpan.FromSeconds(1));
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
                Cpf = cells[cpfColumn.Index].Value?.ToString(),
                Nome = cells[nomeColumn.Index].Value?.ToString(),
                Telefone = cells[telefoneColumn.Index].Value?.ToString(),
                ArquivosEnviados = (bool)cells[ArquivoEnviadoColumn.Index]?.Value,
                MensagemEnviada = (bool)cells[MensagemEnviadaColumn.Index]?.Value,
                ContatoEncontrado = (bool?)cells[ContatoEncontradoColumn.Index]?.Value ?? null
            };
        }

        private void pararExecucãoToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void reiniciarArquivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(UltimoArquivoCarregado))
                {
                    var contatos = new List<string>();
                    contatos.Add("Cpf;Nome;Celular;SucessoEnvioMsg;SucessoEnvioArquivos;ContatoEncontrado");

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


    public class ClipboardExample
    {
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        public const int KEYEVENTF_KEYUP = 0x0002;
        public const int VK_CONTROL = 0x11;
        public const int VK_V = 0x56;
        
        [STAThread]
        public static void CTRL_V()
        {
            Thread.Sleep(2000);

            keybd_event(VK_CONTROL, 0, 0, 0);
            keybd_event(VK_V, 0, 0, 0);
            keybd_event(VK_V, 0, KEYEVENTF_KEYUP, 0);
            keybd_event(VK_CONTROL, 0, KEYEVENTF_KEYUP, 0);
        }
    }
}