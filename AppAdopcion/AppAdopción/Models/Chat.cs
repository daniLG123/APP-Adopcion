using System;
using System.Collections.Generic;
using System.Text;

//Sin terminar de implementar

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
