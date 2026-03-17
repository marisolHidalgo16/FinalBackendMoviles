using AccesoDatos;
using Core.Entidades;
using Core.Entidades.Request;
using Core.Entidades.Response;
using Core.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Data.SqlClient;
using System.IO.Ports;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Logica.Usuario
{
    public class LogUsuario
    {
        public ResInsertarUsuario ingresar(ReqInsertarUsuario req)
        {
            ResInsertarUsuario res = new ResInsertarUsuario();
            res.resultado = false;
            res.error = new List<Error>();
            res.resultado = false;
            enumBitacora enumBitacora = enumBitacora.fallido;
            int errorID = 0;
            string errorDescripcion = string.Empty;
            try
            {
                if (String.IsNullOrEmpty(req.usuario.Nombre))
                {
                    res.error.Add(Utilitarios.Utilitarios.crearError(enumErrores.nombreFaltante));
                    errorID = (int)enumErrores.nombreFaltante;
                    errorDescripcion = enumErrores.nombreFaltante.ToString();
                }

                if (String.IsNullOrEmpty(req.usuario.apellidos))
                {
                    res.error.Add(Utilitarios.Utilitarios.crearError(enumErrores.apellidosFaltante));
                    errorID = (int)enumErrores.apellidosFaltante;
                    errorDescripcion = enumErrores.apellidosFaltante.ToString();

                }

                if (String.IsNullOrEmpty(req.usuario.Email))
                {
                    res.error.Add(Utilitarios.Utilitarios.crearError(enumErrores.emailFaltante));
                    errorID = (int)enumErrores.emailInvalido;
                    errorDescripcion = enumErrores.emailInvalido.ToString();

                }
                else if (!EsEmailValido(req.usuario.Email))
                {
                    res.error.Add(Utilitarios.Utilitarios.crearError(enumErrores.emailInvalido));

                }
                if (String.IsNullOrEmpty(req.usuario.password))
                {
                    res.error.Add(Utilitarios.Utilitarios.crearError(enumErrores.passwordVacio));
                    errorID = (int)enumErrores.passwordVacio;

                }

                if (res.error.Any())
                {
                    //Algun dato tiene error
                    //return res;
                }
                else
                {
                    Guid? guid = new Guid();
                    int? idReturn = 0;
                    int? errorId = 0;
                    string errorBD = "";
                    string token = Utilitarios.Utilitarios.crearToken();
                    //Todo ok, enviar a Linq
                    using (ConexionDataContext miLinq = new ConexionDataContext())
                    {
                        miLinq.SP_INGRESAR_USUARIO(req.usuario.Nombre, req.usuario.apellidos, req.usuario.Email, req.usuario.password, token, ref guid, ref idReturn, ref errorId, ref errorBD);
                    }
                    if (guid == null)
                    {
                        res.error.Add(Utilitarios.Utilitarios.crearError(enumErrores.errorBaseDatos));
                        res.resultado = false;
                    }
                    else
                    {

                        if (Utilitarios.Utilitarios.EnviarCorreoVerificacion(req.usuario.Nombre, req.usuario.apellidos, req.usuario.Email, token))
                        {
                            //Se envio correctamente
                            res.resultado = true;
                            res.error = null;

                        }
                        else
                        {
                            //No en envio... Crear SP que elimine el registro
                        }
                       
                    }
                }
            }
            catch (Exception q)
            {
                res.error.Add(Utilitarios.Utilitarios.crearError(enumErrores.errorNoControlado));
            }
            finally
            {
                ReqBitacorear reqBitacorear = new ReqBitacorear();
                Bitacora bitacora = new Bitacora();

                bitacora.clase = GetType().Name;
                bitacora.metodo = MethodBase.GetCurrentMethod().Name;
                if (res.resultado = true) { bitacora.tipo = enumBitacora.exitoso; } else { bitacora.tipo = enumBitacora.fallido; }
                bitacora.errorId = errorID;
                bitacora.descripcion = errorDescripcion;
                bitacora.request = JsonConvert.SerializeObject(req);
                bitacora.response= JsonConvert.SerializeObject(res); ;

                reqBitacorear.Bitacora = bitacora;

                Utilitarios.Utilitarios.bitacorear(reqBitacorear);

            }

            return res;

        }

        public ResObtenerUsuario obtener(ReqObtenerUsuario req)
        {
            ResObtenerUsuario res = new ResObtenerUsuario();
            res.resultado = false;
            try
            {
                if (req.guid.Equals(Guid.Empty)) {
                    res.error.Add(Utilitarios.Utilitarios.crearError(enumErrores.guidDeUsuarioFaltante));
                }

                if (res.error.Any()) {
                    //Hubo un error
                    res.resultado = false;
                }
                else
                {
                    //Validaciones correcto
                    SP_OBTENER_USUARIOResult miTipoComplejo = new SP_OBTENER_USUARIOResult();
                    using(ConexionDataContext linq = new ConexionDataContext())
                    {
                        miTipoComplejo = linq.SP_OBTENER_USUARIO(req.guid).ToList().First();
                    }

                    res.Usuario = this.factoriaUsuario(miTipoComplejo);
                }
            }
            catch (Exception e)
            {
                res.error.Add(Utilitarios.Utilitarios.crearError(enumErrores.errorNoControlado));
            }

            return res;
        }

        public ResLogin login (ReqLogin req)
        {
            ResLogin res = new ResLogin();
            res.error = new List<Error>();
            res.resultado = false;

            try
            {
                //Ustedes validan
 

                sp_LoginResult sp_LoginResult = new sp_LoginResult();
                using (ConexionDataContext conexion = new ConexionDataContext())
                {
                    sp_LoginResult = conexion.sp_Login(req.email, req.password).ToList().FirstOrDefault();
                }
                if(sp_LoginResult != null)
                {
                    res.usuario = new Core.Entidades.Usuario();
                    res.usuario.guid = sp_LoginResult.GUID_USUARIO;
                    res.usuario.Nombre = sp_LoginResult.NOMBRE;
                    res.usuario.apellidos = sp_LoginResult.APELLIDOS;
                    res.resultado = true;
                }
                else
                {
                    res.resultado = false;
                    res.error.Add(Utilitarios.Utilitarios.crearError(enumErrores.loginIncorrecto));
                }

               
            }
            catch (Exception e) { 
                res.resultado=false;
                res.error.Add(Utilitarios.Utilitarios.crearError(enumErrores.errorNoControlado));
            }

            return res;
            
        }

        public ResActivarUsuario activar(ReqActivarUsuario req)
        {
            ResActivarUsuario res = new ResActivarUsuario();
            res.error = new List<Error>();

            //Validar
            int? filas = null;
            int? idReturn = null;
            int? errorId = null;
            string errorD = null;


            using (ConexionDataContext linq  = new ConexionDataContext())
            {
                linq.SP_ACTIVAR_USUARIO(req.correo, req.token, ref filas, ref idReturn, ref errorId, ref errorD);
            }
           if (filas == 1)
            {
                res.resultado = true;
                res.error = null;
            }
            else
            {
                res.resultado = false;
                res.error.Add(Utilitarios.Utilitarios.crearError(enumErrores.errorActivandoUsuario));
            }
           return res;
        }

        //La factoriiaaaa!!
        private Core.Entidades.Usuario factoriaUsuario(SP_OBTENER_USUARIOResult elTipoComplejo)
        {
            Core.Entidades.Usuario usuario = new Core.Entidades.Usuario();
            usuario.Nombre = elTipoComplejo.NOMBRE;
            usuario.apellidos = elTipoComplejo.APELLIDOS;
            usuario.Email = elTipoComplejo.CORREO_ELECTRONICO;

            return usuario;
        }

        public bool EsEmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return email.Contains("@") &&
                   email.LastIndexOf("@") == email.IndexOf("@") &&
                   email.Contains(".") &&
                   email.IndexOf("@") < email.LastIndexOf(".");
        }
    }
}
