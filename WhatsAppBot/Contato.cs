using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WhatsAppBot
{
    public class Contato
    {
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public bool MensagemEnviada { get; set; }
        public bool ArquivosEnviados { get; set; }
        public bool? ContatoEncontrado { get; set; }

        public Contato() { }
        public Contato(object[] infs) 
        {
            Cpf = infs[0]?.ToString();
            Nome = infs[1]?.ToString();
            Telefone = infs[2]?.ToString();
            if (infs.Length >= 4 && int.TryParse(infs[3]?.ToString(), out int i) && i == 1)
            {
                MensagemEnviada = true;
            }
            else 
            {
                MensagemEnviada = false;
            }

            if (infs.Length >= 5 && int.TryParse(infs[4]?.ToString(), out i) && i == 1)
            {
                ArquivosEnviados = true;
            }
            else
            {
                ArquivosEnviados = false;
            }

            if (infs.Length >= 6 && !string.IsNullOrEmpty(infs[5]?.ToString()))
            {
                if (int.TryParse(infs[5]?.ToString(), out i) && i == 1)
                {
                    ContatoEncontrado = true;
                }
                else
                {
                    ContatoEncontrado = false;
                }
            }

        }

        public object[] ToRow() 
        {
            return new object[]
            {
                Cpf,
                Nome,
                Telefone,
                MensagemEnviada,
                ArquivosEnviados,
                ContatoEncontrado.HasValue ? ContatoEncontrado.Value : (bool?)null
            };
        }

        public static List<string> Arquivos = new List<string>();
        public List<string> BuscarArquivos(BuscarArquivos busca) 
        {
            try
            {
                Directory.CreateDirectory(busca.DiretorioArquivos);
                if ((Arquivos?.Count ?? 0) == 0)
                {
                    Arquivos = Directory.GetFiles(busca.DiretorioArquivos).ToList();
                }
            }
            catch { return new List<string>(); }


            return Arquivos.FindAll(f =>
            {
                var camposArquivos = Path.GetFileNameWithoutExtension(f).Split(busca.Delimitador.ToCharArray());

                foreach (var kvp in busca.Campos.Where(kvp => kvp.Value != "NADA")) 
                {
                    var valorArquivo = camposArquivos[kvp.Key];
                    var valorContato = GetPropertyValue(kvp.Value);

                    if (kvp.Value.ToLower() == "nome") 
                    {
                        valorArquivo = valorArquivo.Replace(" ", "");
                        valorContato = valorContato.Replace(" ", "");
                    }

                    if (valorArquivo.ToLower() != valorContato.ToLower()) 
                    {
                        return false;
                    }
                }

                return true;
            });

        }
        public string GetPropertyValue(string propertyName) 
        {
            var property = GetType().GetProperties().Where(p => p.Name.ToLower() == propertyName.ToLower()).FirstOrDefault();
            return property?.GetValue(this)?.ToString();
        }
        public string DefinirMensagem(List<string> mensagens)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();

            for (int i = 0; i < mensagens.Count; i++) 
            {
                if (!string.IsNullOrEmpty(mensagens[i]))
                {
                    dic.Add(i+1, mensagens[i]);
                }
            }

            if (dic.Count > 0) 
            {
                return dic[new Random().Next(dic.Keys.Min(), dic.Keys.Max()+1)];
            }

            return null;
        }
    }
}