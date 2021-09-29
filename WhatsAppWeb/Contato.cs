using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WhatsAppWeb
{
    public class Contato
    {
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Mensagem1 { get; set; }
        public string Mensagem2 { get; set; }
        public string Mensagem3 { get; set; }
        public bool MensagemEnviada { get; set; }
        public bool ArquivoEnviado { get; set; }


        public static List<string> Arquivos = null;
        public string BuscarArquivo(BuscarArquivo busca) 
        {
            if (Arquivos == null || (Arquivos?.Count ?? 0 ) == 0) 
            {
                Arquivos = Directory.GetFiles(busca.DiretorioArquivos).ToList();
            }

            return Arquivos.Find(f =>
            {
                var camposArquivos = Path.GetFileNameWithoutExtension(f).Split(busca.Delimitador.ToCharArray());

                foreach (var kvp in busca.Campos) 
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

        public string DefinirMensagem()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();

            if (!string.IsNullOrEmpty(Mensagem1))
            {
                dic.Add(1, Mensagem1);
            }
            
            if (!string.IsNullOrEmpty(Mensagem2))
            {
                dic.Add(2, Mensagem2);
            }
            
            if (!string.IsNullOrEmpty(Mensagem3)) 
            {
                dic.Add(3, Mensagem3);
            }

            if (dic.Count > 0) 
            {
                return dic[new Random().Next(dic.Keys.Min(), dic.Keys.Max()+1)];
            }

            return null;
        }
    }
}