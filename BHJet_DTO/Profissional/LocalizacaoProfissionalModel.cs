using BHJet_Enumeradores;

namespace BHJet_DTO.Profissional
{
    public class LocalizacaoProfissionalModel
    {
        public int idColaboradorEmpresaSistema { get; set; }
        public string NomeColaborador { get; set; }
        public TipoProfissional TipoColaborador { get; set; }
        public string geoPosicao { get; set; }
    }
}
