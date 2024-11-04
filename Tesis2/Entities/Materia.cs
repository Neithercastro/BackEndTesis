using System;
using System.Collections.Generic;

namespace Tesis2.Entities;

public partial class Materia
{
    public int IdMaterias { get; set; }

    public string? Nombre { get; set; }

    public int? Semestre { get; set; }

    public string? DireccionImagen { get; set; }

    public virtual ICollection<Contenidodetalle> Contenidodetalles { get; set; }// = new List<Contenidodetalle>();

    public virtual ICollection<Materiadetalle> Materiadetalles { get; set; } = new List<Materiadetalle>();

    public virtual Semestre? SemestreNavigation { get; set; }
}
