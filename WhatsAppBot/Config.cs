using System.Collections.Generic;
using WhatsAppBot.Enuns;

namespace WhatsAppBot
{
    public class Config
    {
        public int IntervaloMin { get; set; }
        public int IntervaloMax { get; set; }
        public int SegundosDeProcura { get; set; }
        public TipoDeProcura TipoDeProcura { get; set; }
        public BuscarArquivos BuscarArquivos { get; set; }
        public List<string> Mensagens { get; set; }
        public bool Iphone { get; set; }
    }
}
