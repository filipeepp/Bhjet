using BHJet_Core.Extension;
using BHJet_DTO.Cliente;
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
    [RoutePrefix("api/Cliente")]
    public class ClienteController : ApiController
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

        #region Cliente Normal

        /// <summary>
        /// Busca dados de cliente e seus contratos
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("contrato")]
        [ResponseType(typeof(IEnumerable<ClienteDTO>))]
        public IHttpActionResult GetClienteContrato([FromUri]string trecho = "")
        {
            // Busca Dados resumidos
            var entidade = new ClienteRepositorio().BuscaClienteContrato(trecho);

            // Validacao
            if (entidade == null)
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(entidade.Select(cli => new ClienteDTO()
            {
                ID = cli.idCliente,
                vcNomeRazaoSocial = cli.vcNomeRazaoSocial,
                vcDescricaoTarifario = cli.vcDescricaoTarifario,
                bitAtivo = cli.bitAtivo

            }));
        }


        /// <summary>
        /// Busca dados de cliente
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("contrato/ativo")]
        [ResponseType(typeof(IEnumerable<ClienteDTO>))]
        public IHttpActionResult GetClientesValorAtivo()
        {
            // Busca Dados
            var entidade = new ClienteRepositorio().BuscaListaClientes();

            // Validacao
            if (entidade == null)
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(entidade.Select(cli => new ClienteDTO()
            {
                ID = cli.idCliente,
                vcNomeFantasia = cli.vcNomeFantasia,
                vcNomeRazaoSocial = cli.vcNomeRazaoSocial,
                vcCPFCNPJ = cli.vcCPFCNPJ,
                vcInscricaoEstadual = cli.vcInscricaoEstadual,
                bitRetemISS = cli.bitRetemISS,
                vcObservacoes = cli.vcObservacoes,
                vcSite = cli.vcSite,
                vcRua = cli.vcRua,
                vcNumero = cli.vcNumero,
                vcComplemento = cli.vcComplemento,
                vcBairro = cli.vcBairro,
                vcCidade = cli.vcCidade,
                vcUF = cli.vcUF,
                vcCEP = cli.vcCEP,
                bitAtivo = cli.bitAtivo,
                vcDescricaoTarifario = cli.vcDescricaoTarifario

            }));
        }

        /// <summary>
        /// Busca cliente completo por ID
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("{idCliente:long}")]
        [ResponseType(typeof(ClienteCompletoModel))]
        public IHttpActionResult GetClienteCompleto(long idCliente)
        {
            // Busca dados cadastrais
            var entidadeDadosCadastrais = new ClienteRepositorio().BuscaClienteDadosCadastrais(idCliente);
            // Busca contato(s)
            var entidadeContato = new ClienteRepositorio().BuscaClienteContatos(idCliente);
            // Busca valor(es)
            var entidadeValor = new ClienteRepositorio().BuscaClienteValores(idCliente);

            //Monta Cliente Completo
            if (entidadeDadosCadastrais == null && entidadeContato == null && entidadeValor == null)
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            var entidade = new ClienteCompletoModel()
            {
                ID = idCliente,
                DadosCadastrais = entidadeDadosCadastrais.Select(cli => new ClienteDadosCadastraisModel()
                {
                    ClienteAvulso = cli.ClienteAvulso,
                    NomeRazaoSocial = cli.NomeRazaoSocial,
                    NomeFantasia = cli.NomeFantasia,
                    CPFCNPJ = cli.CPFCNPJ,
                    InscricaoEstadual = cli.InscricaoEstadual,
                    ISS = cli.ISS,
                    Endereco = cli.Endereco,
                    NumeroEndereco = cli.NumeroEndereco,
                    Complemento = cli.Complemento,
                    Bairro = cli.Bairro,
                    Cidade = cli.Cidade,
                    Estado = cli.Estado,
                    CEP = cli.CEP,
                    Observacoes = cli.Observacoes,
                    HomePage = cli.HomePage

                }).FirstOrDefault(),
                Contato = entidadeContato.Select(cot => new ClienteContatoModel()
                {
                    ID = cot.ID,
                    Contato = cot.Contato,
                    Email = cot.Email,
                    TelefoneComercial = cot.TelefoneComercial,
                    TelefoneCelular = cot.TelefoneCelular,
                    TelefoneWhatsapp = cot.Whatsapp,
                    Setor = cot.Setor,
                    DataNascimento = cot.DataNascimento

                }).ToArray(),
                ContratoCarro = entidadeValor.Where(c => c.idTipoVeiculo == 2).Any() ? entidadeValor.Where(c => c.idTipoVeiculo == 2).Select(v => new ClienteValorModel()
                {
                    idTarifario = v.idTarifario,
                    FranquiaHoras = v.idTarifario,
                    FranquiaKM = v.idTarifario,
                    FranquiaMinutosParados = v.idTarifario,
                    Observacao = v.Observacao,
                    ValorContrato = v.decValorContrato,
                    ValorHoraAdicional = v.decValorHoraAdicional,
                    ValorKMAdicional = v.decValorKMAdicional,
                    ValorMinutoParado = v.decValorMinutoParado
                }).FirstOrDefault() : null,
                ContratoMoto = entidadeValor.Where(c => c.idTipoVeiculo == 1).Any() ? entidadeValor.Where(c => c.idTipoVeiculo == 1).Select(v => new ClienteValorModel()
                {
                    idTarifario = v.idTarifario,
                    FranquiaHoras = v.idTarifario,
                    FranquiaKM = v.idTarifario,
                    FranquiaMinutosParados = v.idTarifario,
                    Observacao = v.Observacao,
                    ValorContrato = v.decValorContrato,
                    ValorHoraAdicional = v.decValorHoraAdicional,
                    ValorKMAdicional = v.decValorKMAdicional,
                    ValorMinutoParado = v.decValorMinutoParado
                }).FirstOrDefault() : null
            };

            //Return
            return Ok(entidade);
        }

        /// <summary>
        /// Busca Lista de Clientes
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("")]
        [ResponseType(typeof(IEnumerable<ClienteDTO>))]
        public IHttpActionResult GetListaClientes([FromUri]string trecho = "")
        {
            // Busca Dados resumidos
            var entidade = new ClienteRepositorio().BuscaClientes(trecho);

            // Validacao
            if (entidade == null)
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(entidade.Select(cli => new ClienteDTO()
            {
                ID = cli.idCliente,
                bitRetemISS = cli.bitRetemISS,
                idEndereco = cli.idEndereco,
                idUsuario = cli.idEndereco,
                vcNomeRazaoSocial = cli.vcNomeRazaoSocial,
                vcNomeFantasia = cli.vcNomeFantasia,
                vcCPFCNPJ = cli.vcCPFCNPJ,
                vcInscricaoMunicipal = cli.vcInscricaoMunicipal,
                vcInscricaoEstadual = cli.vcInscricaoEstadual,
                vcObservacoes = cli.vcObservacoes,
                vcSite = cli.vcSite,
            }));
        }

        /// <summary>
        /// Post Cliente
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [Route("")]
        public IHttpActionResult PostCliente([FromBody]ClienteCompletoModel model)
        {
            // Busca Dados resumidos
            var clienteRepositorio = new ClienteRepositorio();

            // Verifica existencia
            var entidade = clienteRepositorio.VerificaExistenciaCliente(model.DadosCadastrais.CPFCNPJ);

            // VALIDACAO
            if (entidade.Any())
            {
                bool existeCPF = entidade.Where(x => x.vcCPFCNPJ == model.DadosCadastrais.CPFCNPJ).Any();

                string msg = "";
                if (existeCPF)
                    msg = "mesmo CPF.";

                return BadRequest($"Existe um cadastro com este {msg}. Favor atualizar os dados corretamente");
            }

            // Inclui profissional
            clienteRepositorio.IncluirCliente(new BHJet_Repositorio.Admin.Entidade.ClienteCompletoEntidade()
            {
                DadosCadastrais = new BHJet_Repositorio.Admin.Entidade.ClienteDadosCadastraisEntidade()
                {
                    ClienteAvulso = model.DadosCadastrais.ClienteAvulso,
                    NomeRazaoSocial = model.DadosCadastrais.NomeRazaoSocial,
                    NomeFantasia = model.DadosCadastrais.NomeFantasia,
                    CPFCNPJ = model.DadosCadastrais.CPFCNPJ,
                    InscricaoEstadual = model.DadosCadastrais.InscricaoEstadual,
                    ISS = model.DadosCadastrais.ISS,
                    Endereco = model.DadosCadastrais.Endereco,
                    NumeroEndereco = model.DadosCadastrais.NumeroEndereco,
                    Complemento = model.DadosCadastrais.Complemento,
                    Bairro = model.DadosCadastrais.Bairro,
                    Cidade = model.DadosCadastrais.Cidade,
                    Estado = model.DadosCadastrais.Estado,
                    CEP = model.DadosCadastrais.CEP,
                    Observacoes = model.DadosCadastrais.Observacoes,
                    HomePage = model.DadosCadastrais.HomePage
                },
                Contato = model.Contato.Any() ? model.Contato.Select(x => new BHJet_Repositorio.Admin.Entidade.ClienteContatoEntidade()
                {
                    Contato = x.Contato,
                    Email = x.Email,
                    TelefoneComercial = x.TelefoneComercial,
                    TelefoneCelular = x.TelefoneCelular,
                    Setor = x.Setor,
                    DataNascimento = x.DataNascimento

                }).ToArray() : new BHJet_Repositorio.Admin.Entidade.ClienteContatoEntidade[] { },
                ContratoMoto = model.ContratoMoto != null ? new BHJet_Repositorio.Admin.Entidade.ClienteValorEntidade()
                {
                    idTarifario = model.ContratoMoto.idTarifario,
                    decValorMinutoParado = model.ContratoMoto.ValorMinutoParado,
                    decValorKMAdicional = model.ContratoMoto.ValorKMAdicional,
                    decValorContrato = model.ContratoMoto.ValorContrato ?? 0,
                    decValorHoraAdicional = model.ContratoMoto.ValorHoraAdicional,
                    intFranquiaHoras = model.ContratoMoto.FranquiaHoras,
                    intFranquiaKM = model.ContratoMoto.FranquiaKM,
                    intFranquiaMinutosParados = model.ContratoMoto.FranquiaMinutosParados,
                    Observacao = model.ContratoMoto.Observacao
                } : null,
                ContratoCarro = model.ContratoMoto != null ? new BHJet_Repositorio.Admin.Entidade.ClienteValorEntidade()
                {
                    idTarifario = model.ContratoCarro.idTarifario,
                    decValorMinutoParado = model.ContratoCarro.ValorMinutoParado,
                    decValorKMAdicional = model.ContratoCarro.ValorKMAdicional,
                    decValorContrato = model.ContratoCarro.ValorContrato ?? 0,
                    decValorHoraAdicional = model.ContratoCarro.ValorHoraAdicional,
                    intFranquiaHoras = model.ContratoCarro.FranquiaHoras,
                    intFranquiaKM = model.ContratoCarro.FranquiaKM,
                    intFranquiaMinutosParados = model.ContratoCarro.FranquiaMinutosParados,
                    Observacao = model.ContratoCarro.Observacao
                } : null
            });

            // Return
            return Ok();
        }

        /// <summary>
        /// Post Cliente
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [Route("avulso")]
        public IHttpActionResult PostClienteAvulso([FromBody]ClienteAvulsoDTO model)
        {
            // Busca Dados resumidos
            var clienteRepositorio = new ClienteRepositorio();

            // Verifica existencia
            var entidade = clienteRepositorio.VerificaExistenciaCliente(model.CPF);

            // VALIDACAO
            if (entidade.Any())
            {
                bool existeCPF = entidade.Where(x => x.vcCPFCNPJ == model.CPF).Any();

                string msg = "";
                if (existeCPF)
                    msg = "mesmo CPF.";

                return BadRequest($"Existe um cadastro com este {msg}. Favor atualizar os dados corretamente");
            }

            // Incluir Cliente Avulso
            var idCliente = clienteRepositorio.IncluirCliente(new BHJet_Repositorio.Admin.Entidade.ClienteCompletoEntidade()
            {
                DadosCadastrais = new BHJet_Repositorio.Admin.Entidade.ClienteDadosCadastraisEntidade()
                {
                    ClienteAvulso = true,
                    NomeRazaoSocial = string.Empty,
                    NomeFantasia = string.Empty,
                    CPFCNPJ = model.CPF,
                    InscricaoEstadual = string.Empty,
                    ISS = 0,
                    Endereco = model.Rua,
                    NumeroEndereco = model.Numero.ToString(),
                    Complemento = string.Empty,
                    Bairro = model.Bairro,
                    Cidade = model.Cidade,
                    Estado = model.Estado,
                    CEP = model.CEP,
                    Observacoes = string.Empty,
                    HomePage = string.Empty
                },
                Contato = new BHJet_Repositorio.Admin.Entidade.ClienteContatoEntidade[]
                {
                    new BHJet_Repositorio.Admin.Entidade.ClienteContatoEntidade()
                    {
                         Contato = model.Nome,
                          DataNascimento = model.DataNascimento,
                           Email = model.Email,
                            TelefoneCelular = model.Celular,
                             TelefoneComercial = model.Comercial,
                              Setor = "",
                               TelefoneRamal = "",
                                Whatsapp = 1
                    }
                }
            });

            // Incluir Usuario
            new UsuarioRepositorio().IncluirUsuario(new BHJet_Repositorio.Entidade.UsuarioEntidade()
            {
                bitAtivo = true,
                ClienteSelecionado = idCliente,
                ColaboradorSelecionado = null,
                idTipoUsuario = BHJet_Enumeradores.TipoUsuario.ClienteAvulsoSite,
                vcEmail = model.Email,
                vbIncPassword = model.Senha,
            });

            // Return
            return Ok();
        }

        /// <summary>
        /// Post Cliente Contato
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [Route("{idCliente:long}/contato")]
        public IHttpActionResult PostClienteContato(long idCliente, [FromBody]ClienteContatoModel model)
        {
            // Busca Dados resumidos
            var clienteRepositorio = new ClienteRepositorio();

            // Inclui profissional
            clienteRepositorio.IncluirContato(idCliente, new BHJet_Repositorio.Admin.Entidade.ClienteContatoEntidade()
            {
                Contato = model.Contato,
                Email = model.Email,
                TelefoneComercial = model.TelefoneComercial,
                TelefoneCelular = model.TelefoneCelular,
                Setor = model.Setor,
                DataNascimento = model.DataNascimento
            });

            //Return
            return Ok();
        }


        /// <summary>
        /// Put Cliente
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [Route("{idCliente:long}")]
        public IHttpActionResult PutCliente(long idCliente, [FromBody]ClienteCompletoModel model)
        {
            // Busca Dados resumidos
            var clienteRepositorio = new ClienteRepositorio();

            //Busca ID do Endereço
            var idEndereco = clienteRepositorio.BuscaClienteEndereco(idCliente).FirstOrDefault().idEndereco;

            // Inclui profissional
            clienteRepositorio.AtualizaCliente(idEndereco, new BHJet_Repositorio.Admin.Entidade.ClienteCompletoEntidade()
            {
                ID = model.ID,
                DadosCadastrais = new BHJet_Repositorio.Admin.Entidade.ClienteDadosCadastraisEntidade()
                {
                    ClienteAvulso = model.DadosCadastrais.ClienteAvulso,
                    NomeRazaoSocial = model.DadosCadastrais.NomeRazaoSocial,
                    NomeFantasia = model.DadosCadastrais.NomeFantasia,
                    CPFCNPJ = model.DadosCadastrais.CPFCNPJ,
                    InscricaoEstadual = model.DadosCadastrais.InscricaoEstadual,
                    ISS = model.DadosCadastrais.ISS,
                    Endereco = model.DadosCadastrais.Endereco,
                    NumeroEndereco = model.DadosCadastrais.NumeroEndereco,
                    Complemento = model.DadosCadastrais.Complemento,
                    Bairro = model.DadosCadastrais.Bairro,
                    Cidade = model.DadosCadastrais.Cidade,
                    Estado = model.DadosCadastrais.Estado,
                    CEP = model.DadosCadastrais.CEP,
                    Observacoes = model.DadosCadastrais.Observacoes,
                    HomePage = model.DadosCadastrais.HomePage
                },
                Contato = model.Contato.Any() ? model.Contato.Select(x => new BHJet_Repositorio.Admin.Entidade.ClienteContatoEntidade()
                {
                    ID = x.ID,
                    Contato = x.Contato,
                    Email = x.Email,
                    TelefoneComercial = x.TelefoneComercial,
                    TelefoneCelular = x.TelefoneCelular,
                    Setor = x.Setor,
                    DataNascimento = x.DataNascimento

                }).ToArray() : new BHJet_Repositorio.Admin.Entidade.ClienteContatoEntidade[] { },
                ContratoMoto = model.ContratoMoto != null ? new BHJet_Repositorio.Admin.Entidade.ClienteValorEntidade()
                {
                    idTarifario = model.ContratoMoto.idTarifario,
                    decValorMinutoParado = model.ContratoMoto.ValorMinutoParado,
                    decValorKMAdicional = model.ContratoMoto.ValorKMAdicional,
                    decValorContrato = model.ContratoMoto.ValorContrato ?? 0,
                    decValorHoraAdicional = model.ContratoMoto.ValorHoraAdicional,
                    intFranquiaHoras = model.ContratoMoto.FranquiaHoras,
                    intFranquiaKM = model.ContratoMoto.FranquiaKM,
                    intFranquiaMinutosParados = model.ContratoMoto.FranquiaMinutosParados,
                    Observacao = model.ContratoMoto.Observacao
                } : null,
                ContratoCarro = model.ContratoMoto != null ? new BHJet_Repositorio.Admin.Entidade.ClienteValorEntidade()
                {
                    idTarifario = model.ContratoCarro.idTarifario,
                    decValorMinutoParado = model.ContratoCarro.ValorMinutoParado,
                    decValorKMAdicional = model.ContratoCarro.ValorKMAdicional,
                    decValorContrato = model.ContratoCarro.ValorContrato ?? 0,
                    decValorHoraAdicional = model.ContratoCarro.ValorHoraAdicional,
                    intFranquiaHoras = model.ContratoCarro.FranquiaHoras,
                    intFranquiaKM = model.ContratoCarro.FranquiaKM,
                    intFranquiaMinutosParados = model.ContratoCarro.FranquiaMinutosParados,
                    Observacao = model.ContratoCarro.Observacao
                } : null
            });

            // Return
            return Ok();
        }


        /// <summary>
        /// Deleta contato específico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [Route("contato/{idContato:int}")]
        public IHttpActionResult DeleteContato(int idContato)
        {
            // Instancia
            var clienteRepositorio = new ClienteRepositorio();

            // Deleta contrato
            clienteRepositorio.ExcluiContato(idContato);

            // Return
            return Ok();
        }

        ///// <summary>
        ///// Deleta contrato específico
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[Authorize]
        //[Route("contrato/{idValor:int}")]
        //public IHttpActionResult DeleteValor(int idValor)
        //{
        //    // Instancia
        //    var clienteRepositorio = new ClienteRepositorio();

        //    //Verifica se o contrato a ser excluido é o ativo
        //    var contratoAtivo = clienteRepositorio.BuscaClienteContratoAtivo(idValor);

        //    if (contratoAtivo.FirstOrDefault().)
        //        return BadRequest("O valor a ser excluído é o que está ativo para este cliente. Verifique ou edite o contrato ativo antes de excluí-lo");

        //    // Deleta contrato
        //    clienteRepositorio.ExcluiContrato(idValor);

        //    // Return
        //    return Ok();
        //}
        #endregion

        #region Cliente Avulso

        /// <summary>
        /// Busca dados de cliente avulso
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("avulso/contrato/ativo")]
        [ResponseType(typeof(IEnumerable<ClienteDTO>))]
        public IHttpActionResult GetClientesAvulsosValorAtivo()
        {
            // Busca Dados
            var entidade = new ClienteRepositorio().BuscaListaClientes(true);

            // Validacao
            if (entidade == null)
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(entidade.Select(cli => new ClienteDTO()
            {
                ID = cli.idCliente,
                vcNomeFantasia = cli.vcNomeFantasia,
                vcNomeRazaoSocial = cli.vcNomeRazaoSocial,
                vcCPFCNPJ = cli.vcCPFCNPJ,
                vcInscricaoEstadual = cli.vcInscricaoEstadual,
                bitRetemISS = cli.bitRetemISS,
                vcObservacoes = cli.vcObservacoes,
                vcSite = cli.vcSite,
                vcRua = cli.vcRua,
                vcNumero = cli.vcNumero,
                vcComplemento = cli.vcComplemento,
                vcBairro = cli.vcBairro,
                vcCidade = cli.vcCidade,
                vcUF = cli.vcUF,
                vcCEP = cli.vcCEP,
                bitAtivo = cli.bitAtivo,
                vcDescricaoTarifario = cli.vcDescricaoTarifario

            }));
        }

        #endregion

    }
}