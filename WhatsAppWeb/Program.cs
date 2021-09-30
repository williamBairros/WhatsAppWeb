using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace WhatsAppWeb
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
                var contatos = CarregarContatos().Where(c => !c.ArquivosEnviados || !c.MensagemEnviada).ToList();

                using (var driver = new ChromeDriver())
                {
                    driver.Navigate().GoToUrl("https://web.whatsapp.com/");
                    var seachText = WaitLogin(driver);

                    contatos.ForEach(c =>
                    {
                        var tentativas = 3;
                        while (tentativas > 0)
                        {
                            try
                            {
                                SetarContato(c, driver, seachText);
                                if (!string.IsNullOrEmpty(c.DefinirMensagem()) && !c.MensagemEnviada)
                                {
                                    EnviadoMensagem(driver, c);
                                    c.MensagemEnviada = true;
                                    AtualizarEnvioContato(c);
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
                                    AtualizarEnvioContato(c);
                                }

                                break;
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

                        Thread.Sleep(TimeSpan.FromSeconds(new Random().Next(config.IntervaloMin, config.IntervaloMax+1)));
                    });
                }
            }
            catch(Exception ex) 
            {
                ExceptionToFile(ex);
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

        private static void AtualizarEnvioContato(Contato c)
        {
            var linhas = File.ReadAllLines("contatos.csv", Encoding.UTF8).ToList();
            var index = linhas.FindIndex(l => l.Split(';')[0] == c.Cpf);
            linhas[index] = $"{c.Cpf};{c.Nome};{c.Telefone};{c?.Mensagem1};{c.Mensagem2};{c.Mensagem3};{(c.MensagemEnviada ? "1" : "0")};{(c.ArquivosEnviados ? "1": "0")}";
            File.WriteAllLines("contatos.csv", linhas, Encoding.UTF8);
        }

        private static void SetarContato(Contato c, ChromeDriver driver, IWebElement seachText)
        {
            seachText.Clear();
            seachText.SendKeys(c.Nome);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            VerificandoContatoSelecionado(driver, c);
        }

        private static void EnviarArquivo(ChromeDriver driver, Contato contato, string arquivo)
        {
            AnexarButtonClick(driver);
            IEnumerable<IWebElement> elements = null;
            while (elements?.Where(e => e.SecureGetAttribute("accept") == "*")?.FirstOrDefault() == null)
            {
                Console.Clear();
                try
                {
                    elements = driver.FindElements(By.TagName("input"));
                }
                catch { }

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            }

            var input = driver.SecureFind(By.CssSelector("input[type='file']"));
            input.SendKeys(arquivo);
            driver.SecureFindAndClick(By.CssSelector("span[data-icon='send']"));
        }

        private static void EnviadoMensagem(ChromeDriver driver, Contato contato)
        {
            driver.SecureFindAndSendKeys(By.XPath("/html/body/div[1]/div[1]/div[1]/div[4]/div[1]/footer/div[1]/div/div/div[2]/div[1]/div/div[2]"), contato.DefinirMensagem());
            driver.SecureFindAndClick(By.CssSelector("span[data-icon='send']"));
        }

        private static void VerificandoContatoSelecionado(IWebDriver driver, Contato c)
        {
            IEnumerable<IWebElement> contatos = null;
            while (contatos?.Where(e => e.SecureGetAttribute("Title") == c.Nome)?.FirstOrDefault() == null)
            {
                try
                {
                    contatos = driver.FindElements(By.TagName("span"));
                }
                catch { }

                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            contatos = driver.FindElements(By.TagName("span"));
            contatos.Where(e => e.SecureGetAttribute("Title") == c.Nome).FirstOrDefault().Click();
        }

        private static void AnexarButtonClick(IWebDriver driver)
        {
            driver.SecureFindAndClick(By.CssSelector("span[data-icon='clip']"));
        }

        private static List<Contato> CarregarContatos()
        {
            var primeraLinha = true;
            var linhas = File.ReadAllLines("contatos.csv", Encoding.UTF8);
            var ret = new List<Contato>();
            foreach (string linha in linhas) 
            {
                if (primeraLinha) 
                {
                    primeraLinha = false;
                    continue;
                }
                var campos = linha.Split(new char[] { ';' });

                var contato = new Contato() { Cpf = campos[0], Nome = campos[1], Telefone = campos[2] };
                if (campos.Length > 3) 
                {
                    contato.Mensagem1 = campos[3];
                }
                if (campos.Length > 4)
                {
                    contato.Mensagem2 = campos[4];
                }
                if (campos.Length > 5)
                {
                    contato.Mensagem3 = campos[5];
                }
                if (campos.Length > 6) 
                {
                    contato.MensagemEnviada = campos[6]?.Trim() == "1";
                }
                if (campos.Length > 7)
                {
                    contato.ArquivosEnviados = campos[7]?.Trim() == "1";
                }

                ret.Add(contato);
            }
            return ret;
        }

        public static IWebElement WaitLogin(IWebDriver driver) 
        {
            IWebElement seachText = null;
            while (seachText == null) 
            {
                Console.Clear();
                Console.WriteLine("Aguardando login");
                try
                {
                    seachText = driver.SecureFind(By.XPath("/html/body/div[1]/div[1]/div[1]/div[3]/div/div[1]/div/label/div/div[2]"), TimeSpan.FromSeconds(1));
                }
                catch { }

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            return seachText;
        }
    }
}
