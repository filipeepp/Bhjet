namespace BHJet_Repositorio.Admin.Entidade
{
    public class StatusEntidade
    {
        public int idStatusCorrida { get; set; }
        public string vcDescricaoStatus { get; set; }
        public bool bitInicia { get; set; }
        public bool bitFinaliza { get; set; }
        public bool bitCancela { get; set; }
    }
}
