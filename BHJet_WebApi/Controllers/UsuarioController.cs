using BHJet_Core.Utilitario;
using BHJet_DTO.Usuario;
using BHJet_Repositorio.Admin;
using BHJet_Repositorio.Admin.Entidade;
using BHJet_WebApi.Util;
using System;
using System.IO;
using System.Linq;
using System.Web.Http;

namespace BHJet_WebApi.Controllers
{
    [RoutePrefix("Usuarios")]
    public class UsuarioController : ApiController
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
        /// Busca lista de usuarios
        /// </summary>
        /// <param name="trecho"></param>
        /// <returns></returns>
        [Authorize]
        [Route("")]
        public IHttpActionResult GetUsuarios([FromUri]string trecho = "")
        {
            // Busca Usuarios
            var entidade = new UsuarioRepositorio().BuscaUsuarios(trecho);

            // Validacao
            if (entidade == null)
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(entidade.Select(us => new UsuarioDTO()
            {
                ID = us.idUsuario,
                Email = us.vcEmail,
                Situacao = us.bitAtivo,
                SituacaoDesc = us.bitDescAtivo,
                TipoUsuario = us.idTipoUsuario
            }));
        }

        /// <summary>
        /// Cadastra usuario
        /// </summary>
        /// <param name="model">UsuarioDTO</param>
        /// <returns></returns>
        [Authorize]
        [Route("")]
        public IHttpActionResult PostUsuarios([FromBody]UsuarioDTO model)
        {
            // Instancia
            var usuRep = new UsuarioRepositorio();

            // Busca Usuarios
            var entidade = usuRep.BuscaUsuarios(model.Email);

            // Validacao
            if (entidade != null && entidade.Any() && entidade.Where(x => x.vcEmail == model.Email).Any())
                return BadRequest("Já existe um usuário cadastrado para o email informado.");

            // Cadastra Usuario
            usuRep.IncluirUsuario(new BHJet_Repositorio.Entidade.UsuarioEntidade()
            {
                vcEmail = model.Email,
                bitAtivo = model.Situacao,
                idTipoUsuario = model.TipoUsuario,
                vbPassword = model.Senha,
                ClienteSelecionado = model.ClienteSelecionado
            });

            // Return
            return Ok();
        }

        /// <summary>
        /// Atualiza usuario
        /// </summary>
        /// <param name="model">UsuarioDTO</param>
        /// <returns></returns>
        [Authorize]
        [Route("")]
        public IHttpActionResult PutUsuarios([FromBody]UsuarioDTO model)
        {
            // Instancia
            var usuRep = new UsuarioRepositorio();

            // Busca Usuarios
            var entidade = usuRep.BuscaUsuarios(model.Email);

            // Validacao
            var idEncontrado = entidade.Where(x => x.vcEmail == model.Email)?.FirstOrDefault()?.idUsuario ?? model.ID;
            if ((entidade != null && entidade.Count() > 1) || (entidade != null && idEncontrado != model.ID))
                return BadRequest("Já existe um usuário cadastrado para o email informado.");

            // Busca Usuarios
            usuRep.AtualizaUsuario(new BHJet_Repositorio.Entidade.UsuarioEntidade()
            {
                idUsuario = model.ID,
                bitAtivo = model.Situacao,
                vcEmail = model.Email,
                idTipoUsuario = model.TipoUsuario,
                ClienteSelecionado = model.ClienteSelecionado,
                vbPassword = model.Senha
            });

            // Return
            return Ok();
        }

        /// <summary>
        /// Busca usuario especifico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [Route("{id:long}")]
        public IHttpActionResult GetUsuario(long id)
        {
            // Instancia
            var usuRep = new UsuarioRepositorio();

            // Cadastra Usuario
            var usuario = usuRep.BuscaUsuario(id);

            // Validação
            if (usuario == null)
                BadRequest($"Usuário {id} não encontrado.");

            // Return
            return Ok(new UsuarioDTO()
            {
                ID = usuario.idUsuario,
                ClienteSelecionado = usuario.ClienteSelecionado,
                Email = usuario.vcEmail,
                Senha = "",
                Situacao = usuario.bitAtivo,
                SituacaoDesc = usuario.bitDescAtivo,
                TipoUsuario = usuario.idTipoUsuario
            });
        }

        /// <summary>
        /// Deleta usuario especifico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id:long}")]
        public IHttpActionResult DeletaUsuario(long id)
        {
            // Instancia
            var usuRep = new UsuarioRepositorio();

            // Cadastra Usuario
            usuRep.DeletaUsuario(id);

            // Return
            return Ok();
        }

        /// <summary>
        /// Atualiza situação do usuario
        /// </summary>
        /// <param name="situacao"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [Route("situacao/{situacao:int}/usuario/{id:long}")]
        public IHttpActionResult PutSituacao(int situacao, long id)
        {
            // Instancia
            var usuRep = new UsuarioRepositorio();

            // Cadastra Usuario
            usuRep.AtualizaSituacaoUsuario(id, situacao);

            // Return
            return Ok();
        }

        /// <summary>
        /// Busca usuario especifico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [Route("perfil")]
        public IHttpActionResult GetUsuarioLogado()
        {
            // Instancia
            var usuRep = new UsuarioRepositorio();
            var id = long.Parse(UsuarioAutenticado.LoginID);

            // Cadastra Usuario
            var usuario = usuRep.BuscaUsuario(id);

            // Validação
            if (usuario == null)
                BadRequest($"Usuário {id} não encontrado.");

            // Return
            return Ok(new UsuarioDTO()
            {
                ID = usuario.idUsuario,
                ClienteSelecionado = usuario.ClienteSelecionado,
                Email = usuario.vcEmail,
                Senha = "",
                Situacao = usuario.bitAtivo,
                SituacaoDesc = usuario.bitDescAtivo,
                TipoUsuario = usuario.idTipoUsuario
            });
        }

        /// <summary>
        /// Recupera senha de usuario especifico
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Route("recuperar/Senha")]
        public IHttpActionResult PostRecuperaSenha([FromBody]string email)
        {
            // Busca Senha do usuario
            var usuario = new UsuarioRepositorio().BuscaUsuario(email);

            // Busca template de email
            //var template = new EmailRepositorio().BuscaTemplate(1);
            var template = new EmailEntidade()
            {
                id = 1,
                vcAssunto = "testando email",
                vcCorpo = File.ReadAllText(@"C:\Users\lhsilva\Desktop\tempEmail.html")
            };

            // Verifica existencia
            if (usuario == null)
                BadRequest($"Usuário {email} não encontrado.");

            // Corpo email
            var senhaDividida = divideSenha(usuario.vbPassword);
            string corpo = string.Format(template.vcCorpo, email, senhaDividida.Item2, senhaDividida.Item1);

            // Instancia
            Email.EnviaMensagemEmail(email, template.vcAssunto, template.vcCorpo);

            // Return
            return Ok("Sua senha foi enviada para seu e-mail.");
        }

        private Tuple<string, string> divideSenha(string senha)
        {
            var partes = senha.Length / 2;
            return new Tuple<string, string>(senha.Substring(0, partes), senha.Substring(partes, senha.Length));
        }

    }
}