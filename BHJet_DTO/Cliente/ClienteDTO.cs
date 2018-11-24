using BHJet_Core.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace BHJet_DTO.Cliente
{
	public class ClienteDTO
	{
		public int? ID { get; set; }
		public ClienteDadosCadastraisDTO DadosCadastrais { get; set; }
		public IEnumerable<ClienteContatoDTO> Contato { get; set; }
		public IEnumerable<ClienteValorDTO> Valor { get; set; }
	}

	public class ClienteDadosCadastraisDTO
	{
		public int Codigo { get; set; }
		public string NomeRazaoSocial { get; set; }
		public string NomeFantasia { get; set; }
		public string CPFCNPJ { get; set; }
		public string InscricaoEstadual { get; set; }
		public bool ISS { get; set; }
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

	public class ClienteContatoDTO
	{

		public string Contato { get; set; }
		public string Email { get; set; }
		public string TelefoneComercial { get; set; }
		public string TelefoneCelular { get; set; }
		public string Setor { get; set; }
		public DateTime DataNascimento { get; set; }
	}

	public class ClienteValorDTO
	{
		public string ValorUnitario { get; set; }
		public TipoTarifa TipoTarifa { get; set; }
		public DateTime VigenciaInicio { get; set; }
		public DateTime VigenciaFim { get; set; }
		public string Franquia { get; set; }
		public string FranquiaAdicional { get; set; }
		public string Observacao { get; set; }
	}

}
