namespace TestAPI.Modelos
{
    public class AlumnosAsistencia
    {
        public int Id { get; set; }
        public int IdAlumno { get; set; }
        public DateTime Fecha { get; set; }
        public bool Asistencia { get; set; }

    }
}
