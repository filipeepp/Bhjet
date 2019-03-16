using BHJet_Enumeradores;
using System;

namespace BHJet_DTO.Profissional
{
    public class ProfissionalModel
    {
        public long ID { get; set; }
        public string NomeCompleto { get; set; }
        public TipoProfissional TipoProfissional { get; set; }
        public RegimeContratacao TipoRegime { get; set; }
    }

    public class ProfissionalCompletoModel
    {
        public long ID { get; set; }
        public string NomeCompleto { get; set; }
        public string CPF { get; set; }
        public string CNH { get; set; }

        public string Veiculos { get => "1,2"; }

        public TipoCarteira TipoCNH { get; set; }
        public int[] TipoVeiculos { get; set; }
        public string Cep { get; set; }
        public string Rua { get; set; }
        public string RuaNumero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string PontoReferencia { get; set; }
        public bool EnderecoPrincipal { get; set; }
        public string TelefoneResidencial { get; set; }
        public string TelefoneCelular { get; set; }
        public bool CelularWpp { get; set; }
        public string Email { get; set; }
        public bool ContratoCLT { get; set; }
        public string Observacao { get; set; }
        public string DocumentoRG { get; set; }
        public string Senha { get; set; }
        public bool Status { get; set; }
        public TipoProfissional TipoProfissional { get; set; }
        public RegimeContratacao TipoRegime { get; set; }
        public ProfissionalComissaoModel[] Comissoes { get; set; }
    }


    public class ProfissionalComissaoModel
    {
        public long? ID { get; set; }
        public decimal decPercentualComissao { get; set; }
        public DateTime dtDataInicioVigencia { get; set; }
        public DateTime dtDataFimVigencia { get; set; }
        public string Observacao { get; set; }
    }

}
