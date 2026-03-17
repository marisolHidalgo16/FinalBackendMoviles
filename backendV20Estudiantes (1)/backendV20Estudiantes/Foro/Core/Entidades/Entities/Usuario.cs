using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Entidades
{
    public class Usuario
    {
        public Guid guid {  get; set; }
        public string Nombre { get; set; }
        public string apellidos { get; set; }
        public string Email { get; set; }
        public string password { get; set; }    

    }
}
