using BHJet_Admin.Infra;
using BHJet_Admin.Models;
using BHJet_Admin.Models.Usuario;
using BHJet_Servico.Cliente;
using BHJet_Servico.Usuario;
using System;
using System.Linq;
using System.Web.Mvc;

namespace BHJet_Admin.Controllers
{
    public class UsuarioController : Controller
    {
        private IUsuarioServico usuariosServico;
        private readonly IClienteServico clienteServico;

        public UsuarioController(IUsuarioServico _usuariosServico, IClienteServico _clienteServico)
        {
            usuariosServico = _usuariosServico;
            clienteServico = _clienteServico;
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
                    usuarios = model.Select(usu => new UsuarioDetalheModel()
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
                    usuarios = new UsuarioDetalheModel[] { }
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
                    Senha = model.novo.Senha,
                    ClienteSelecionado = model.novo.ClienteSelecionado
                });

                model.novo = new UsuarioDetalheModel();
                this.MensagemSucesso("Usuário cadastrado com sucesso.");

                // Busca Usuarios
                return BuscaUsuariosSemValidacao(model);
            }
            catch (Exception e)
            {
                this.TrataErro(e);
                // Return
                return BuscaUsuariosSemValidacao(model);
            }
        }

        private ViewResult BuscaUsuariosSemValidacao(UsuariosModel model)
        {
            try
            {
                // Busca Usuarios
                var modelRetorno = usuariosServico.BuscaListaUsuarios("");

                // Return
                return View(new UsuariosModel()
                {
                    usuarios = modelRetorno.Select(usu => new UsuarioDetalheModel()
                    {
                        ID = usu.ID,
                        Email = usu.Email,
                        Situacao = usu.Situacao,
                        SituacaoDesc = usu.SituacaoDesc,
                        TipoUser = usu.TipoUsuario
                    }).ToArray(),
                    novo = model.novo
                });
            }
            catch
            {
                return View(new UsuariosModel()
                {
                    usuarios = new UsuarioDetalheModel[] { },
                    novo = model.novo
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

        [HttpGet]
        [ValidacaoUsuarioAttribute()]
        public JsonResult BuscaClientes(string trechoPesquisa)
        {
            // Recupera dados
            var entidade = clienteServico.BuscaListaClientes(trechoPesquisa);

            // Return
            return Json(entidade.Select(x => new AutoCompleteModel()
            {
                label = x.ID + " - " + x.vcNomeFantasia,
                value = x.ID
            }), JsonRequestBehavior.AllowGet);
        }
    }
}
