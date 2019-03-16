using BHJet_Enumeradores;
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
		public string vcCEP { get; set; }
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
    }

	public class ClienteCompletoModel
	{
		public long ID { get; set; }
		public ClienteDadosCadastraisModel DadosCadastrais { get; set; }
		public IEnumerable<ClienteContatoModel> Contato { get; set; }
        public ClienteValorModel ContratoCarro { get; set; }
        public ClienteValorModel ContratoMoto { get; set; }
    }

	public class ClienteDadosCadastraisModel
	{
        public bool Avulso { get; set; }
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
		public int ID { get; set; }
		public string Contato { get; set; }
		public string Email { get; set; }
		public string TelefoneComercial { get; set; }
		public string TelefoneCelular { get; set; }
		public int TelefoneWhatsapp { get; set; }
		public string Setor { get; set; }
		public DateTime? DataNascimento { get; set; }
	}

	public class ClienteValorModel
	{
        public int? idTarifario { get; set; }
        public decimal? ValorContrato { get; set; }
        public int? FranquiaKM { get; set; }
        public decimal? ValorKMAdicional { get; set; }
        public int? FranquiaHoras { get; set; }
        public decimal? ValorHoraAdicional { get; set; }
        public int? FranquiaMinutosParados { get; set; }
        public decimal? ValorMinutoParado { get; set; }
        public string Observacao { get; set; }
    }

}
