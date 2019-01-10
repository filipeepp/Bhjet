using System.Configuration;

namespace BHJet_Servico
{
    public class ServicoRotas
    {
        //public static readonly string Base = @"http://bhjetapi.sa-east-1.elasticbeanstalk.com/api/";
        public static readonly string Base = ConfigurationManager.AppSettings["urlapibhjet"];

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
			public const string GetCorridaCliente = "/Corrida/cliente/{0}";
		}

        public class AreaAtuacao
        {
            public const string GetAreaAtuacao = "/AreaAtuacao";
        }

        public class Diaria
        {
            public const string PostDiaria = "/Diaria";
        }

        public class Cliente
        {
			#region Cliente Normal
			public const string GetClientes = "/Cliente";
			public const string PostCliente = "/Cliente";
			public const string PostClienteContato = "/Cliente/{0}/contato";
			public const string PostClienteValor = "/Cliente/{0}/contrato";
			public const string GetClienteContrato = "/Cliente/contrato";
			public const string GetClientesValorAtivo = "/Cliente/contrato/ativo";
			public const string GetClienteCompleto = "/Cliente/{0}";
			public const string PutCliente = "/Cliente/{0}";
			public const string DeleteContato = "/Cliente/contato/{0}";
			public const string DeleteValor = "/Cliente/contrato/{0}";
			#endregion

			#region Cliente Avulso
			public const string GetClientesAvulsosValorAtivo = "/Cliente/avulso/contrato/ativo";
			#endregion
		}

		public class Usuario
        {
            public const string GetUsuarios = "/Usuarios";
            public const string PostUsuario = "/Usuarios";
            public const string PutUsuario = "/Usuarios";
            public const string DeleteUsuario = "/Usuarios/{0}";
            public const string GetUsuario = "/Usuarios/{0}";
            public const string PutSituacao = "/Usuarios/situacao/{0}/usuario/{1}";
        }

        public class Tarifa
        {
            public const string GetTarifaCliente = "/Tarifa/cliente/{0}";
			public const string GetTarifaPadrao = "/Tarifa/padrao";
		}

        public class Faturamento
        {
            public const string PostFaturamento = "/Faturamento";
            public const string PostFaturamentoComum = "/Faturamento/comum";
            public const string GetFaturamentoDetalhe = "/Faturamento/detalhe";
        }

        public class Profissional
        {
            public const string PutProfissional = "/Profissional/{0}";
            public const string PostProfissional = "/Profissional";
            public const string GetProfissional = "/Profissional/{0}";
            public const string GetProfissionais = "/Profissional";
            public const string GetLocalizacoesProfissionais = "/Profissional/tipo/{0}/localizacao";
            public const string GetLocalizacaoProfissional = "/Profissional/{0}/localizacao";
            public const string GetComissaoProfissional = "/Profissional/{0}/comissao";
        }
    }
}
