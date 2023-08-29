﻿using System;
using System.Collections.Generic;

namespace TFGAPI.Models;

public partial class Publicacion
{
    public int PublicacionId { get; set; }

    public int? UsuarioId { get; set; }

    public string? Descripcion { get; set; }

    public DateTime? FechaPublicacion { get; set; }

    public int? NumInteresados { get; set; }

    public string? Ciudad { get; set; }

    public string? NombreAnimal { get; set; }

    public string? EspecieAnimal { get; set; }

    public int? EdadAnimal { get; set; }

    public string? TamanyoAnimal { get; set; }

    public string? SexoAnimal { get; set; }

    public byte[]? FotoAnimal { get; set; }
}
