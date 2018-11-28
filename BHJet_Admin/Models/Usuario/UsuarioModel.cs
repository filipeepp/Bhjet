using BHJet_Core.Enum;

namespace BHJet_Admin.Models.Usuario
{
    public class UsuariosModel
    {
        public UsuarioModel[] usuarios { get; set; }
    }

    public class UsuarioModel
    {
        public long ID { get; set; }
        public string Email { get; set; }
        public TipoUsuario TipoUser { get; set; }
        public string Situacao { get; set; }
    }


}