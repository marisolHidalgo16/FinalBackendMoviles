using AccesoDatos;
using Core.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Usuario
{
    public class LogPublicacion
    {
        public ResObtenerPublicaciones obtener (ReqObtenerPublicaciones req)
        {
            ResObtenerPublicaciones res = new ResObtenerPublicaciones();
            res.publicaciones = new List<Publicacion>();
            res.resultado = false;
            try
            {
                List<SP_OBTENER_PUBLICACIONESResult> sP_OBTENER_PUBLICACIONESResult = new List<SP_OBTENER_PUBLICACIONESResult>();
                using (ConexionDataContext linq = new ConexionDataContext())
                {
                   res.publicaciones = this.factoriaPublicaciones (linq.SP_OBTENER_PUBLICACIONES().ToList());
                }

            }
            catch (Exception ex) { 
                res.resultado=false;
            }
            return res;
        }

        private List<Publicacion> factoriaPublicaciones(List<SP_OBTENER_PUBLICACIONESResult> listaTC)
        {
            List<Publicacion> listaRetornar = new List<Publicacion>();
            foreach (SP_OBTENER_PUBLICACIONESResult cadaPublicacion in listaTC)
            {
                Publicacion publicacion = new Publicacion();
                publicacion.guid = cadaPublicacion.GUID_PUBLICACION;
                publicacion.guidTema = cadaPublicacion.GUID_TEMA;
                publicacion.guidUsuario = cadaPublicacion.GUID_USUARIO;
                publicacion.titulo = cadaPublicacion.TITULO;
                publicacion.mensaje = cadaPublicacion.MENSAJE;

                listaRetornar.Add(publicacion);
            }

            return listaRetornar;
        }
    }
}
