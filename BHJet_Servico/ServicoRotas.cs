namespace BHJet_Servico
{
    public class ServicoRotas
    {
        public static readonly string Base = @"http://localhost:50435";

        public class Autenticacao
        {
            public static readonly string PostAutenticar = "/token";
        }

        public class Dashboard
        {
            public static readonly string GetResumo = "/api/Dashboard/resumo";
            public static readonly string GetLocalizacaoFuncionario = "/api/Dashboard/profissional/{0}/localizacao";
            public static readonly string GetLocalizacaoCorridas = "/api/Dashboard/corridas/{0}/profissional/{1}/localizacao";
        }

    }
}
