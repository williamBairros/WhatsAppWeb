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
    }
}