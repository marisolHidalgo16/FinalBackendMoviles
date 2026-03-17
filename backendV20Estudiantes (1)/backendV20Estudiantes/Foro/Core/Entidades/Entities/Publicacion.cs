using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entidades
{
    public class Publicacion
    {
        public Guid? guid { get; set; }
        public Guid? guidTema { get; set; }
        public Guid? guidUsuario { get; set; }
        public string titulo { get; set; }
        public string mensaje { get; set; }
    }
}
