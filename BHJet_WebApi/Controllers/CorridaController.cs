using BHJet_Core.Variaveis;
using BHJet_CoreGlobal;
using BHJet_DTO.Corrida;
using BHJet_Enumeradores;
using BHJet_Repositorio.Admin;
using BHJet_Repositorio.Admin.Entidade;
using BHJet_Repositorio.Admin.Filtro;
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
            return entidade.DescricaoAtividade;
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
        [Route("status")]
        [ResponseType(typeof(IEnumerable<StatusModel>))]
        public IHttpActionResult GetStatusCorrida()
        {
            // Busca Dados detalhados da corrida/OS
            var entidade = new CorridaRepositorio().BuscaStatusCorrida();

            // Validacao
            if (entidade == null || !entidade.Any())
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(entidade.Select(oc => new StatusModel()
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
        [Route("status/{idStatus:int}/log/{logCorrida:long}/corrida/{idCorrida:long}")]
        public IHttpActionResult PutTipoStatusCorrida(int idStatus, long logCorrida, long idCorrida)
        {
            // Busca Dados detalhados da corrida/OS
            new CorridaRepositorio().AtualizaStatusCorrida(idStatus, logCorrida, idCorrida);

            // Return
            return Ok();
        }

        /// <summary>
        /// Encerrar OS
        /// </summary>
        /// /// <param name="idCorrida">long</param>
        /// /// /// <param name="idStatus">int?</param>
        /// /// /// /// <param name="filtro">EncerrarCorridaFiltro</param>
        /// <returns></returns>
        [Authorize]
        [Route("encerrar/{idCorrida:long}/status/{idStatus:int?}")]
        public IHttpActionResult PutEncerrarOS(long idCorrida, [FromBody]EncerrarCorridaFiltro filtro, int? idStatus = null)
        {
            // Instancia
            new CorridaRepositorio().EncerrarOrdemServico(idCorrida, idStatus,
                (filtro.KilometragemRodada != null ? int.Parse(filtro.KilometragemRodada?.ToString()) : 0), filtro.MinutosParados ?? 0);

            // Return
            return Ok();
        }

        /// <summary>
        /// Busca ocorrencia corrida
        /// </summary>
        /// <returns>List<DetalheCorridaModel></returns>
        [Authorize]
        [Route("ocorrencias")]
        [ResponseType(typeof(IEnumerable<OcorrenciaDTO>))]
        public IHttpActionResult GetOcorrenciaCorrida()
        {
            // Busca Dados detalhados da corrida/OS
            var entidade = new CorridaRepositorio().BuscaOcorrenciaCorrida();

            // Validacao
            if (entidade == null || !entidade.Any())
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(entidade.Select(oc => new OcorrenciaDTO()
            {
                ID = oc.idTipoOcorrenciaCorrida,
                Descricao = oc.vcTipoOcorrenciaCorrida
            }));
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
            return entidade.DescricaoAtividade;
        }

        /// <summary>
        /// Busca preco corrida
        /// </summary>
        /// <returns>Doublle</returns>
        [Route("preco")]
        [ResponseType(typeof(double))]
        public IHttpActionResult PostPrecoCorrido([FromBody]CalculoCorridaDTO model)
        {
            double? valorTotal = CalculaPrecoCoorrida(model);

            // Return
            return Ok(valorTotal);
        }

        private static double? CalculaPrecoCoorrida(CalculoCorridaDTO model)
        {
            // Busca tarifa cliente
            var tarifa = new TarifaRepositorio().BuscaTarificaPorCliente(model.IDCliente, model.TipoVeiculo);

            // Variaveis Preco
            var valorKMAdc = Double.Parse(tarifa.ValorKMAdicional?.ToString() ?? "0");
            var valaorPadrao = Double.Parse(tarifa.ValorContrato?.ToString() ?? "0");

            // Calculo
            var quantidadeKM = DistanciaUtil.CalculaDistancia(model.Localizacao.Select(l => new Localidade()
            {
                Latitude = l.Latitude,
                Longitude = l.Longitude
            }).ToArray());
            var valorKM = (quantidadeKM * tarifa.FranquiaKM) ?? 0;
            var qtdKMExcecdente = quantidadeKM > tarifa.FranquiaKM ? (quantidadeKM - tarifa.FranquiaKM) * valorKMAdc : 0;

            // Valor total
            var valorTotal = valaorPadrao + valorKM + qtdKMExcecdente;
            return valorTotal;
        }

        /// <summary>
        /// Incluir Corrida
        /// </summary>
        /// <returns>List<DetalheCorridaModel></returns>
        [Route("")]
        [ResponseType(typeof(double))]
        public IHttpActionResult PostCorrido([FromBody]IncluirCorridaDTO model)
        {
            // Busca Comissao
            var comissao = new ProfissionalRepositorio().BuscaComissaoProfissional(54);          

             // Calculo Valor Estimado
             var valorEstimado = CalculaPrecoCoorrida(new CalculoCorridaDTO()
            {
                IDCliente = model.IDCliente ?? 0,
                TipoVeiculo = model.TipoProfissional ?? 0,
                Localizacao = model.Enderecos.Select(c => new CalculoCorridaLocalidadeDTO()
                {
                    Latitude = Double.Parse(c.Latitude),
                    Longitude = Double.Parse(c.Longitude)
                }).ToArray()
            });

#if DEBUG
            var usuario = 3;
#else
            var usuario = long.Parse(_usuarioAutenticado.LoginID);
#endif

            // Busca tarifa cliente
            var idCorrida = new CorridaRepositorio().IncluirCorrida(new BHJet_Repositorio.Admin.Filtro.CorridaFiltro()
            {
                IDCliente = model.IDCliente,
                Comissao = comissao.decPercentualComissao,
                TipoProfissional = model.TipoProfissional,
                ValorEstimado = valorEstimado,
                Enderecos = model.Enderecos.Select(c => new EnderecoModel()
                {
                    Descricao = c.Descricao,
                    Latitude = c.Latitude,
                    Longitude = c.Longitude,
                    Observacao = c.Observacao,
                    ProcurarPessoa = c.ProcurarPessoa,
                    TipoOcorrencia = c.TipoOcorrencia
                }).ToList()
            }, usuario);

            // Return
            return Ok(idCorrida);
        }
    }
}


