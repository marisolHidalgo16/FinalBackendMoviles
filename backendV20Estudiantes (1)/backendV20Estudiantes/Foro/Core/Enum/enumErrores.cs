using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum
{
    public enum enumErrores
    {
        errorNoControlado = -2,
        errorBaseDatos = -1,
        nombreFaltante = 1,
        apellidosFaltante = 2,
        emailFaltante = 3,
        emailInvalido = 4,
        passwordVacio = 5,
        guidDeUsuarioFaltante = 6,
        errorActivandoUsuario = 7,
        loginIncorrecto = 8,
    }
}
