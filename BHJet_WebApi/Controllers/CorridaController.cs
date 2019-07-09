using BHJet_Core.Utilitario;
using BHJet_Core.Variaveis;
using BHJet_CoreGlobal;
using BHJet_CoreGlobal.GoogleUtil;
using BHJet_DTO.Corrida;
using BHJet_Enumeradores;
using BHJet_Repositorio.Admin;
using BHJet_Repositorio.Admin.Entidade;
using BHJet_Repositorio.Admin.Filtro;
using BHJet_WebApi.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        [Route("{idOS:long}")]
        [Authorize]
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
                    CaminhoProtocolo = entidade.FirstOrDefault().CaminhoProtocolo,
                    vcLatitude = entidade.FirstOrDefault().vcLatitude,
                    vcLongitude = entidade.FirstOrDefault().vcLongitude
                },
                Destinos = entidade.Count() > 1 ? entidade.Skip(1).Select(x => new DetalheOSEnderecoModel()
                {
                    EnderecoCompleto = x.EnderecoCompleto,
                    ProcurarPor = x.ProcurarPor,
                    Realizar = Realizar(x),
                    StatusCorrida = x.StatusCorrida,
                    TempoEspera = x.TempoEspera?.TimeOfDay,
                    Observacao = x.Observacao,
                    CaminhoProtocolo = x.CaminhoProtocolo,
                    vcLatitude = x.vcLatitude,
                    vcLongitude = x.vcLongitude
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
                ValorEstimado = dtm.ValorEstimado,
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
        /// Busca telefone de contato OCORRENICA corrida
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("{idCorrida:long}/contato")]
        public IHttpActionResult GetContatoOcorrencia(long idCorrida)
        {
            // Busca Dados detalhados da corrida/OS
            var telefone = new CorridaRepositorio().BuscaTelefoneClienteCorrida(idCorrida);

            // Return
            return Ok(telefone);
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
        [Route("ocorrencias")]
        [AllowAnonymous]
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

        /// <summary>
        /// Busca localização de chamados de um tipo especifico para um tipo especifico de motorista
        /// </summary>
        /// <returns>List<LocalizacaoProfissional></returns>
        [Route("{idOS:long}/cancelar")]
        [Authorize]
        [ResponseType(typeof(DetalheCorridaModel))]
        public IHttpActionResult PutCancelarOS(int idOS)
        {
            try
            {
                // Busca Dados detalhados da corrida/OS
                var corridaRep = new CorridaRepositorio();

                // Verifica se corrida pertence ao usuario solicitado
                var pertence = corridaRep.CorridaPertenceUsuario(idOS, long.Parse(UsuarioAutenticado.LoginID));
                if (!pertence)
                    return StatusCode(System.Net.HttpStatusCode.Unauthorized);

                // Verifica se já foi cancelada
                var corridaEncerrada = corridaRep.CorridaEncerrada(idOS);

                if (corridaEncerrada)
                    return ResponseMessage(new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent($"A corrida {idOS} já foi cancelada ou encerrada.")
                    });

                // Cancela corrida
                corridaRep.CancelaCorrida(idOS);

                // Return
                return Ok();
            }
            catch
            {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent)
                {
                    Content = new StringContent($"Não foi possível cancelar a OS, por favor, entre em contato com a administração da BHJet.")
                });
            }
        }

        /// <summary>
        /// LIBERAR OS
        /// </summary>
        /// /// <param name="idCorrida">long</param>
        /// <returns></returns>
        [Authorize]
        [Route("aceitar/{idCorrida:long}")]
        public IHttpActionResult PutAceitarOS(long idCorrida)
        {
            // Busca Dados detalhados da corrida/OS
            var corridaRep = new CorridaRepositorio();

            // Verifica se corrida pertence ao usuario solicitado
            var pertence = corridaRep.CorridaPertenceUsuario(idCorrida, long.Parse(UsuarioAutenticado.LoginID));
            if (!pertence)
                return StatusCode(System.Net.HttpStatusCode.Unauthorized);

            // Instancia
            new CorridaRepositorio().AceitarOrdemServico(idCorrida);

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
        [ResponseType(typeof(PrecoCorridaDTO))]
        [Route("preco")]
        public IHttpActionResult PostPrecoCorrido([FromBody]CalculoCorridaDTO model)
        {
            var valorTotal = CalculaPrecoCorrida(model);

            // Return
            return Ok(valorTotal);
        }

        private static PrecoCorridaDTO CalculaPrecoCorrida(CalculoCorridaDTO model)
        {
            //valor minimo
            //valor do ponto de coleta
            //valor por ponto de entrega
            //valor por km rodado
            //valor de espera(Mais de 10 minutos é cobrado)

            // Busca tarifa cliente
            var tarifa = new TarifaRepositorio().BuscaTarificaCorrida(model.TipoVeiculo);

            // Variaveis Preco
            var valaorPadrao = Double.Parse(tarifa.ValorContrato?.ToString() ?? "0");
            var valorPontoColeta = Double.Parse(tarifa.decValorPontoColeta?.ToString() ?? "0");
            var valorPontoEntrega = Double.Parse(tarifa.decValorPontoExcedente?.ToString() ?? "0");
            var valorEspera = Double.Parse(tarifa.decValorMinutoParado?.ToString() ?? "0");
            var valorKMAdc = Double.Parse(tarifa.ValorKMAdicional?.ToString() ?? "0");
            var quantidadeDestinos = model.Localizacao.Count() - 1;
            var valorPorMinutosEspera = model.MinutosEspera > 10 ? valorEspera * model.MinutosEspera : 0;

            // Distancia de KM
            var googleAPI = new GoogleApiUtil(ConfigurationManager.AppSettings["GoogleApiKey"]);

            // Calcula distancia
            double distanciaMetros = 0;

            // Percorre todas localizacoes
            for (int i = 0; i < model.Localizacao.Length; i++)
            {
                // Localizacoes
                var origem = model.Localizacao[i];

                if (model.Localizacao.Length <= i + 1)
                    break;

                var destino = model.Localizacao[i + 1];

                distanciaMetros += googleAPI.BuscaDistanciaMatrix(new BHJet_CoreGlobal.GoogleUtil.Model.GeoLocalizacaoMatrixModel()
                {
                    Origem = new BHJet_CoreGlobal.GoogleUtil.Model.GeoLocalizacaoModel()
                    {
                        Latitude = origem.Latitude,
                        Longitude = origem.Longitude
                    },
                    Destino = new BHJet_CoreGlobal.GoogleUtil.Model.GeoLocalizacaoModel()
                    {
                        Latitude = destino.Latitude,
                        Longitude = destino.Longitude
                    }
                }) ?? 0;
            }

            // Calculo de KM
            double distanciaKM = distanciaMetros.MetroParaKM();

            // Validacao
            if (distanciaKM == 0)
                throw new NullReferenceException("Não foi possível calcular o preço da corrida. Tente novamente mais tarde.");

            // Total calculado
            var TOTALCORRIDA = valorPontoColeta + (valorPontoEntrega * quantidadeDestinos) + (valorKMAdc * distanciaKM) + valorPorMinutosEspera;

            // Total
            if (TOTALCORRIDA < valaorPadrao)
                TOTALCORRIDA = valaorPadrao;

            // Return
            return new PrecoCorridaDTO()
            {
                Preco = TOTALCORRIDA,
                QuantidadeKM = distanciaKM
            };
        }

        /// <summary>
        /// Incluir Corrida
        /// </summary>
        /// <returns>List<DetalheCorridaModel></returns>
        [Route("")]
        [Authorize]
        [ResponseType(typeof(double))]
        public IHttpActionResult PostCorrida([FromBody]IncluirCorridaDTO model)
        {
            // Busca Comissao
            var comissao = new ProfissionalRepositorio().BuscaComissaoProfissional(model.IDProfissional ?? 0);

            // Calculo Valor Estimado
            var valorEstimado = CalculaPrecoCorrida(new CalculoCorridaDTO()
            {
                IDCliente = model.IDCliente ?? 0,
                TipoVeiculo = model.TipoProfissional ?? 0,
                Localizacao = model.Enderecos.Select(c => new CalculoCorridaLocalidadeDTO()
                {
                    Latitude = Double.Parse(c.Latitude.Replace(".", ",")),
                    Longitude = Double.Parse(c.Longitude.Replace(".", ","))
                }).ToArray()
            });

#if DEBUG
            var usuario = 55;
#else
            var usuario = long.Parse(UsuarioAutenticado.LoginID);
#endif

            // Busca tarifa cliente
            var idCorrida = new CorridaRepositorio().IncluirCorrida(new BHJet_Repositorio.Admin.Filtro.CorridaFiltro()
            {
                IDCliente = model.IDCliente,
                IDProfissional = model.IDProfissional,
                Comissao = comissao != null ? comissao.decPercentualComissao : (decimal?)null,
                TipoProfissional = model.TipoProfissional,
                ValorEstimado = valorEstimado.Preco,
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


