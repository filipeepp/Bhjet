namespace BHJet_Mobile.Infra.Variaveis
{
    public class ServicoRotas
    {
        public static readonly string Base = "http://bhjetapi.sa-east-1.elasticbeanstalk.com/api/";

        public class Autenticacao
        {
            public const string PostAutenticar = "/token";
        }

        public class Motorista
        {
            public const string GetPerfil = "/Profissional/perfil";
        }
    }
}
