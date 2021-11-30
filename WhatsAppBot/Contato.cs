﻿using System;
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
        public string Mensagem1 { get; set; }
        public string Mensagem2 { get; set; }
        public string Mensagem3 { get; set; }
        public bool MensagemEnviada { get; set; }
        public bool ArquivosEnviados { get; set; }
        public bool? ContatoEncontrado { get; set; }

        public Contato() { }
        public Contato(object[] infs) 
        {
            Cpf = infs[0]?.ToString();
            Nome = infs[1]?.ToString();
            Telefone = infs[2]?.ToString();
            Mensagem1 = infs[3]?.ToString();
            Mensagem2 = infs[4]?.ToString();
            Mensagem3 = infs[5]?.ToString();
            if (infs.Length >= 7 && int.TryParse(infs[6]?.ToString(), out int i) && i == 1)
            {
                MensagemEnviada = true;
            }
            else 
            {
                MensagemEnviada = false;
            }

            if (infs.Length >= 8 && int.TryParse(infs[7]?.ToString(), out i) && i == 1)
            {
                ArquivosEnviados = true;
            }
            else
            {
                ArquivosEnviados = false;
            }

            if (infs.Length >= 9 && !string.IsNullOrEmpty(infs[8]?.ToString()))
            {
                if (int.TryParse(infs[8]?.ToString(), out i) && i == 1)
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
                Mensagem1,
                Mensagem2,
                Mensagem3,
                MensagemEnviada,
                ArquivosEnviados,
                ContatoEncontrado.HasValue ? ContatoEncontrado.Value : (bool?)null
            };
        }

        public static List<string> Arquivos = null;
        public List<string> BuscarArquivos(BuscarArquivos busca) 
        {
            if (Arquivos == null || (Arquivos?.Count ?? 0 ) == 0) 
            {
                Arquivos = Directory.GetFiles(busca.DiretorioArquivos).ToList();
            }

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