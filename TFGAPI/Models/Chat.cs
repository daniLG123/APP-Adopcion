using System;
using System.Collections.Generic;

namespace TFGAPI.Models;

public partial class Chat
{
    public int ChatId { get; set; }

    public int? PublicacionId { get; set; }

    public int? UsuarioIniciadorId { get; set; }

    public int? UsuarioPublicacionId { get; set; }

    public DateTime? FechaInicio { get; set; }
}
