using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BHJet_Core.Customizavel
{
	public static class MetodoPagina
	{
		public static bool PaginaEdicao()
		{
			if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["edicao"]))
				return HttpContext.Current.Request.QueryString["edicao"].ToLower().Trim() == "true".ToLower().Trim() ? true : false;
			else
				return false;
		}

		public static string GetUrlParameter(HttpRequestBase request, string parName)
		{
			string result = string.Empty;

			var urlParameters = HttpUtility.ParseQueryString(request.Url.Query);
			if (urlParameters.AllKeys.Contains(parName))
			{
				result = urlParameters.Get(parName);
			}

			return result;
		}
	}
}
