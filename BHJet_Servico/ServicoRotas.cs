namespace BHJet_Servico
{
    public class ServicoRotas
    {
        public static readonly string Base = @"http://localhost:50435";

        public class Autenticacao
        {
            public const string PostAutenticar = "/token";
        }

        public class Dashboard
        {
            public const string GetResumo = "/api/Dashboard/resumo";
        }

        public class Corrida
        {
            public const string GetDetalheCorridas = "/api/Corrida/id/{0}/";
            public const string GetLocalizacaoCorridas = "/api/Corrida/status/{0}/profissional/{1}/localizacao";
        }

        public class Profissional
        {
            public const string GetLocalizacoesProfissionais = "/api/Profissional/profissional/tipo/{0}/localizacao";
            public const string GetLocalizacaoProfissional = "/api/Profissional/profissional/id/{0}/localizacao";
        }
    }
}
