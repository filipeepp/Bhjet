using BHJet_Core.Enum;

namespace BHJet_Repositorio.Filtro
{
    public class ValidaUsuarioFiltro
    {
        public string usuarioEmail { get; set; }
        public string usuarioSenha { get; set; }
        public TipoUsuario usuarioTipo { get; set; }
    }
}
