using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GerarVcf
{
    class Program
    {
        static void Main(string[] args)
        {
            var linhas = File.ReadAllLines($"{Environment.CurrentDirectory}\\contatos.csv", Encoding.UTF8);
            var vcfCampos = new List<string>();
            var primeraLinha = true;
            foreach (string linha in linhas) 
            {
                if (primeraLinha)
                {
                    primeraLinha = false;
                    continue;
                }
                var campos = linha.Split(new char[] {';'});
                vcfCampos.Add(AddVcf(campos));
            }
            File.WriteAllLines($"{DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}.vcf", vcfCampos);
        }

        private static string AddVcf(string[] campos)
        {
            var sb = new StringBuilder();
            sb.AppendLine("BEGIN: VCARD");
            sb.AppendLine("VERSION:3.0");
            var nomeSeparado = campos[1].Split(' ').ToList();
            nomeSeparado.Reverse();
            sb.AppendLine($"N:{string.Join(";", nomeSeparado)}");
            sb.AppendLine($"FN:{campos[1].Split(' ')[0]}");
            sb.AppendLine($"TEL;TYPE=CELL:{campos[2]}");
            sb.Append("END:VCARD");
            return sb.ToString();
        }
    }
}
