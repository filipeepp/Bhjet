namespace BHJet_DTO.Corrida
{
    public class StatusModel
    {
        public int StatusCorrida { get; set; }
        public string DescricaoStatus { get; set; }
        public bool Inicia { get; set; }
        public bool Finaliza { get; set; }
        public bool Cancela { get; set; }
    }
}
