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
		[Display(Name ="Avulso Mensal")]
		AvulsoMensal = 1,

		[Display(Name ="Alocação Mensal")]
		AlocacaoMensal = 2

	}
}
