using Core.Entidades;
using Core.Entidades.Response;
using DTO.Usuario;
using Logica.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace API.Controllers
{
    public class UsuarioController : ApiController
    {
        [HttpPost]
        [Route("api/usuario/insertar")]
        public ResInsertarUsuario insertar(DTOUsuario dtoUsuario)
        {
            ReqInsertarUsuario req = new ReqInsertarUsuario();
            req.usuario = new Usuario();
            req.usuario.Nombre = dtoUsuario.Nombre;
            req.usuario.apellidos = dtoUsuario.apellidos;
            req.usuario.Email = dtoUsuario.Email;
            req.usuario.password = dtoUsuario.password;

            return new Logica.Usuario.LogUsuario().ingresar(req);
        }

        [HttpPost]
        [Route("api/usuario/activar")]
        public ResActivarUsuario activar(DTOActivarUsuario dtoActivar)
        {
            ReqActivarUsuario req = new ReqActivarUsuario();
            req.correo = dtoActivar.correo;
            req.token = dtoActivar.token;

            return new Logica.Usuario.LogUsuario().activar(req);

        }

        [HttpPost]
        [Route("api/usuario/login")]
        public ResLogin login(DTOLogin dtoLogin)
        {
            ReqLogin req = new ReqLogin();
            req.email = dtoLogin.correo;
            req.password = dtoLogin.pass;

            return new Logica.Usuario.LogUsuario().login(req);

        }

    }
}
