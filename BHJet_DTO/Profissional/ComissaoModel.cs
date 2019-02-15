using System;

namespace BHJet_DTO.Profissional
{
    public class ComissaoModel
    {
        public long idComissaoColaboradorEmpresaSistema { get; set; }
        public long idColaboradorEmpresaSistema { get; set; }
        public decimal decPercentualComissao { get; set; }
        public DateTime dtDataInicioVigencia { get; set; }
        public DateTime dtDataFimVigencia { get; set; }
        public bool bitAtivo { get; set; }
        public string vcObservacoes { get; set; }
    }
}
