using BHJet_Enumeradores;
using BHJet_Servico.Dashboard;
using BHJet_Usuario.Models.Entregas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BHJet_Usuario.Controllers
{
    public class EntregasController : Controller
    {

		public EntregasController()
		{ }

		// GET: Entregas
		public ActionResult Index()
		{
			return View();
		}
		
	}
}