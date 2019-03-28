using BHJet_Admin.Models;
using BHJet_Enumeradores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BHJet_Admin.Infra
{
    public class ValidacaoCorridaAttribute : AuthorizeAttribute
    {
        private OSAvulsoPassos Passo { get; set; }

        public ValidacaoCorridaAttribute(OSAvulsoPassos passo)
        {
            Passo = passo;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            VerificaPassoDaSoliciracaoCorrida(filterContext);
        }

        private void VerificaPassoDaSoliciracaoCorrida(AuthorizationContext filterContext)
        {
            // Variaveis
            var corrida = filterContext.Controller.TempData[OSAvulsaControle.NomeVD] != null ? (EntregaModel)filterContext.Controller.TempData[OSAvulsaControle.NomeVD] : null;

            // Validacao Usuario
            if (corrida != null && (((int)corrida.PassoOS) + 1) != (int)Passo)
                filterContext.Result = new RedirectResult("~/HomeExterno/Index");
        }
    }
}