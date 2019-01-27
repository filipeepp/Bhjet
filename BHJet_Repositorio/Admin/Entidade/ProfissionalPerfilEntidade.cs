using BHJet_Enumeradores;

namespace BHJet_Repositorio.Admin.Entidade
{
    public class ProfissionalPerfilEntidade
    {
        public long idUsuario { get; set; }
        public long idColaboradorEmpresaSistema { get; set; }
        public long? idRegistroDiaria { get; set; }
        public string vcNomeCompleto { get; set; }
        public string vcEmail { get; set; }
        public TipoProfissional idTipoProfissional { get; set; }
    }
}
