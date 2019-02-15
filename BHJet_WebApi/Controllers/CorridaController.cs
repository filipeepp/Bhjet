using BHJet_Core.Variaveis;
using BHJet_CoreGlobal;
using BHJet_DTO.Corrida;
using BHJet_Enumeradores;
using BHJet_Repositorio.Admin;
using BHJet_Repositorio.Admin.Entidade;
using BHJet_WebApi.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace BHJet_WebApi.Controllers
{
    [RoutePrefix("api/Corrida")]
    public class CorridaController : ApiController
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
        /// Busca localização de chamados de um tipo especifico para um tipo especifico de motorista
        /// </summary>
        /// <returns>List<LocalizacaoProfissional></returns>
        [Authorize]
        [Route("{idOS:long}")]
        [ResponseType(typeof(DetalheCorridaModel))]
        public IHttpActionResult GetCorrida(int idOS)
        {
            // Busca Dados detalhados da corrida/OS
            var entidade = new CorridaRepositorio().BuscaDetalheCorrida(idOS);

            // Validacao
            if (entidade == null || !entidade.Any())
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(new DetalheCorridaModel()
            {
                IDCliente = entidade.FirstOrDefault().IDCliente,
                IDProfissional = entidade.FirstOrDefault().IDProfissional,
                NumeroOS = entidade.FirstOrDefault().NumeroOS,
                Origem = new DetalheOSEnderecoModel()
                {
                    EnderecoCompleto = entidade.FirstOrDefault().EnderecoCompleto,
                    ProcurarPor = entidade.FirstOrDefault().ProcurarPor,
                    Realizar = Realizar(entidade.FirstOrDefault()),
                    StatusCorrida = entidade.FirstOrDefault().StatusCorrida,
                    TempoEspera = entidade.FirstOrDefault().TempoEspera?.TimeOfDay,
                    Observacao = entidade.FirstOrDefault().Observacao,
                    CaminhoProtocolo = entidade.FirstOrDefault().CaminhoProtocolo
                },
                Destinos = entidade.Count() > 1 ? entidade.Skip(1).Select(x => new DetalheOSEnderecoModel()
                {
                    EnderecoCompleto = x.EnderecoCompleto,
                    ProcurarPor = x.ProcurarPor,
                    Realizar = Realizar(x),
                    StatusCorrida = x.StatusCorrida,
                    TempoEspera = x.TempoEspera?.TimeOfDay,
                    Observacao = x.Observacao,
                    CaminhoProtocolo = x.CaminhoProtocolo
                }).ToArray() : new DetalheOSEnderecoModel[] { }
            });
        }

        /// <summary>
        /// Busca Corrida em aberto
        /// </summary>
        /// <returns>CorridaEncontradaEntidade</returns>
        [Authorize]
        [Route("aberta/{idProfissional:long}/{tipoProfissional:int}")]
        [ResponseType(typeof(CorridaEncontradaEntidade))]
        public IHttpActionResult GetCorridaAberta(long idProfissional, int tipoProfissional)
        {
            // Busca Dados detalhados da corrida/OS
            var entidade = new CorridaRepositorio().BuscaCorridaAberta(idProfissional, tipoProfissional);

            // Validacao
            if (entidade == null)
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(entidade);
        }

        private string Realizar(OSCorridaEntidade entidade)
        {
            List<string> realizar = new List<string>();
            if (entidade.bitColetarAssinatura)
                realizar.Add("Coletar Assinatura");
            if (entidade.bitEntregarDocumento)
                realizar.Add("Entregar Documento");
            if (entidade.bitEntregarObjeto)
                realizar.Add("Entregar Objeto");
            if (entidade.bitRetirarDocumento)
                realizar.Add("Retirar Documento");
            if (entidade.bitRetirarObjeto)
                realizar.Add("Retirar Objeto");

            return string.Join(", ", realizar.ToArray());
        }

        /// <summary>
        /// Busca localização de chamados de um tipo especifico para um tipo especifico de motorista
        /// </summary>
        /// <returns>List<LocalizacaoProfissional></returns>
        [Authorize]
        [Route("status/{statusCorrida:int}/profissional/{tipoProfissional:int}/localizacao")]
        [ResponseType(typeof(List<LocalizacaoCorridaModel>))]
        public IHttpActionResult GetLocalizacaoCorridas(int statusCorrida, int tipoProfissional)
        {
            #region Validações
            // Tipo profissional desejado
            TipoProfissional tipo;
            if (!Enum.TryParse(tipoProfissional.ToString(), out tipo))
                BadRequest(Mensagem.Validacao.TipoProfissionalInvalido);
            // Status de corrida
            StatusCorrida status;
            if (!Enum.TryParse(statusCorrida.ToString(), out status))
                BadRequest(Mensagem.Validacao.StatusCorridaInvalido);
            #endregion

            // Busca Dados resumidos
            var entidade = new CorridaRepositorio().BuscaLocalizacaoCorrida(status, tipo);

            // Validacao
            if (entidade == null || !entidade.Any())
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(entidade.Select(pf => new LocalizacaoCorridaModel()
            {
                idCorrida = pf.idCorrida,
                geoPosicao = pf.vcLatitude + ";" + pf.vcLongitude,
            }));
        }

        /// <summary>
        /// Busca detalhes da corrida por cliente
        /// </summary>
        /// <returns>List<DetalheCorridaModel></returns>
        [Authorize]
        [Route("cliente/{clienteID:long}")]
        [ResponseType(typeof(DetalheCorridaModel))]
        public IHttpActionResult GetCorridaCliente(long clienteID)
        {
            // Busca Dados detalhados da corrida/OS
            var entidade = new CorridaRepositorio().BuscaDetalheCorridaCliente(clienteID);

            // Validacao
            if (entidade == null || !entidade.Any())
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(entidade.Select(dtm => new DetalheCorridaModel()
            {
                IDCliente = dtm.IDCliente,
                NumeroOS = dtm.NumeroOS,
                DataInicio = dtm.DataHoraInicio,
                NomeProfissional = dtm.NomeProfissional,
                ValorFinalizado = dtm.ValorFinalizado
            }));
        }

        /// <summary>
		/// Busca LOG corrida
		/// </summary>
		/// <returns>List<DetalheCorridaModel></returns>
		[Authorize]
        [Route("log/{idCorrida:long}")]
        [ResponseType(typeof(IEnumerable<LogCorridaModel>))]
        public IHttpActionResult GetLogCorrida(long idCorrida)
        {
            // Busca Dados detalhados da corrida/OS
            var entidade = new CorridaRepositorio().BuscaLogCorrida(idCorrida);

            // Validacao
            if (entidade == null)
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(entidade.Select(c => new LogCorridaModel()
            {
                idCorrida = c.idCorrida,
                idEnderecoCorrida = c.idEnderecoCorrida,
                IDOcorrencia = c.IDOcorrencia,
                dtHoraChegada = c.dtHoraChegada,
                EnderecoCompleto = c.EnderecoCompleto,
                Observacao = c.vcObservacao,
                Latitude = c.vcLatitude,
                Longitude = c.vcLongitude,
                Status = c.Status,
                TelefoneContato = c.vcTelefoneContato,
                RegistroFoto = c.Foto,
                PessoaContato = c.vcPessoaContato,
                Atividade = MontaAtividade(c)
            }));
        }

        /// <summary>
        /// Cadastra foto de protocolo
        /// </summary>
        /// <param name="idEndereco">long</param>
        /// <returns></returns>
        [Authorize]
        [Route("protocolo/endereco/{idEndereco:long}")]
        public IHttpActionResult PostRegistroProtocolo(long idEndereco)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            // Request
            var httpRequest = HttpContext.Current.Request;

            // Arquivo
            var arquivo = httpRequest.Files[0];

            // COnverte arquivo
            byte[] arquivoBytes = new byte[] { };
            arquivoBytes = arquivo.InputStream.ReadFully();

            // Instancia
            new CorridaRepositorio().InsereRegistroProtocolo(arquivoBytes, idEndereco);

            // Return
            return Ok();
        }

        /// <summary>
        /// Altera CHEGADA em um endereco de uma corrida
        /// </summary>
        /// /// <param name="idEnderecoCorrida">long</param>
        /// <returns></returns>
        [Authorize]
        [Route("chegada/{idEnderecoCorrida:long}")]
        public IHttpActionResult PutRegistrarChegadaEnderecoCorrida(long idEnderecoCorrida)
        {
            // Instancia
            new CorridaRepositorio().RegistraChegadaEndereco(idEnderecoCorrida);

            // Return
            return Ok();
        }

        /// <summary>
        /// Busca ocorrencia corrida
        /// </summary>
        /// <returns>List<DetalheCorridaModel></returns>
        [Authorize]
        [Route("ocorrencias")]
        [ResponseType(typeof(IEnumerable<OcorrenciaModel>))]
        public IHttpActionResult GetTipoOcorrenciaCorrida()
        {
            // Busca Dados detalhados da corrida/OS
            var entidade = new CorridaRepositorio().BuscaOcorrenciasCorrida();

            // Validacao
            if (entidade == null || !entidade.Any())
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(entidade.Select(oc => new OcorrenciaModel()
            {
                StatusCorrida = oc.idStatusCorrida,
                DescricaoStatus = oc.vcDescricaoStatus,
                Cancela = oc.bitCancela,
                Finaliza = oc.bitFinaliza,
                Inicia = oc.bitInicia
            }));
        }

        /// <summary>
        /// Atualiza OCORRENICA corrida
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("ocorrencias/{idOcorrencia:int}/log/{logCorrida:long}/corrida/{idCorrida:long}")]
        public IHttpActionResult PutTipoOcorrenciaCorrida(int idOcorrencia, long logCorrida, long idCorrida)
        {
            // Busca Dados detalhados da corrida/OS
            new CorridaRepositorio().AtualizaOcorrenciasCorrida(idOcorrencia, logCorrida, idCorrida);

            // Return
            return Ok();
        }

        /// <summary>
        /// Encerrar OS
        /// </summary>
        /// /// <param name="idCorrida">long</param>
        /// /// /// <param name="idOcorrencia">int?</param>
        /// /// /// /// <param name="filtro">EncerrarCorridaFiltro</param>
        /// <returns></returns>
        [Authorize]
        [Route("encerrar/{idCorrida:long}/ocorrencia/{idOcorrencia:int?}")]
        public IHttpActionResult PutEncerrarOS(long idCorrida, [FromBody]EncerrarCorridaFiltro filtro, int? idOcorrencia = null)
        {
            // Instancia
            new CorridaRepositorio().EncerrarOrdemServico(idCorrida, idOcorrencia, filtro.KilometragemRodada ?? 0);

            // Return
            return Ok();
        }

        /// <summary>
        /// RECUSAR OS
        /// </summary>
        /// /// <param name="idCorrida">long</param>
        /// <returns></returns>
        [Authorize]
        [Route("recusar/{idCorrida:long}")]
        public IHttpActionResult PostRecusarOS(long idCorrida)
        {
            // Busca profissional
            var perfil = new ProfissionalRepositorio().BuscaPerfilProfissional(UsuarioAutenticado.LoginID);

            // Instancia
            new CorridaRepositorio().RecusarOrdemServico(idCorrida, perfil.idColaboradorEmpresaSistema);

            // Return
            return Ok();
        }

        /// <summary>
        /// LIBERAR OS
        /// </summary>
        /// /// <param name="idCorrida">long</param>
        /// <returns></returns>
        [Authorize]
        [Route("liberar/{idCorrida:long}")]
        public IHttpActionResult PostLiberarOS(long idCorrida)
        {
            // Busca profissional
            var perfil = new ProfissionalRepositorio().BuscaPerfilProfissional(UsuarioAutenticado.LoginID);

            // Instancia
            new CorridaRepositorio().LiberarOrdemServico(idCorrida, perfil.idColaboradorEmpresaSistema);

            // Return
            return Ok();
        }

        private string MontaAtividade(LogCorridaEntidade entidade)
        {
            if (entidade.bitEntregarDocumento)
                return "Entregar Documento";
            else if (entidade.bitColetarAssinatura)
                return "Coletar Assinatura";
            else if (entidade.bitRetirarDocumento)
                return "Retirar Documento";
            else if (entidade.bitRetirarObjeto)
                return "Retirar Objeto";
            else if (entidade.bitEntregarObjeto)
                return "Entregar Objeto";
            else
                return "Outros";
        }
    }
}



