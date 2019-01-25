﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHJet_Repositorio.Admin.Entidade
{
    public class ClienteEntidade
    {
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

	}

	public class ClienteCompletoEntidade
	{
		public long ID { get; set; }
		public ClienteDadosCadastraisEntidade DadosCadastrais { get; set; }
		public IEnumerable<ClienteContatoEntidade> Contato { get; set; }
		public IEnumerable<ClienteValorEntidade> Valor { get; set; }

	}

	public class ClienteDadosCadastraisEntidade
	{
		public string NomeRazaoSocial { get; set; }
		public string NomeFantasia { get; set; }
		public string CPFCNPJ { get; set; }
		public string InscricaoMunicipal { get; set; }
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
		public int Avulso { get; set; }
	}


	public class ClienteContatoEntidade
	{
		public int ID { get; set; }
		public string Contato { get; set; }
		public string Email { get; set; }
		public string TelefoneRamal { get; set; }
		public string TelefoneComercial { get; set; }
		public string TelefoneCelular { get; set; }
		public string Setor { get; set; }
		public int Whatsapp { get; set; }
		public DateTime? DataNascimento { get; set; }
	}

	public class ClienteValorEntidade
	{
		public int ID { get; set; }
		public int ValorAtivado { get; set; }
		public int PagamentoAVista { get; set; }
		public decimal ValorUnitario { get; set; }
		public string TipoTarifa { get; set; }
		public DateTime VigenciaInicio { get; set; }
		public DateTime VigenciaFim { get; set; }
		public decimal ValorBandeirada { get; set; }
		public decimal ValorKMBandeiradada { get; set; }
		public decimal ValorKMAdicionalCorrida { get; set; }
		public int MinutosParado { get; set; }
		public decimal ValorMinutosParado { get; set; }
		public DateTime HorarioInicial { get; set; }
		public DateTime HorarioFinal { get; set; }
		public decimal ValorDiaria { get; set; }
		public decimal ValorKMDiaria { get; set; }
		public decimal ValorKMAdicionalDiaria { get; set; }
		public decimal Franquia { get; set; }
		public decimal FranquiaAdicional { get; set; }
		public string Observacao { get; set; }
	}
}
