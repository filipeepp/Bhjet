namespace BHJet_Repositorio.Admin.Entidade
{
    public class OcorrenciaEntidade
    {
        public int idStatusCorrida { get; set; }
        public string vcDescricaoStatus { get; set; }
        public bool bitInicia { get; set; }
        public bool bitFinaliza { get; set; }
        public bool bitCancela { get; set; }
    }
}
