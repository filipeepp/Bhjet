using BHJet_Core.Extension;
using BHJet_Core.Utilitario;
using System;
using System.Web.Mvc;

namespace BHJet_Admin.Controllers
{
    public static class TratamentoErro
    {
        public static void TrataErro(this Controller controle, Exception e)
        {
            if (e.GetType() == typeof(SucessException))
            {
                controle.TempData["mensagemGeral"] = e.Message.ToAscii();
                controle.TempData["imgMensagemGeral"] = @"\\.\\Images\\sucesso.png".ToAscii();
            }
            else
            {
                controle.TempData["mensagemGeral"] = e.Message.ToAscii();
                controle.TempData["imgMensagemGeral"] = @"\\.\\Images\\warming.png".ToAscii();
            }

            return;
        }

        public static void MensagemSucesso(this Controller controle, string mensagemSucesso)
        {
            controle.TempData["mensagemGeral"] = mensagemSucesso.ToAscii();
            controle.TempData["imgMensagemGeral"] = @"\\.\\Images\\sucesso.png".ToAscii();
            return;
        }

        public static void MensagemAlerta(this Controller controle, string mensagemAlerta)
        {
            controle.TempData["mensagemGeral"] = mensagemAlerta.ToAscii();
            controle.TempData["imgMensagemGeral"] = @"\\.\\Images\\warming.png".ToAscii();
            return;
        }
    }
}