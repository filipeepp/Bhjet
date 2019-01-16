using BHJet_Core.Enum;

namespace BHJet_DTO.Profissional
{
    public class PerfilModel
    {
        public long idUsuario { get; set; }
        public long idColaboradorEmpresaSistema { get; set; }
        public long idRegistroDiaria { get; set; }
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public TipoProfissional TipoProfissional { get; set; }
    }
}
