using System;
using System.Collections.Generic;

namespace Tesis2.Entities;

public partial class Semestre
{
    public int IdSemestres { get; set; }

    public virtual ICollection<Materia> Materia { get; set; } = new List<Materia>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
