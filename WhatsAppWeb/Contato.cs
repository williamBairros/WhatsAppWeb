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
        public string Mensagem { get; internal set; }
        public bool MensagemEnviada { get; internal set; }
        public bool ArquivoEnviado { get; internal set; }


        public static List<string> Arquivos = null;
        public string BuscarArquivo(BuscarArquivo busca) 
        {
            if (Arquivos == null || (Arquivos?.Count ?? 0 ) == 0) 
            {
                Arquivos = Directory.GetFiles($"{Environment.CurrentDirectory}\\Arquivos").ToList();
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

    }
}