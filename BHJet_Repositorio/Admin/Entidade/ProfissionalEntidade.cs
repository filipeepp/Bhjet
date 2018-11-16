using BHJet_Core.Enum;

namespace BHJet_Repositorio.Admin.Entidade
{
    public class ProfissionalEntidade
    {
        public long ID { get; set; }
        public string NomeCompleto { get; set; }
        public TipoProfissional TipoProfissional { get; set; }
        public RegimeContratacao TipoContrato { get; set; }
    }

    public class ProfissionalCompletoEntidade
    {
        public long ID { get; set; }
        public string NomeCompleto { get; set; }
        public string CPF { get; set; }
        public string CNH { get; set; }
        public TipoCarteira TipoCNH { get; set; }
        public string EnderecoCompleto { get; set; }
        public string TelefoneResidencial { get; set; }
        public string TelefoneCelular { get; set; }
        public bool CelularWpp { get; set; }
        public string Email { get; set; }
        public bool ContratoCLT { get; set; }
        public string Observacao { get; set; }
        public TipoProfissional TipoProfissional { get; set; }
        public RegimeContratacao TipoContrato { get; set; }
    }
}
