using TestAPI.Modelos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestAPI.Repositorios
{
   
    public interface IAlumnosRepositorio
    {
        Task<IEnumerable<Alumno>> ObtenerPorGrupo(int idGrupo);
    }
    
}
