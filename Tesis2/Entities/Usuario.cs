using System;
using System.Collections.Generic;

namespace Tesis2.Entities;

public partial class Usuario
{
    public int Idusuarios { get; set; }

    public string Usuario1 { get; set; } = null!;

    public string? Contraseña { get; set; }

    public string? CorreoElectronico { get; set; }

    public string? Nombre { get; set; }

    public int? Idestilos { get; set; }

    public int? Semestres { get; set; }

    public virtual Estilosaprendizaje? IdestilosNavigation { get; set; }

    public virtual Semestre? SemestresNavigation { get; set; }
}
