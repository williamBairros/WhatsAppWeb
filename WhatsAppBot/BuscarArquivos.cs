using System.Collections.Generic;

namespace WhatsAppBot
{
    public class BuscarArquivos
    {
        public string DiretorioArquivos { get; set; }
        public string Delimitador { get; set; }
        public Dictionary<int, string> Campos { get; set; }
    }
}
