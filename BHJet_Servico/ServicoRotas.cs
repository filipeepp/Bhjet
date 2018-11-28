﻿namespace BHJet_Servico
{
    public class ServicoRotas
    {
        public static readonly string Base = @"http://bhjetapi.sa-east-1.elasticbeanstalk.com/api/";

        public class Autenticacao
        {
            public const string PostAutenticar = "/token";
        }

        public class Dashboard
        {
            public const string GetResumo = "/Dashboard/resumo";
            public const string GetResumoChamadosSit = "/Dashboard/resumo/chamado";
            public const string GetResumoAtendimentosCategoria = "/Dashboard/resumo/atendimento";
        }

        public class Corrida
        {
            public const string GetDetalheCorridas = "/Corrida/{0}";
            public const string GetLocalizacaoCorridas = "/Corrida/status/{0}/profissional/{1}/localizacao";
        }

        public class Diaria
        {
            public const string PostDiaria = "/Diaria";
        }

        public class Cliente
        {
            public const string GetClientes = "/Cliente";
        }

        public class Tarifa
        {
            public const string GetTarifaCliente = "/Tarifa/cliente/{0}";
        }

        public class Faturamento
        {
            public const string PostFaturamento = "/Faturamento";
            public const string PostFaturamentoComum = "/Faturamento/comum";
        }

        public class Profissional
        {
            public const string PutProfissional = "/Profissional/{0}";
            public const string PostProfissional = "/Profissional";
            public const string GetProfissional = "/Profissional/{0}";
            public const string GetProfissionais = "/Profissional";
            public const string GetLocalizacoesProfissionais = "/Profissional/profissional/tipo/{0}/localizacao";
            public const string GetLocalizacaoProfissional = "/Profissional/profissional/{0}/localizacao";
        }
    }
}
