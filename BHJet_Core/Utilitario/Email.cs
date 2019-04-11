using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace BHJet_Core.Utilitario
{
    public static class Email
    {

        public static void EnviaMensagemEmail(string Destinatario, string Assunto, string Mensagem)
        {
            // Cnfiguracao
            var config = BuscaConfiguracao();

            // Mensagem
            MailMessage mensagemEmail = new MailMessage(config.Remetente, Destinatario, Assunto, Mensagem);

            // Smtp Cliente
            SmtpClient client = new SmtpClient(config.Servidor, config.Porta);
            client.EnableSsl = true;

            // Network
            NetworkCredential cred = new NetworkCredential(config.Usuario, config.Senha);
            client.Credentials = cred;

            // envia a mensagem
            client.Send(mensagemEmail);
        }

        private static EmailConfigSection BuscaConfiguracao()
        {
            return ConfigurationManager.GetSection("EmailConfigSection") as EmailConfigSection;
        }
    }

    public class EmailConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("Remetente", IsKey = true, IsRequired = true)]
        public string Remetente
        {
            get { return (string)base["Remetente"]; }
            set { base["Remetente"] = value; }
        }

        [ConfigurationProperty("Servidor", IsRequired = true)]
        public string Servidor
        {
            get { return (string)base["Servidor"]; }
            set { base["Servidor"] = value; }
        }

        [ConfigurationProperty("Porta", IsRequired = true)]
        public int Porta
        {
            get { return (int)base["Porta"]; }
            set { base["Porta"] = value; }
        }

        [ConfigurationProperty("Usuario", IsRequired = true)]
        public string Usuario
        {
            get { return (string)base["Usuario"]; }
            set { base["Usuario"] = value; }
        }

        [ConfigurationProperty("Senha", IsRequired = true)]
        public string Senha
        {
            get { return (string)base["Senha"]; }
            set { base["Senha"] = value; }
        }
    }
}
