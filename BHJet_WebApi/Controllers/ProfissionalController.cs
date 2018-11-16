using BHJet_Core.Enum;
using BHJet_Core.Variaveis;
using BHJet_DTO.Profissional;
using BHJet_Repositorio.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace BHJet_WebApi.Controllers
{
    [RoutePrefix("api/Profissional")]
    public class ProfissionalController : ApiController
    {
        /// <summary>
        /// Busca localização de profissionais disponíveis
        /// </summary>
        /// <returns>List<LocalizacaoProfissional></returns>
        [Authorize]
        [Route("tipo/{tipoProfissional:int}/localizacao")]
        [ResponseType(typeof(IEnumerable<LocalizacaoProfissionalModel>))]
        public IHttpActionResult GetLocalizacaoMotoristasDisponiveis(int tipoProfissional)
        {
            // Tipo profissional desejado
            TipoProfissional tipo;
            if (!Enum.TryParse(tipoProfissional.ToString(), out tipo))
                BadRequest(Mensagem.Validacao.TipoProfissionalInvalido);

            // Busca Dados resumidos
            var entidade = new ProfissionalRepositorio().BuscaLocalizacaoProfissionaisDisponiveis(tipo);

            // Validacao
            if (entidade == null || !entidade.Any())
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(entidade.Select(pf => new LocalizacaoProfissionalModel()
            {
                idColaboradorEmpresaSistema = pf.idColaboradorEmpresaSistema,
                geoPosicao = $"{pf.vcLatitude};{pf.vcLongitude}"
            }));
        }

        /// <summary>
        /// Busca localização de profissionais disponíveis
        /// </summary>
        /// <returns>List<LocalizacaoProfissional></returns>
        [Authorize]
        [Route("{idProfissional:long}/localizacao")]
        [ResponseType(typeof(LocalizacaoProfissionalModel))]
        public IHttpActionResult GetLocalizacaoMotoristaDisponiveil(long idProfissional)
        {
            // Busca Dados resumidos
            var entidade = new ProfissionalRepositorio().BuscaLocalizacaoProfissionalDisponiveil(idProfissional);

            // Validacao
            if (entidade == null)
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(new LocalizacaoProfissionalModel()
            {
                idColaboradorEmpresaSistema = entidade.idColaboradorEmpresaSistema,
                geoPosicao = $"{entidade.vcLatitude};{entidade.vcLongitude}"
            });
        }

        /// <summary>
        /// Busca lista de Profissionais
        /// </summary>
        /// <returns>List<LocalizacaoProfissional></returns>
        [Authorize]
        [Route("")]
        [ResponseType(typeof(IEnumerable<ProfissionalModel>))]
        public IHttpActionResult GetListaProfissionais()
        {
            // Busca Dados resumidos
            var entidade = new ProfissionalRepositorio().BuscaProfissionais();

            // Validacao
            if (entidade == null)
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(entidade.Select(pro => new ProfissionalModel()
            {
                ID = pro.ID,
                NomeCompleto = pro.NomeCompleto,
                TipoRegime = pro.TipoContrato,
                TipoProfissional = pro.TipoProfissional
            }));
        }

        /// <summary>
        /// Busca lista de Profissionais
        /// </summary>
        /// <returns>List<LocalizacaoProfissional></returns>
        [Authorize]
        [Route("{idProfissional:long}")]
        [ResponseType(typeof(ProfissionalCompletoModel))]
        public IHttpActionResult GetProfissional(long idProfissional)
        {
            // Busca Dados resumidos
            var entidade = new ProfissionalRepositorio().BuscaProfissional(idProfissional);

            // Validacao
            if (entidade == null)
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(new ProfissionalCompletoModel()
            {
                CelularWpp = entidade.CelularWpp,
                CNH = entidade.CNH,
                ContratoCLT = entidade.ContratoCLT,
                CPF = entidade.CPF,
                Email = entidade.Email,
                EnderecoCompleto = entidade.EnderecoCompleto,
                ID = entidade.ID,
                NomeCompleto = entidade.NomeCompleto,
                Observacao = entidade.Observacao,
                TelefoneCelular = entidade.TelefoneCelular,
                TelefoneResidencial = entidade.TelefoneResidencial,
                TipoCNH = entidade.TipoCNH,
                TipoRegime = entidade.TipoContrato,
                TipoProfissional = entidade.TipoProfissional
            });
        }

        /// <summary>
        /// Busca lista de Profissionais
        /// </summary>
        /// <returns>List<LocalizacaoProfissional></returns>
        [Authorize]
        [Route("{idProfissional:long}")]
        [ResponseType(typeof(ProfissionalCompletoModel))]
        public IHttpActionResult PutProfissional([FromBody]ProfissionalCompletoModel model)
        {
            // Busca Dados resumidos
            var entidade = new ProfissionalRepositorio().BuscaProfissional(idProfissional);

            // Validacao
            if (entidade == null)
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(new ProfissionalCompletoModel()
            {
                CelularWpp = entidade.CelularWpp,
                CNH = entidade.CNH,
                ContratoCLT = entidade.ContratoCLT,
                CPF = entidade.CPF,
                Email = entidade.Email,
                EnderecoCompleto = entidade.EnderecoCompleto,
                ID = entidade.ID,
                NomeCompleto = entidade.NomeCompleto,
                Observacao = entidade.Observacao,
                TelefoneCelular = entidade.TelefoneCelular,
                TelefoneResidencial = entidade.TelefoneResidencial,
                TipoCNH = entidade.TipoCNH,
                TipoRegime = entidade.TipoContrato,
                TipoProfissional = entidade.TipoProfissional
            });
        }
    }
}