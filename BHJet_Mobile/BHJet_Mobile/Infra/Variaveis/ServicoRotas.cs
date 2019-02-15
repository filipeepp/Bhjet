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
            public const string GetAberta = "/Corrida/aberta/{0}/{1}";
            public const string GetLog = "/Corrida/log/{0}";
            public const string PostProtocolo = "/Corrida/protocolo/endereco/{0}";
            public const string PutChegada = "/Corrida/chegada/{0}/";
            public const string GetOcorrencias = "/Corrida/ocorrencias";
            public const string PutEncerrarCorrida = "/Corrida/encerrar/{0}/ocorrencia/{1}";
            public const string PutOcorrenciaCorrida = "/Corrida/ocorrencias/{0}/log/{1}/corrida/{2}";
            public const string PostRecusarCorrida = "/Corrida/recusar/{0}";
            public const string PostLiberarCorrida = "/Corrida/liberar/{0}";
        }
    }
}
