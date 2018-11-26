using BHJet_Core.Enum;

namespace BHJet_Repositorio.Entidade
{
    public class UsuarioEntidade
    {
        public long idUsuario { get; set; }
        public TipoUsuario idTipoUsuario { get; set; }
        public string vcEmail { get; set; }
    }
}
