using BHJet_Core.Enum;
using System;
using System.Collections.Generic;

namespace BHJet_DTO.Cliente
{
    public class ClienteDTO
	{
		public long? ID { get; set; }
        public int idEndereco { get; set; }
        public int idUsuario { get; set; }
        public string vcNomeRazaoSocial { get; set; }
        public string vcNomeFantasia { get; set; }
        public string vcCPFCNPJ { get; set; }
        public string vcInscricaoMunicipal { get; set; }
        public string vcInscricaoEstadual { get; set; }
        public bool bitRetemISS { get; set; }
        public string vcObservacoes { get; set; }
        public string vcSite { get; set; }
        public IEnumerable<ClienteContatoModel> Contato { get; set; }
		public IEnumerable<ClienteValorModel> Valor { get; set; }
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
		public string ValorUnitario { get; set; }
		public TipoTarifa TipoTarifa { get; set; }
		public DateTime VigenciaInicio { get; set; }
		public DateTime VigenciaFim { get; set; }
		public string Franquia { get; set; }
		public string FranquiaAdicional { get; set; }
		public string Observacao { get; set; }
	}

}
