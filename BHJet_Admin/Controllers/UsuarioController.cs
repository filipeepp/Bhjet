using BHJet_Admin.Infra;
using BHJet_Admin.Models.Usuario;
using BHJet_Servico.Usuario;
using System;
using System.Linq;
using System.Web.Mvc;

namespace BHJet_Admin.Controllers
{
    public class UsuarioController : Controller
    {
        private IUsuarioServico usuariosServico;

        public UsuarioController(IUsuarioServico _usuariosServico)
        {
            usuariosServico = _usuariosServico;
        }

        // GET: Usuario
        [ValidacaoUsuarioAttribute()]
        public ActionResult Index(string trechoPqs = "")
        {
            try
            {
                // Clear
                ModelState.Clear();

                // Busca Usuarios
                var model = usuariosServico.BuscaListaUsuarios(trechoPqs);

                // Return
                return View(new UsuariosModel()
                {
                    usuarios = model.Select(usu => new UsuarioModel()
                    {
                        ID = usu.ID,
                        Email = usu.Email,
                        Situacao = usu.Situacao,
                        SituacaoDesc = usu.SituacaoDesc,
                        TipoUser = usu.TipoUsuario
                    }).ToArray(),
                     novo = null
                });
            }
            catch (Exception e)
            {
                this.TrataErro(e);
                // Return
                return View(new UsuariosModel()
                {
                    usuarios = new UsuarioModel[] { }
                });
            }
        }

        // POST: Usuario
        [HttpPost]
        [ValidacaoUsuarioAttribute()]
        public ActionResult Index(UsuariosModel model)
        {
            try
            {
                // Cadastra usuario
                usuariosServico.CadastrarUsuario(new BHJet_DTO.Usuario.UsuarioDTO()
                {
                    Email = model.novo.Email,
                    Situacao = model.novo.Situacao,
                    TipoUsuario = model.novo.TipoUser,
                    Senha = model.novo.Senha
                });

                // Busca Usuarios
                return BuscaUsuariosSemValidacao();
            }
            catch (Exception e)
            {
                this.TrataErro(e);
                // Return
                return BuscaUsuariosSemValidacao();
            }
        }

        private ViewResult BuscaUsuariosSemValidacao()
        {
            try
            {
                // Busca Usuarios
                var modelRetorno = usuariosServico.BuscaListaUsuarios("");

                // Return
                return View(new UsuariosModel()
                {
                    usuarios = modelRetorno.Select(usu => new UsuarioModel()
                    {
                        ID = usu.ID,
                        Email = usu.Email,
                        Situacao = usu.Situacao,
                        SituacaoDesc = usu.SituacaoDesc,
                        TipoUser = usu.TipoUsuario
                    }).ToArray()
                });
            }
            catch
            {
                return View(new UsuariosModel()
                {
                    usuarios = new UsuarioModel[] { }
                });
            }
        }

        [HttpGet]
        [ValidacaoUsuarioAttribute()]
        public JsonResult DeletaUsuario(long id)
        {
            // Recupera dados
            usuariosServico.DeletaUsuario(id);

            // Return
            return Json("Usuário deletado com sucesso !", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ValidacaoUsuarioAttribute()]
        public JsonResult AlteraSituacao(int situacao, long id)
        {
            // Recupera dados
            usuariosServico.AtualizaSituacao(situacao, id);

            var msg = situacao == 1 ? "ativado" : "desativado";

            // Return
            return Json($"Usuário {msg} com sucesso !", JsonRequestBehavior.AllowGet);
        }
    }
}
