using System;
using System.Collections.Generic;
using System.Text;

namespace AppAdopción.Models
{
    public class Usuario
    {
        public int UsuarioId { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Telefono { get; set; }

        public string EsProtectora { get; set; }
        public string Ciudad { get; set; }
    }
}
