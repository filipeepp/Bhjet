using BHJet_Core.Enum;

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
        public TipoCarteira TipoCNH { get; set; }
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
        public TipoProfissional TipoProfissional { get; set; }
        public RegimeContratacao TipoRegime { get; set; }
    }
}
