using System;
using System.Collections.Generic;

namespace Tesis2.Entities;

public partial class Materiadetalle
{
    public int IdMateriaDetalle { get; set; }
    public string? NombreActividad { get; set; }
    public int? IdMateria { get; set; }

    

    public virtual Materia? IdMateriaNavigation { get; set; }

    public virtual ICollection<Contenidodetalle> Contenidodetalles { get; set; } 
}
