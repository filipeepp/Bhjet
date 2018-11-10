using BHJet_Core.Enum;

namespace BHJet_Repositorio.Entidade
{
    public class UsuarioEntidade
    {
        long idUsuario { get; set; }
        TipoUsuario idTipoUsuario { get; set; }
        string vcEmail { get; set; }
    }
}
