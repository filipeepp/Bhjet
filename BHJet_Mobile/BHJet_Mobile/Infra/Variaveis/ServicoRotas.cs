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
            public const string GetDadosBasicos = "/Profissional/{0}";
            public const string PutDadosBasicos = "/Profissional/{0}/basico";
            public const string PutDisponibilidade = "/Profissional/Disponivel";
        }
        public class Diaria
        {
            public const string GetVerificacaoAberta = "/Diaria/turno/verifica";
            public const string GetTurno = "/Diaria/turno";
            public const string PostTurno = "/Diaria/turno";
        }

        public class Corrida
        {
            public const string GetAberta = "/Corrida/aberta/{0}";
        }
    }
}
