namespace TestAPI.Modelos
{
    public class AlumnosAsistencia
    {
        public int Id { get; set; }
        public string NombreAlumno { get; set; }
        public int Grupo { get; set; }
        public DateTime Fecha { get; set; }
        public bool Asistencia { get; set; }

    }
}
