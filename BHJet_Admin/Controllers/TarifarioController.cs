using BHJet_Admin.Infra;
using BHJet_Admin.Models.Tarifario;
using BHJet_Core.Extension;
using BHJet_DTO.Tarifa;
using BHJet_Servico.Tarifa;
using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace BHJet_Admin.Controllers
{
    public class TarifarioController : Controller
    {
        private readonly ITarifaServico tarifaServico;

        public TarifarioController(ITarifaServico _tarifa)
        {
            tarifaServico = _tarifa;
        }

        // GET: Tarifario
        public ActionResult Index()
        {
            var TarifaModel = new TarifarioModel();

            try
            {
                // Busca tarifas
                TarifaModel = BuscaTarifaPadrao();

                // Model
                return View(TarifaModel);
            }
            catch (Exception e)
            {
                this.TrataErro(e);
                return View(TarifaModel);
            }
        }

        [HttpPost]
        [ValidacaoUsuarioAttribute()]
        public ActionResult Index(TarifarioModel model)
        {
            var TarifaModel = new TarifarioModel();

            try
            {
                // Atualiza tarifas
                tarifaServico.AtualizaTarifaPadrao(model.Tarifas.Select(tf => new TarifarioDTO()
                {
                    idTarifario = tf.idTarifario,
                    Ativo = tf.Ativo,
                    idTipoServico = tf.idTipoServico,
                    idTipoVeiculo = tf.idTipoVeiculo,
                    DataFimVigencia = tf.DataFimVigencia,
                    DataInicioVigencia = tf.DataInicioVigencia,
                    DescricaoTarifario = tf.DescricaoTarifario,
                    FranquiaHoras = tf.FranquiaHoras,
                    FranquiaKM = tf.FranquiaKM,
                    FranquiaMinutosParados = tf.FranquiaMinutosParados,
                    Observacao = tf.Observacao,
                    ValorContrato = tf.ValorContrato?.ToDecimalCurrency(),
                    ValorHoraAdicional = tf.ValorHoraAdicional?.ToDecimalCurrency(),
                    ValorKMAdicional = tf.ValorKMAdicional?.ToDecimalCurrency(),
                    ValorMinutoParado = tf.ValorMinutoParado?.ToDecimalCurrency()
                }).ToArray());

                // Busca tarifas
                TarifaModel = BuscaTarifaPadrao();

                // Sucesso.
                this.MensagemSucesso("Tarifas atualizadas com sucesso.");

                // Return
                return View(TarifaModel);
            }
            catch (Exception e)
            {
                this.TrataErro(e);
                return View(TarifaModel);
            }
        }


        private TarifarioModel BuscaTarifaPadrao()
        {
            // Busca tarifas
            var tarifas = tarifaServico.BuscaTarifaPadrao();

            // Model
            return new TarifarioModel
            {
                Tarifas = tarifas.Select(tf => new TarifaModel()
                {
                    idTarifario = tf.idTarifario,
                    Ativo = tf.Ativo,
                    idTipoServico = tf.idTipoServico,
                    idTipoVeiculo = tf.idTipoVeiculo,
                    DataFimVigencia = tf.DataFimVigencia,
                    DataInicioVigencia = tf.DataInicioVigencia,
                    DescricaoTarifario = tf.DescricaoTarifario,
                    FranquiaHoras = tf.FranquiaHoras,
                    FranquiaKM = tf.FranquiaKM,
                    FranquiaMinutosParados = tf.FranquiaMinutosParados,
                    Observacao = tf.Observacao,
                    ValorContrato = tf.ValorContrato?.ToString("C", new CultureInfo("pt-BR")),
                    ValorHoraAdicional = tf.ValorHoraAdicional?.ToString("C", new CultureInfo("pt-BR")),
                    ValorKMAdicional = tf.ValorKMAdicional?.ToString("C", new CultureInfo("pt-BR")),
                    ValorMinutoParado = tf.ValorMinutoParado?.ToString("C", new CultureInfo("pt-BR")),
                    ValorPontoExcedente = tf.ValorPontoExcedente?.ToString("C", new CultureInfo("pt-BR"))
                }).ToArray()
            };
        }


        [HttpGet]
        [ValidacaoUsuarioAttribute()]
        public JsonResult BuscarTarifarioPadraoAtivo(int codigoTipoVeiculo)
        {
            var data = tarifaServico.BuscaTarifaAtiva(codigoTipoVeiculo);

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);

            return Json(json, JsonRequestBehavior.AllowGet);

        }
    }
}