using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WhatsAppBot
{
    public class Contato
    {
        public string Telefone { get; set; }
        public string Mensagem { get; set; }

        public Contato() { }
        public Contato(object[] infs) 
        {
            Telefone = infs[0]?.ToString();
            Mensagem = infs[1]?.ToString();
        }

        public object[] ToRow() 
        {
            return new object[]
            {
                Telefone,
                Mensagem
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
    }
}