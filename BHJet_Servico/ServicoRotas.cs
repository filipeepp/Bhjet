namespace BHJet_Servico
{
    public class ServicoRotas
    {
        public static readonly string Base = @"http://apibhjetapi.sa-east-1.elasticbeanstalk.com";

        public class Autenticacao
        {
            public const string PostAutenticar = "/token";
        }

        public class Dashboard
        {
            public const string GetResumo = "/api/Dashboard/resumo";
            public const string GetResumoChamadosSit = "/api/Dashboard/resumo/chamado";
            public const string GetResumoAtendimentosCategoria = "/api/Dashboard/resumo/atendimento";
        }

        public class Corrida
        {
            public const string GetDetalheCorridas = "/api/Corrida/{0}/";
            public const string GetLocalizacaoCorridas = "/api/Corrida/status/{0}/profissional/{1}/localizacao";
        }

        public class Profissional
        {
            public const string PutProfissional = "/api/Profissional/{0}";
            public const string GetProfissional = "/api/Profissional/{0}";
            public const string GetProfissionais = "/api/Profissional";
            public const string GetLocalizacoesProfissionais = "/api/Profissional/profissional/tipo/{0}/localizacao";
            public const string GetLocalizacaoProfissional = "/api/Profissional/profissional/{0}/localizacao";
        }
    }
}
