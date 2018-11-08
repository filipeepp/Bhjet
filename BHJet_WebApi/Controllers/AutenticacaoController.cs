using System.Web.Http;

namespace BHJet_WebApi.Controllers
{
    [RoutePrefix("api/Autenticacao")]
    public class AutenticacaoController : ApiController
    {
        [Authorize]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok();
        }
    }
}