using BHJet_Enumeradores;
using System.Web.Mvc;

namespace BHJet_Admin.Infra
{
    public class UsuarioLogado : Controller
    {
        private static UsuarioLogado instance;

        private UsuarioLogado() { }

        public static UsuarioLogado Instance
        {
            get
            {
                if (instance == null)
                    lock (typeof(UsuarioLogado))
                        if (instance == null) instance = new UsuarioLogado();

                return instance;
            }
        }

        public static void Logar(string idUsuario, long? idCliente, string token, string email, TipoUsuario tipo)
        {
            UsuarioLogado.instance.bhIdCli = idCliente;
            UsuarioLogado.instance.bhIdUsu = idUsuario;
            UsuarioLogado.instance.bhTkUsu = token;
            UsuarioLogado.instance.BhjTpUsu = tipo;
            UsuarioLogado.instance.bhEmlUsu = email;
        }

        public static void Logoff()
        {
            UsuarioLogado.instance.bhIdCli = null;
            UsuarioLogado.instance.bhIdUsu = string.Empty;
            UsuarioLogado.instance.bhTkUsu = string.Empty;
            UsuarioLogado.instance.BhjTpUsu = null;
            UsuarioLogado.instance.bhEmlUsu = string.Empty;
        }


        public string bhIdUsu
        {
            get
            {
                if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["bhIdUsu"] != null)
                    return (string)System.Web.HttpContext.Current.Session["bhIdUsu"];
                else
                    return string.Empty;
            }
            set => System.Web.HttpContext.Current.Session["bhIdUsu"] = value;
        }

        public string bhTkUsu
        {
            get
            {
                if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["bhTkUsu"] != null)
                    return (string)System.Web.HttpContext.Current.Session["bhTkUsu"];
                else
                    return string.Empty;
            }
            set => System.Web.HttpContext.Current.Session["bhTkUsu"] = value;
        }

        public string bhEmlUsu
        {
            get
            {
                if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["bhEmlUsu"] != null)
                    return (string)System.Web.HttpContext.Current.Session["bhEmlUsu"];
                else
                    return string.Empty;
            }
            set => System.Web.HttpContext.Current.Session["bhEmlUsu"] = value;
        }

        public long? bhIdCli
        {
            get
            {
                if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["bhIdCli"] != null)
                    return (long)System.Web.HttpContext.Current.Session["bhIdCli"];
                else
                    return null;
            }
            set => System.Web.HttpContext.Current.Session["bhIdCli"] = value;
        }

        public TipoUsuario? BhjTpUsu
        {
            get
            {
                if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["BhjTpUsu"] != null)
                    return (TipoUsuario)System.Web.HttpContext.Current.Session["BhjTpUsu"];
                else
                    return TipoUsuario.ClienteAvulsoSite;
            }
            set => System.Web.HttpContext.Current.Session["BhjTpUsu"] = value;
        }
    }
}