namespace BHJet_Mobile.Infra.Variaveis
{
    public class ServicoRotas
    {
       // public static readonly string Base = "http://bhjetapi.sa-east-1.elasticbeanstalk.com/api/";
        public static readonly string Base = "http://bhjet.jrmendonca.com.br/api/";

        public class Autenticacao
        {
            public const string PostAutenticar = "/token";
        }

        public class Motorista
        {
            public const string GetPerfil = "api/Profissional/perfil";
            public const string GetDadosBasicos = "api/Profissional/{0}";
            public const string PutDadosBasicos = "api/Profissional/{0}/basico";
            public const string PutDisponibilidade = "api/Profissional/Disponivel";
        }
        public class Diaria
        {
            public const string GetVerificacaoAberta = "api/Diaria/turno/verifica";
            public const string GetTurno = "api/Diaria/turno";
            public const string PostTurno = "api/Diaria/turno";
        }

        public class Corrida
        {
            public const string GetAberta = "api/Corrida/aberta/{0}/{1}";
            public const string GetLog = "api/Corrida/log/{0}";
            public const string PostProtocolo = "api/Corrida/protocolo/endereco/{0}";
            public const string PutChegada = "api/Corrida/chegada/{0}/";
            public const string GetOcorrencias = "api/Corrida/ocorrencias";
            public const string PutEncerrarCorrida = "api/Corrida/encerrar/{0}/ocorrencia/{1}";
            public const string PutOcorrenciaCorrida = "api/Corrida/ocorrencias/{0}/log/{1}/corrida/{2}";
            public const string PostRecusarCorrida = "api/Corrida/recusar/{0}";
            public const string PostLiberarCorrida = "api/Corrida/liberar/{0}";
        }
    }
}
