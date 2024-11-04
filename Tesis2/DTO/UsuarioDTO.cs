namespace Tesis2.DTO
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public int? idestilos { get; set; }
        public string usuario { get; set; }
        public string correo { get; set;}
        public string contrasena { get; set;}
        public string Nombre { get; set;}
        public string? NombreEstilo { get; set; }

        public int? semestre { get; set; }


    }
}
