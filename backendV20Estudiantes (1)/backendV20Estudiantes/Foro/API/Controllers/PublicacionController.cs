using Core.Entidades;
using Logica.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class PublicacionController : ApiController
    {
        [HttpPost]
        [Route("api/publicacion/obtener")]
        public ResObtenerPublicaciones obtener (ReqObtenerPublicaciones req)
        {
            return new LogPublicacion().obtener(req);
        }
    }
}
