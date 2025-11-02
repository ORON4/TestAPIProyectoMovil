namespace TestAPI.Modelos
{
    public class Respuesta
    {
        public object datos { get; set; }
        public int total_informacion { get; set; }
        public string mensaje { get; set; }

        public Respuesta(object datos = null, int totalInformacion = 0, string mensaje = "")
        {
            this.datos = datos;
            this.total_informacion = totalInformacion;
            this.mensaje = mensaje;
        }
    }
}
