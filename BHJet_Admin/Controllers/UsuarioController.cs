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

        [ValidacaoUsuarioAttribute()]
        public ActionResult Index()
        {
            return View();
        }

        // GET: Usuario
        [ValidacaoUsuarioAttribute()]
        public ActionResult Listar(string trechoPqs = "")
        {
            try
            {
                // Clear
                ModelState.Clear();

                // Busca Usuarios
                var model = usuariosServico.BuscaListaUsuarios(trechoPqs);

                // Return
                return View(model.Select(usu => new UsuarioModel()
                {
                    ID = usu.ID,
                    Email = usu.Email,
                    Situacao = usu.Situacao,
                    SituacaoDesc = usu.SituacaoDesc,
                    TipoUser = usu.TipoUsuario
                }));
            }
            catch (Exception e)
            {
                this.TrataErro(e);
                // Return
                return View(new UsuarioModel());
            }
        }

        [ValidacaoUsuarioAttribute()]
        public ActionResult Novo(bool? Edicao, long? ID)
        {
            if (Edicao != null && Edicao == true && ID != null)
            {
                ModelState.Clear();

                // Tipo de Execução
                ViewBag.TipoAlteracao = "Editar";

                // Busca dados do profissional
                var usuario = usuariosServico.BuscaUsuario(ID ?? 0);

                // Return
                return View(new UsuarioModel()
                {
                    ID = usuario.ID,
                    Email = usuario.Email,
                    Senha = string.Empty,
                    EdicaoCadastro = true,
                    Situacao = usuario.Situacao,
                    TipoUser = usuario.TipoUsuario,
                    SituacaoDesc = usuario.SituacaoDesc,
                    ClienteSelecionado = usuario.ClienteSelecionado,
                    ClienteSelecionadoBKP = usuario.ClienteSelecionado
                });
            }
            else
            {
                ViewBag.TipoAlteracao = "Adicionar";

                return View(new UsuarioModel()
                {
                    EdicaoCadastro = false
                });
            }
        }

        // POST: Usuario
        [HttpPost]
        [ValidacaoUsuarioAttribute()]
        public ActionResult Novo(UsuarioModel model)
        {
            try
            {
                // Modelo
                var cliente = new BHJet_DTO.Usuario.UsuarioDTO()
                {
                    Email = model.Email,
                    Situacao = model.Situacao,
                    TipoUsuario = model.TipoUser,
                    Senha = model.Senha,
                    ClienteSelecionado = model.ClienteSelecionado
                };

                // Ação
                if (model.EdicaoCadastro)
                    usuariosServico.CadastrarUsuario(cliente);
                else
                    usuariosServico.CadastrarUsuario(cliente);

                model = new UsuarioModel();
                this.MensagemSucesso("Usuário cadastrado com sucesso.");

                // Busca Usuarios
                return View(model);
            }
            catch (Exception e)
            {
                this.TrataErro(e);
                // Return
                return View(model);
            }
        }

        private ViewResult BuscaUsuariosSemValidacao(UsuarioModel model)
        {
            try
            {
                // Busca Usuarios
                var modelRetorno = usuariosServico.BuscaListaUsuarios("");

                // Return
                return View(modelRetorno.Select(usu => new UsuarioModel()
                {
                    ID = usu.ID,
                    Email = usu.Email,
                    Situacao = usu.Situacao,
                    SituacaoDesc = usu.SituacaoDesc,
                    TipoUser = usu.TipoUsuario
                }));
            }
            catch
            {
                return View(new UsuarioModel());
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
