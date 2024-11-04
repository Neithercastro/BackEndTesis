using System;
using System.Collections.Generic;

namespace Tesis2.Entities;

public partial class Verificacion
{
    public int Idverificacion { get; set; }

    public string? Usuario { get; set; }

    public string? Permiso { get; set; }

    public string? Token { get; set; }
}
