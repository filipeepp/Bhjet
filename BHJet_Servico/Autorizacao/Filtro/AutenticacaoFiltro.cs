using BHJet_Enumeradores;

namespace BHJet_Servico.Autorizacao.Filtro
{
    public class AutenticacaoFiltro
    {
        public string usuario { get; set; }
        public string senha { get; set; }
        public TipoAplicacao area { get; set; }
    }
}
