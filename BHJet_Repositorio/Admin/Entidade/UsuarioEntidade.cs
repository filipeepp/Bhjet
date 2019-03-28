using BHJet_Enumeradores;

namespace BHJet_Repositorio.Entidade
{
    public class UsuarioEntidade
    {
        public long idUsuario { get; set; }
        public TipoUsuario idTipoUsuario { get; set; }
        public string vcEmail { get; set; }
        public bool bitAtivo { get; set; }
        public string bitDescAtivo { get; set; }
        public string vbIncPassword { get; set; }
        public long? ClienteSelecionado { get; set; }
        public long? ColaboradorSelecionado { get; set; }
    }
}
