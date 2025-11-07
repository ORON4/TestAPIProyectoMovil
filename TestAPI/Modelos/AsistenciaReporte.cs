namespace TestAPI.Modelos
{
    public class AsistenciaReporte
    {
        public int Id { get; set; } 
        public int IdAlumno { get; set; }
        public string NombreAlumno { get; set; }
        public DateTime Fecha { get; set; }
        public bool Asistencia { get; set; }
    }
}
