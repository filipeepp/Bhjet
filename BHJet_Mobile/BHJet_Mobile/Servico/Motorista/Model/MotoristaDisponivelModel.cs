using BHJet_Enumeradores;

namespace BHJet_Mobile.Servico.Motorista.Model
{
    public class MotoristaDisponivelModel
    {
        public bool bitDisponivel { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public TipoProfissional idTipoProfissional { get; set; }
    }
}
