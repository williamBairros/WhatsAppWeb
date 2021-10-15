using System.Collections.Generic;

namespace WhatsAppWeb
{
    public class BuscarArquivo
    {
        public string DiretorioArquivos { get; set; }
        public Dictionary<int,string> Campos { get; set; }
        public string Delimitador { get; set; }
    }
}