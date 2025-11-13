namespace TestAPI.Modelos
{
    public class TareaAlumnoDTO
    {

        public int Id { get; set; } // El ID de la Tarea (Definición global)
        public int AlumnoTareaId { get; set; } // El ID de la relación (Para borrar/entregar)
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaEntrega { get; set; }
        public bool Estatus { get; set; } // El estatus específico del alumno
    
    }
}
