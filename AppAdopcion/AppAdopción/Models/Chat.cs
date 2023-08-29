using System;
using System.Collections.Generic;
using System.Text;

namespace AppAdopción.Models
{
    class Chat
    {
        public int ChatId { get; set; }

        public int PublicacionId { get; set; }

        public int UsuarioIniciadorId { get; set; }

        public int UsuarioPublicacionId { get; set; }

        public DateTime FechaInicio { get; set; }
    }
}
