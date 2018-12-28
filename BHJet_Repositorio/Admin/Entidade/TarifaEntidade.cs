using System;

namespace BHJet_Repositorio.Admin.Entidade
{
    public class TarifaEntidade
    {
        public int ID { get; set; }
        public string Descricao { get; set; }
        public decimal ValorDiaria { get; set; }
		public DateTime? VigenciaInicio { get; set; }
		public DateTime? VigenciaFim { get; set; }
		public decimal? FranquiaKMDiaria { get; set; }
		public decimal? ValorKMAdicionalDiaria { get; set; }
		public decimal? FranquiaKMMensalidade { get; set; }
		public decimal? ValorKMAdicionalMensalidade { get; set; }
		public int? Ativo { get; set; }
	}
}
