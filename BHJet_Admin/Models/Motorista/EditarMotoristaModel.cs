using BHJet_Core.Enum;

namespace BHJet_Admin.Models.Motorista
{
    public class EditarMotoristaModel
    {
        public MotoristaSimplesModel[] ListaMotorista { get; set; }

        public NovoMotoristaModel MotoristaSelecionado { get; set; }
    }

    public class MotoristaSimplesModel
    {
        public long ID { get; set; }

        public string NomeCompleto { get; set; }

        public RegimeContratacao TipoRegimeContratacao { get; set; }
    }
}