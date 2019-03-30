using BHJet_Admin.Infra;
using BHJet_Admin.Models;
using BHJet_Admin.Models.Usuario;
using BHJet_Enumeradores;
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

        [ValidacaoUsuarioAttribute(TipoUsuario.Administrador)]
        public ActionResult Index()
        {
            return View();
        }

        // GET: Usuario
        [ValidacaoUsuarioAttribute(TipoUsuario.Administrador)]
        public ActionResult Listar(string trechoPqs = "")
        {
            try
            {
                // Clear
                ModelState.Clear();

                // Busca Usuarios
                var model = usuariosServico.BuscaListaUsuarios(trechoPqs);

                // Tira profissionais da lista
                if (model != null && model.Any())
                    model = model.Where(usu => usu.TipoUsuario != BHJet_Enumeradores.TipoUsuario.Profissional).ToArray();

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

        [ValidacaoUsuarioAttribute(TipoUsuario.Administrador)]
        public ActionResult Novo(bool? Edicao, long? ID)
        {

            if (Edicao != null && Edicao == true && ID != null)
            {
                ModelState.Clear();

                // Tipo de Execução
                SetaBotaoAcao(true);

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
                // Tipo de Execução
                SetaBotaoAcao(false);

                return View(new UsuarioModel()
                {
                    EdicaoCadastro = false
                });
            }
        }

        // POST: Usuario
        [HttpPost]
        [ValidacaoUsuarioAttribute(TipoUsuario.Administrador)]
        public ActionResult Novo(UsuarioModel model)
        {
            try
            {
                // Modelo
                var usuario = new BHJet_DTO.Usuario.UsuarioDTO()
                {
                    ID = model.ID,
                    Email = model.Email,
                    Situacao = model.Situacao,
                    TipoUsuario = model.TipoUser,
                    Senha = model.Senha,
                    ClienteSelecionado = model.ClienteSelecionado
                };

                // Ação
                if (model.EdicaoCadastro)
                    usuariosServico.AtualizaUsuario(usuario);
                else
                    usuariosServico.CadastrarUsuario(usuario);

                model = new UsuarioModel(model.EdicaoCadastro);
                this.MensagemSucesso("Solicitação realizada com sucesso.");

                // Busca Usuarios
                return View(model);
            }
            catch (Exception e)
            {
                this.TrataErro(e);
                // Return
                return View(model);
            }
            finally
            {
                SetaBotaoAcao(model.EdicaoCadastro);
            }
        }

        private void SetaBotaoAcao(bool edicao)
        {
            if (edicao)
                ViewBag.TipoAlteracao = "Editar";
            else
                ViewBag.TipoAlteracao = "Adicionar";
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
        [ValidacaoUsuarioAttribute(TipoUsuario.Administrador)]
        public JsonResult DeletaUsuario(long id)
        {
            // Recupera dados
            usuariosServico.DeletaUsuario(id);

            // Return
            return Json("Usuário deletado com sucesso !", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ValidacaoUsuarioAttribute(TipoUsuario.Administrador)]
        public JsonResult AlteraSituacao(int situacao, long id)
        {
            // Recupera dados
            usuariosServico.AtualizaSituacao(situacao, id);

            var msg = situacao == 1 ? "ativado" : "desativado";

            // Return
            return Json($"Usuário {msg} com sucesso !", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ValidacaoUsuarioAttribute(TipoUsuario.Administrador)]
        public JsonResult BuscaClientes(string trechoPesquisa)
        {
            // Recupera dados
            var entidade = clienteServico.BuscaListaClientes(trechoPesquisa);

            // Return
            return Json(entidade.Where(c => c.bitAvulso == false).Select(x => new AutoCompleteModel()
            {
                label = x.ID + " - " + x.vcNomeFantasia,
                value = x.ID
            }), JsonRequestBehavior.AllowGet);
        }
    }
}
