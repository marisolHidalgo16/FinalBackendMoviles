using AccesoDatos;
using Core.Entidades;
using Core.Enum;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Web;


namespace Utilitarios
{
    public static class Utilitarios
    {
        public static Error crearError(enumErrores enumError)
        {
            Error error = new Error();
            error.codigo = enumError;
            error.mensaje = enumError.ToString();

            return error;

        }


        public static void bitacorear (ReqBitacorear req)
        {
            //Validar campos
           try
            {
                using (ConexionDataContext linq = new ConexionDataContext())
                {
                    linq.SP_INSERTAR_BITACORA(req.Bitacora.clase, req.Bitacora.metodo, (short)req.Bitacora.tipo, req.Bitacora.errorId, req.Bitacora.descripcion, req.Bitacora.request, req.Bitacora.response);
                }
            }
            catch(Exception e)
            {
                //Bitacorear en un .txt
                //Enviar mail a los devs etc etc
            }
        }

        public static string crearToken()
        {
            const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Range(0, 6)
                .Select(_ => caracteres[random.Next(caracteres.Length)])
                .ToArray());
        }

        public static bool EnviarCorreoVerificacion(string nombre, string apellido, string correo, string token)
        {
            #region contraseñasSecretas
            string correoEnvio = "SuCuenta";
            string passAplicacion = "SuPasswordUltraSecreto";
            #endregion
            try
            {
                var mensaje = new MailMessage
                {
                    From = new MailAddress("SuCuenta@suApp.com", "Foro UNA"),
                    Subject = "Verifica tu cuenta",
                    IsBodyHtml = true,
                    Body = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
</head>
<body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
    <div style='max-width: 500px; margin: 0 auto; background: white; padding: 30px; border-radius: 10px; box-shadow: 0 2px 10px rgba(0,0,0,0.1);'>
        <h2 style='color: #333; text-align: center;'>¡Bienvenido/a!</h2>
        <p style='color: #555; font-size: 16px;'>Hola <strong>{nombre} {apellido}</strong>,</p>
        <p style='color: #555; font-size: 16px;'>Gracias por registrarte. Para activar tu cuenta, usa el siguiente código de verificación:</p>
        <div style='background: #007bff; color: white; padding: 15px; text-align: center; font-size: 24px; letter-spacing: 5px; border-radius: 5px; margin: 20px 0;'>
            {token}
        </div>
        <p style='color: #888; font-size: 14px; text-align: center;'>Este código expira en 24 horas.</p>
        <hr style='border: none; border-top: 1px solid #eee; margin: 20px 0;'>
        <p style='color: #aaa; font-size: 12px; text-align: center;'>Si no solicitaste esta verificación, ignora este correo.</p>
    </div>
</body>
</html>"
                };

                mensaje.To.Add(correo);

                using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential(correoEnvio, passAplicacion);
                    smtp.EnableSsl = true;
                    smtp.Send(mensaje);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
