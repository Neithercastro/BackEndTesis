using System;
using System.Collections.Generic;

namespace Tesis2.Entities;

public partial class Estilosaprendizaje
{
    public int IdestilosAprendizaje { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<Contenidodetalle> Contenidodetalles { get; set; } //= new List<Contenidodetalle>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
