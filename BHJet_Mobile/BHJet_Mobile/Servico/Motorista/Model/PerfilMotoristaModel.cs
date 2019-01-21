using BHJet_Enumeradores;

namespace BHJet_Mobile.Servico.Motorista.Model
{
    public class PerfilMotoristaModel
    {
        public long idUsuario { get; set; }
        public long idColaboradorEmpresaSistema { get; set; }
        public long? idRegistroDiaria { get; set; }
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public TipoProfissional TipoProfissional { get; set; }
    }
}
