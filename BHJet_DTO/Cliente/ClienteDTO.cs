using BHJet_Core.Enum;
using System;
using System.Collections.Generic;

namespace BHJet_DTO.Cliente
{
    public class ClienteDTO
	{
		public int ID { get; set; }
		public int idCliente { get; set; }
		public int idEndereco { get; set; }
		public string vcRua { get; set; }
		public string vcNumero { get; set; }
		public string vcComplemento { get; set; }
		public string vcBairro { get; set; }
		public string vcCidade { get; set; }
		public string vcUF { get; set; }
		public int idUsuario { get; set; }
		public string vcNomeRazaoSocial { get; set; }
		public string vcNomeFantasia { get; set; }
		public string vcCPFCNPJ { get; set; }
		public string vcInscricaoMunicipal { get; set; }
		public string vcInscricaoEstadual { get; set; }
		public bool bitRetemISS { get; set; }
		public string vcObservacoes { get; set; }
		public string vcSite { get; set; }
		public int bitAtivo { get; set; }
		public string vcDescricaoTarifario { get; set; }
		public IEnumerable<ClienteContatoModel> Contato { get; set; }
		public IEnumerable<ClienteValorModel> Valor { get; set; }
	}

	public class ClienteCompletoModel
	{
		public ClienteDadosCadastraisModel DadosCadastrais { get; set; }
		public IEnumerable<ClienteContatoModel> Contato { get; set; }
		public IEnumerable<ClienteValorModel> Valor { get; set; }

	}

	public class ClienteDadosCadastraisModel
	{
		public string NomeRazaoSocial { get; set; }
		public string NomeFantasia { get; set; }
		public string CPFCNPJ { get; set; }
		public string InscricaoEstadual { get; set; }
		public int ISS { get; set; }
		public string Endereco { get; set; }
		public string NumeroEndereco { get; set; }
		public string Complemento { get; set; }
		public string Bairro { get; set; }
		public string Cidade { get; set; }
		public string Estado { get; set; }
		public string CEP { get; set; }
		public string Observacoes { get; set; }
		public string HomePage { get; set; }
	}


	public class ClienteContatoModel
	{
		public string Contato { get; set; }
		public string Email { get; set; }
		public string TelefoneComercial { get; set; }
		public string TelefoneCelular { get; set; }
		public string Setor { get; set; }
		public DateTime DataNascimento { get; set; }
	}

	public class ClienteValorModel
	{
		public int ValorAtivado { get; set; }
		public decimal ValorUnitario { get; set; }
		public string TipoTarifa { get; set; }
		public DateTime VigenciaInicio { get; set; }
		public DateTime VigenciaFim { get; set; }
		public decimal Franquia { get; set; }
		public decimal FranquiaAdicional { get; set; }
		public string Observacao { get; set; }
	}

}
