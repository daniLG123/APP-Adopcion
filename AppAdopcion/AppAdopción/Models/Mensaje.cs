﻿using System;
using System.Collections.Generic;
using System.Text;

//Sin terminar de implementar

namespace AppAdopción.Models
{
    class Mensaje
    {
        public int MensajeId { get; set; }

        public int UsuarioEmisorId { get; set; }

        public int ChatId { get; set; }

        public string NombreEmisor { get; set; }

        public string Contenido { get; set; }

        public DateTime FechaEnvio { get; set; }

        public int UsuarioReceptorId { get; set; }
    }
}
