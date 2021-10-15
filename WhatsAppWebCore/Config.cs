using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsAppWeb
{
    public class Config
    {
        public int IntervaloMin { get; set; }
        public int IntervaloMax { get; set; }
        public int SegundosDeProcura { get; set; }
        public TipoDeProcura TipoDeProcura { get; set; }
        public BuscarArquivo BuscarArquivos { get; set; } 
    }

    public enum TipoDeProcura
    { 
        Nome = 0,
        Telefone = 1,
    }
}
