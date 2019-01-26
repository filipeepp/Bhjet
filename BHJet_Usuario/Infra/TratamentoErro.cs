using BHJet_Core.Extension;
using BHJet_Core.Utilitario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BHJet_Usuario.Infra
{
	public static class TratamentoErro
	{
		public static void TrataErro(this Controller controle, Exception e)
		{
			if (e.GetType() == typeof(SucessException))
			{
				controle.TempData["mensagemGeral"] = Encoding.UTF8.GetBytes(e.Message);
				controle.TempData["imgMensagemGeral"] = @"\\.\\Images\\sucesso.png".ToAscii();
			}
			else
			{
				controle.TempData["mensagemGeral"] = e.Message;
				controle.TempData["imgMensagemGeral"] = @"\\.\\Images\\warming.png".ToAscii();
			}

			return;
		}

		public static void MensagemSucesso(this Controller controle, string mensagemSucesso)
		{
			controle.TempData["mensagemGeral"] = mensagemSucesso;
			controle.TempData["imgMensagemGeral"] = @"\\.\\Images\\sucesso.png".ToAscii();
			return;

		}

		public static void MensagemAlerta(this Controller controle, string mensagemAlerta)
		{
			controle.TempData["mensagemGeral"] = mensagemAlerta;
			controle.TempData["imgMensagemGeral"] = @"\\.\\Images\\warming.png".ToAscii();
			return;
		}
	}
}