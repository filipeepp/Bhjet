﻿using BHJet_DTO.Usuario;
using BHJet_Repositorio.Admin;
using System.Linq;
using System.Web.Http;

namespace BHJet_WebApi.Controllers
{
    [RoutePrefix("api/Usuarios")]
    public class UsuarioController : ApiController
    {
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
        /// Busca lista de usuarios
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
                return BadRequest("Já existe um usário cadastrado para o email informado.");

            // Cadastra Usuario
            usuRep.IncluirUsuario(new BHJet_Repositorio.Entidade.UsuarioEntidade()
            {
                vcEmail = model.Email,
                bitAtivo = model.Situacao,
                idTipoUsuario = model.TipoUsuario,
                vbIncPassword = model.Senha
            });

            // Return
            return Ok();
        }

        /// <summary>
        /// Deleta usuario especifico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [Route("{id:long}")]
        public IHttpActionResult DeleteSituacao(long id)
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


    }
}