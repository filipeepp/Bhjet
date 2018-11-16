using BHJet_Servico.Autorizacao;
using BHJet_Servico.Corrida;
using BHJet_Servico.Dashboard;
using BHJet_Servico.Profissional;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

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
                return Activator.CreateInstance(typeof(AutorizacaoServico));
            else if (tipo == typeof(IResumoServico))
                return Activator.CreateInstance(typeof(ResumoServico));
            else if (tipo == typeof(ICorridaServico))
                return Activator.CreateInstance(typeof(CorridaServico));
            else if (tipo == typeof(IProfissionalServico))
                return Activator.CreateInstance(typeof(ProfissionalServico));
            else
                return Activator.CreateInstance(tipo);
        }
    }
}