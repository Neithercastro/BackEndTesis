using System;
using System.Collections.Generic;

namespace Tesis2.Entities;

public partial class Contenidodetalle
{
    public int IdContenido { get; set; }
    public int? IdMateria { get; set; }
    public int? IdActividad { get; set; }
    public int? IdEstiloAprendizaje { get; set; }
    public string? Descripcion { get; set; }
    public string? MaterialApoyo1 { get; set; }
    public string? MaterialApoyo2 { get; set; }

    public virtual Estilosaprendizaje? IdEstiloAprendizajeNavigation { get; set; }
    public virtual Materia? IdMateriaNavigation { get; set; }
    public virtual Materiadetalle? IdMateriaDetalleNavigation { get; set; }
}
