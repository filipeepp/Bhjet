using BHJet_Core.Enum;
using BHJet_Core.Extension;
using BHJet_Core.Variaveis;
using BHJet_DTO.Profissional;
using BHJet_Repositorio.Admin;
using BHJet_Repositorio.Admin.Entidade;
using BHJet_WebApi.Util;
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
        private UsuarioLogado _usuarioAutenticado;

        /// <summary>
        /// Informações do usuário autenticado
        /// </summary>
        public UsuarioLogado UsuarioAutenticado
        {
            get
            {
                if (_usuarioAutenticado == null)
                    _usuarioAutenticado = new UsuarioLogado();

                return _usuarioAutenticado;
            }
        }

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
                geoPosicao = $"{pf.vcLatitude};{pf.vcLongitude}",
                TipoColaborador = tipo,
                NomeColaborador = pf.vcNomeCompleto
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
                geoPosicao = $"{entidade.vcLatitude};{entidade.vcLongitude}",
                NomeColaborador = entidade.vcNomeCompleto,
                TipoColaborador = (TipoProfissional)entidade.idTipoProfissional
            });
        }

        /// <summary>
        /// Busca lista de Profissionais
        /// </summary>
        /// <returns>List<LocalizacaoProfissional></returns>
        [Authorize]
        [Route("")]
        [ResponseType(typeof(IEnumerable<ProfissionalModel>))]
        public IHttpActionResult GetListaProfissionais([FromUri]string trecho = "")
        {
            // Busca Dados resumidos
            var entidade = new ProfissionalRepositorio().BuscaProfissionais(trecho);

            // Validacao
            if (entidade == null)
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(entidade.Select(pro => new ProfissionalModel()
            {
                ID = pro.ID,
                NomeCompleto = pro.NomeCompleto,
                TipoRegime = pro.TipoRegime,
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
                ID = entidade.ID,
                NomeCompleto = entidade.NomeCompleto,
                CelularWpp = entidade.CelularWpp,
                CNH = entidade.CNH,
                ContratoCLT = entidade.ContratoCLT,
                CPF = entidade.CPF,
                Email = entidade.Email,
                Rua = entidade.Rua,
                Bairro = entidade.Bairro,
                Cidade = entidade.Cidade,
                Complemento = entidade.Complemento,
                PontoReferencia = entidade.PontoReferencia,
                UF = entidade.UF,
                Cep = entidade.Cep,
                EnderecoPrincipal = entidade.EnderecoPrincipal,
                RuaNumero = entidade.RuaNumero,
                Observacao = entidade.Observacao,
                TelefoneCelular = entidade.TelefoneCelular,
                TelefoneResidencial = entidade.TelefoneResidencial,
                TipoCNH = entidade.TipoCNH,
                TipoRegime = entidade.TipoRegime,
                TipoProfissional = entidade.TipoProfissional,
                Comissoes = entidade.Comissoes.Select(c => new ProfissionalComissaoModel()
                {
                    ID = c.idComissaoColaboradorEmpresaSistema,
                    decPercentualComissao = c.decPercentualComissao,
                    dtDataInicioVigencia = c.dtDataInicioVigencia,
                    dtDataFimVigencia = c.dtDataFimVigencia,
                    Observacao = c.vcObservacoes
                }).ToArray()
            });
        }

        /// <summary>
        /// Atualiza profissionais
        /// </summary>
        /// <returns>List<LocalizacaoProfissional></returns>
        [Authorize]
        [Route("{idProfissional:long}")]
        public IHttpActionResult PutProfissional(long idProfissional, [FromBody]ProfissionalCompletoModel model)
        {
            // Busca Dados resumidos
            new ProfissionalRepositorio().AtualizaProfissional(new BHJet_Repositorio.Admin.Entidade.ProfissionalCompletoEntidade()
            {
                ID = idProfissional,
                NomeCompleto = model.NomeCompleto,
                CPF = model.CPF,
                Email = model.Email,
                Observacao = model.Observacao,
                TipoCNH = model.TipoCNH,
                CNH = model.CNH,
                TipoProfissional = model.TipoProfissional,
                TipoRegime = model.TipoRegime,
                ContratoCLT = model.ContratoCLT,
                TelefoneCelular = model.TelefoneCelular,
                TelefoneResidencial = model.TelefoneResidencial,
                CelularWpp = model.CelularWpp,
                Cep = model.Cep,
                Rua = model.Rua,
                Bairro = model.Bairro,
                Cidade = model.Cidade,
                UF = model.UF,
                RuaNumero = model.RuaNumero,
                Complemento = model.Complemento,
                EnderecoPrincipal = model.EnderecoPrincipal,
                PontoReferencia = model.PontoReferencia,
                Comissoes = model.Comissoes != null ? model.Comissoes.Select(c => new ProfissionalComissaoEntidade()
                {
                    idComissaoColaboradorEmpresaSistema = c.ID,
                    decPercentualComissao = c.decPercentualComissao,
                    dtDataInicioVigencia = c.dtDataInicioVigencia,
                    dtDataFimVigencia = c.dtDataFimVigencia,
                    vcObservacoes = c.Observacao
                }).ToArray() : new ProfissionalComissaoEntidade[] { }
            });

            // Return
            return Ok();
        }

        /// <summary>
        /// Busca lista de Profissionais
        /// </summary>
        /// <returns>List<LocalizacaoProfissional></returns>
        [Authorize]
        [Route("")]
        public IHttpActionResult PostProfissional([FromBody]ProfissionalCompletoModel model)
        {
            // Busca Dados resumidos
            var profRepositorio = new ProfissionalRepositorio();

            // Verifica existencia
            var entidade = profRepositorio.VerificaProfissionalExistente(model.Email, model.CPF);

            // VALIDACAO
            if (entidade.Any())
            {
                bool existeCPF = entidade.Where(x => x.vcCPFCNPJ == model.CPF).Any();
                bool existeEmail = entidade.Where(x => x.vcEmail == model.Email).Any();

                string msg = "";
                if (existeCPF && existeEmail)
                    msg = "CPF e Email.";
                else if (existeCPF)
                    msg = "mesmo CPF.";
                else if (existeEmail)
                    msg = "mesmo Email.";

                return BadRequest($"Existe um cadastro com este {msg}. Favor atualizar os dados corretamente");
            }

            // Inclui profissional
            profRepositorio.IncluirProfissional(new BHJet_Repositorio.Admin.Entidade.ProfissionalCompletoEntidade()
            {
                IDGestor = UsuarioAutenticado.LoginID.ToLong(),
                NomeCompleto = model.NomeCompleto,
                CPF = model.CPF,
                Email = model.Email,
                Observacao = model.Observacao,
                TipoCNH = model.TipoCNH,
                CNH = model.CNH,
                TipoProfissional = model.TipoProfissional,
                TipoRegime = model.TipoRegime,
                ContratoCLT = model.ContratoCLT,
                TelefoneCelular = model.TelefoneCelular,
                TelefoneResidencial = model.TelefoneResidencial,
                CelularWpp = model.CelularWpp,
                Cep = model.Cep,
                Rua = model.Rua,
                Bairro = model.Bairro,
                Cidade = model.Cidade,
                UF = model.UF,
                RuaNumero = model.RuaNumero,
                Complemento = model.Complemento,
                EnderecoPrincipal = model.EnderecoPrincipal,
                PontoReferencia = model.PontoReferencia,
                Comissoes = model.Comissoes.Any() ? model.Comissoes.Select(x => new ProfissionalComissaoEntidade()
                {
                    decPercentualComissao = x.decPercentualComissao,
                    dtDataFimVigencia = x.dtDataFimVigencia,
                    dtDataInicioVigencia = x.dtDataInicioVigencia
                }).ToArray() : new ProfissionalComissaoEntidade[] { }
            });

            // Return
            return Ok();
        }

        /// <summary>
        /// Busca lista de Profissionais
        /// </summary>
        /// <returns>List<LocalizacaoProfissional</returns>
        [Authorize]
        [Route("{idProfissional:long}/comissao")]
        [ResponseType(typeof(ComissaoModel))]
        public IHttpActionResult GetComissaoProfissional(long idProfissional)
        {
            // Busca Dados resumidos
            var comissao = new ProfissionalRepositorio().BuscaComissaoProfissional(idProfissional);

            // Validacao
            if (comissao == null)
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(new ComissaoModel()
            {
                idComissaoColaboradorEmpresaSistema = comissao.idComissaoColaboradorEmpresaSistema,
                idColaboradorEmpresaSistema = comissao.idColaboradorEmpresaSistema,
                bitAtivo = comissao.bitAtivo,
                decPercentualComissao = comissao.decPercentualComissao,
                dtDataInicioVigencia = comissao.dtDataInicioVigencia,
                dtDataFimVigencia = comissao.dtDataFimVigencia,
                vcObservacoes = comissao.vcObservacoes
            });
        }

    }
}