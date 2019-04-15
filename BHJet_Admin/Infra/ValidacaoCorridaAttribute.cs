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
            //base.OnAuthorization(filterContext);
            VerificaPassoDaSoliciracaoCorrida(filterContext);
        }

        private void VerificaPassoDaSoliciracaoCorrida(AuthorizationContext filterContext)
        {
            // Variaveis
            var corrida = filterContext.Controller.TempData[OSAvulsaControle.NomeVD] != null ? (EntregaModel)filterContext.Controller.TempData[OSAvulsaControle.NomeVD] : null;

            filterContext.Controller.TempData[OSAvulsaControle.NomeVD] = corrida;

            // Validacao Usuario
            if (corrida != null)
            {
                switch (Passo)
                {
                    case OSAvulsoPassos.Origem:
                        break;
                    case OSAvulsoPassos.Destinos:
                        if(corrida.PassoOS != OSAvulsoPassos.Origem && corrida.PassoOS != OSAvulsoPassos.Conclusao)
                            filterContext.Result = new RedirectResult("~/HomeExterno/Index");
                        break;
                    case OSAvulsoPassos.Conclusao:
                        if (corrida.PassoOS != OSAvulsoPassos.Destinos)
                            filterContext.Result = new RedirectResult("~/HomeExterno/Index");
                        break;
                    case OSAvulsoPassos.Pagamento:
                        if (corrida.PassoOS != OSAvulsoPassos.Destinos && corrida.PassoOS != OSAvulsoPassos.Conclusao)
                            filterContext.Result = new RedirectResult("~/HomeExterno/Index");
                        break;
                }
            }
        }
    }
}