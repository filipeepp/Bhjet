using BHJet_Admin.Models.Faturamento;
using System;
using System.Web.Mvc;

namespace BHJet_Admin.Controllers
{
    public class FaturamentoController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        #region Gerar Faturamento
        public ActionResult GerarFaturamento()
        {
            return View(new GerarFaturamantoModel()
            {
                ListaClientes = new System.Collections.Generic.Dictionary<int, string>()
                {

                }
            });
        }

        [HttpPost]
        public ActionResult GerarFaturamento(GerarFaturamantoModel model)
        {
            return View(new GerarFaturamantoModel()
            {
                ListaClientes = new System.Collections.Generic.Dictionary<int, string>()
                {

                },
                ClienteSelecionado = 1,
                ListaFaturamento = new System.Collections.Generic.List<FaturamentoModel>()
                 {
                    new FaturamentoModel()
                     {
                      Cliente = "Cliente A",
                      Apuração = DateTime.Now,
                       DescContrato = "Avulso",
                       Valor = 1541m
                    }
                 }
            });
        }
        #endregion


        #region Faturamento Avulso
        public ActionResult FaturamentoNormal()
        {

            var cli = new System.Collections.Generic.Dictionary<int, string>();
            cli.Add(1, "Mercado Teste");
            cli.Add(2, "Loja Teste");

            return View(new FaturamentoNormal()
            {
                ListaClientes = cli,
                ListaTipoContrato = new System.Collections.Generic.Dictionary<int, string>()
                {

                }
            });
        }

        [HttpPost]
        public ActionResult FaturamentoNormal(FaturamentoAvulsoModel model)
        {
            return View();
        }
        #endregion

        #region Faturamento Avulso
        public ActionResult FaturamentoAvulso()
        {

            var cli = new System.Collections.Generic.Dictionary<int, string>();
            cli.Add(1, "Mercado Teste");
            cli.Add(2, "Loja Teste");

            return View(new FaturamentoAvulsoModel()
            {
                ListaClientes = cli,
                ListaTipoContrato = new System.Collections.Generic.Dictionary<int, string>()
                {

                }
            });
        }

        [HttpPost]
        public ActionResult FaturamentoAvulso(FaturamentoAvulsoModel model)
        {
            return View();
        }
        #endregion
    }
}