using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHJet_Core.Enum
{
	public enum TipoTarifa
	{
		[Description("Avulso Mensal")]
		AvulsoMensal = 1,

		[Description("Alocação Mensal")]
		AlocacaoMensal = 2

	}
}
