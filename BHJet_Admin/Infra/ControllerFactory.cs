using BHJet_Servico.Autorizacao;
using BHJet_Servico.Cliente;
using BHJet_Servico.Corrida;
using BHJet_Servico.Dashboard;
using BHJet_Servico.Diaria;
using BHJet_Servico.Profissional;
using BHJet_Servico.Tarifa;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BHJet_Servico.Faturamento;
using BHJet_Servico.Usuario;
using BHJet_Servico.Area;

namespace BHJet_Admin.Infra
{
    public class ControllerFactory : DefaultControllerFactory
    {
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {

            var controllerType = GetControllerType(requestContext, controllerName);

            if (controllerType.GetConstructors().Any(ct => ct.GetParameters().Count() == 0))
                return Activator.CreateInstance(controllerType) as IController;
            else
            {
                var paramsType = controllerType.GetConstructors()
                    .First().GetParameters()
                    .Select(p => p.ParameterType);
                var paramObjs = paramsType.Select(p => GetInstance(p)).ToArray();
                return Activator.CreateInstance(controllerType, paramObjs) as IController;
            }
        }

        private static object GetInstance(Type tipo)
        {
            if (tipo == typeof(IAutorizacaoServico))
                return Activator.CreateInstance(typeof(AutorizacaoServico), BuscaToken());
            else if (tipo == typeof(IResumoServico))
                return Activator.CreateInstance(typeof(ResumoServico), BuscaToken());
            else if (tipo == typeof(ICorridaServico))
                return Activator.CreateInstance(typeof(CorridaServico), BuscaToken());
            else if (tipo == typeof(IProfissionalServico))
                return Activator.CreateInstance(typeof(ProfissionalServico), BuscaToken());
            else if (tipo == typeof(IDiariaServico))
                return Activator.CreateInstance(typeof(DiariaServico), BuscaToken());
            else if (tipo == typeof(IClienteServico))
                return Activator.CreateInstance(typeof(ClienteServico), BuscaToken());
            else if (tipo == typeof(ITarifaServico))
                return Activator.CreateInstance(typeof(TarifaServico), BuscaToken());
            else if (tipo == typeof(IFaturamentoServico))
                return Activator.CreateInstance(typeof(FaturamentoServico), BuscaToken());
            else if (tipo == typeof(IUsuarioServico))
                return Activator.CreateInstance(typeof(UsuarioServico), BuscaToken());
            else if(tipo == typeof(IAreaAtuacaoServico))
                return Activator.CreateInstance(typeof(AreaAtuacaoServico), BuscaToken());
            else if (tipo == typeof(IOSAvulsaControle))
                return Activator.CreateInstance(typeof(OSAvulsaControle));
            else
                return Activator.CreateInstance(tipo);
        }

        private static string BuscaToken()
        {
            return UsuarioLogado.Instance.bhTkUsu?.ToString() ?? "";
        }
    }
}
