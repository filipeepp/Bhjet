using System.Configuration;

namespace BHJet_Servico
{
    public class ServicoRotas
    {
        public static readonly string Base = ConfigurationManager.AppSettings["urlapibhjet"];

        public class Autenticacao
        {
            public const string PostAutenticar = "/token";
        }

        public class Dashboard
        {
            public const string GetResumo = "api/Dashboard/resumo";
            public const string GetResumoChamadosSit = "api/Dashboard/resumo/chamado";
            public const string GetResumoAtendimentosCategoria = "api/Dashboard/resumo/atendimento";
        }

        public class Corrida
        {
            public const string GetDetalheCorridas = "api/Corrida/{0}";
            public const string GetLocalizacaoCorridas = "api/Corrida/status/{0}/profissional/{1}/localizacao";
			public const string GetCorridaCliente = "api/Corrida/cliente/{0}";
            public const string GetOcorrencia = "api/Corrida/ocorrencias";
            public const string GetPreco = "api/Corrida/preco";
            public const string PostCorrida = "api/Corrida";
        }

        public class AreaAtuacao
        {
            public const string GetAreaAtuacao = "api/AreaAtuacao";
        }

        public class Diaria
        {
            public const string PostDiaria = "api/Diaria";
        }

        public class Cliente
        {
			#region Cliente Normal
			public const string GetClientes = "api/Cliente";
			public const string PostCliente = "api/Cliente";
            public const string PostClienteAvulso = "api/Cliente/avulso";
            public const string PostClienteContato = "api/Cliente/{0}/contato";
			public const string PostClienteValor = "api/Cliente/{0}/contrato";
			public const string GetClienteContrato = "api/Cliente/contrato";
			public const string GetClientesValorAtivo = "api/Cliente/contrato/ativo";
			public const string GetClienteCompleto = "api/Cliente/{0}";
			public const string PutCliente = "api/Cliente/{0}";
			public const string DeleteContato = "api/Cliente/contato/{0}";
			public const string DeleteValor = "api/Cliente/contrato/{0}";
			#endregion

			#region Cliente Avulso
			public const string GetClientesAvulsosValorAtivo = "api/Cliente/avulso/contrato/ativo";
			#endregion
		}

		public class Usuario
        {
            public const string GetUsuarios = "api/Usuarios";
            public const string PostUsuario = "api/Usuarios";
            public const string PutUsuario = "api/Usuarios";
            public const string DeleteUsuario = "api/Usuarios/{0}";
            public const string GetUsuario = "api/Usuarios/{0}";
            public const string PutSituacao = "api/Usuarios/situacao/{0}/usuario/{1}";
            public const string GetPerfil = "api/Usuarios/perfil";
        }

        public class Tarifa
        {
            public const string GetTarifarioPadrao = "api/Tarifa";
            public const string PutTarifarioPadrao = "api/Tarifa";
            public const string GetTarifaCliente = "api/Tarifa/cliente";
			public const string GetTarifaPadrao = "api/Tarifa/tipoVeiculo/{0}";
		}

        public class Faturamento
        {
            public const string PostFaturamento = "api/Faturamento";
            public const string PostFaturamentoComum = "api/Faturamento/comum";
            public const string GetFaturamentoDetalhe = "api/Faturamento/detalhe";
        }

        public class Profissional
        {
            public const string PutProfissional = "api/Profissional/{0}";
            public const string PostProfissional = "api/Profissional";
            public const string GetProfissional = "api/Profissional/{0}";
            public const string GetProfissionais = "api/Profissional";
            public const string GetProfissionaisDisponiveis = "api/Profissional/Disponivel";
            public const string GetLocalizacoesProfissionais = "api/Profissional/tipo/{0}/localizacao";
            public const string GetLocalizacaoProfissional = "api/Profissional/{0}/localizacao";
            public const string GetComissaoProfissional = "api/Profissional/{0}/comissao";
            public const string GetTipoVeiculos = "api/Profissional/veiculo/tipos";
        }
    }
}
