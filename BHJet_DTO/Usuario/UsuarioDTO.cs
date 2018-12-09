using BHJet_Core.Enum;

namespace BHJet_DTO.Usuario
{
    public class UsuarioDTO
    {
        public long ID { get; set; }
        public string Email { get; set; }
        public TipoUsuario TipoUsuario { get; set; }
        public bool Situacao { get; set; }
        public string SituacaoDesc { get; set; }
        public string Senha { get; set; }
        public int? ClienteSelecionado { get; set; }
    }
}
