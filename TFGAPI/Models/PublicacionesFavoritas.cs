using System;
using System.Collections.Generic;

namespace TFGAPI.Models;

public partial class PublicacionesFavoritas
{
    public int Id { get; set; }

    public int? UsuarioId { get; set; }

    public int? PublicacionId { get; set; }
}
