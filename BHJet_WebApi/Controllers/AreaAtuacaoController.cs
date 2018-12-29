using BHJet_DTO.Area;
using BHJet_Repositorio.Admin;
using BHJet_Repositorio.Admin.Entidade;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace BHJet_WebApi.Controllers
{
    [RoutePrefix("api/AreaAtuacao")]
    public class AreaAtuacaoController : ApiController
    {
        /// <summary>
		/// Busca Areas de atuação
		/// </summary>
		/// <returns></returns>
		[Authorize]
        [Route("")]
        [ResponseType(typeof(AreaAtuacaoDTO[]))]
        public IHttpActionResult GetAreaAtuacao()
        {
            // Busca Dados detalhados da corrida/OS
            var entidade = new AreaAtuacaoRepositorio().BuscaAreaAtuacaoAtiva();

            // Validacao
            if (entidade == null || !entidade.Any())
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            
            var resultado = entidade.Select(at => new AreaAtuacaoDTO()
            {
                ID = at.idRegistro,
                GeoVertices = at.vcGeoVertices.Split(';').Select(gc => new AreaAtuacaoGeoPosicao()
                {
                    Latitude = gc.Split(',')[0],
                    Longitude = gc.Split(',')[1]
                }).ToArray(),
                Ativo = at.bitDisponivel
            });

            // Return
            return Ok(resultado);
        }

        /// <summary>
        /// Grava Areas de atuação
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("")]
        public IHttpActionResult PostAreaAtuacao([FromBody]AreasFiltroDTO[] filtro)
        {
            // Filtro
            var entidadeFiltro = filtro.Select(x => new AreaAtuacaoEntidade()
            {
                 vcGeoVertices = string.Join(";", x.Area.Select(gv => $"{gv.Latitude},{gv.Longitude}"))
            }).ToArray();

            // Busca Dados detalhados da corrida/OS
             new AreaAtuacaoRepositorio().AtualizaAreaAtuacaoAtiva(entidadeFiltro);

            // Return
            return Ok();
        }


    }
}




public class Rootobject
{
    public Class1[] Property1 { get; set; }
}

public class Class1
{
    public Area[] Area { get; set; }
}

public class Area
{
    public string Latitude { get; set; }
    public string Longitude { get; set; }
}
