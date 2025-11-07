using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestAPI.Modelos;
using System;

namespace TestAPI.Repositorios
{
    public class AlumnosAsistenciaRepositorio : IAlumnosAsistenciaRepositorio
    {
        private readonly string _connectionString;

        public AlumnosAsistenciaRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<AlumnosAsistencia>> ObtenerTodas()
        {
            using var conexion = new SqlConnection(_connectionString);
            // NOTA: Este SQL puede fallar ahora si 'Grupo' ya no existe.
            // Deberías hacer un JOIN con la tabla Alumnos si necesitas mostrar todo.
            var sql = "SELECT * FROM AlumnosAsistencia";
            return await conexion.QueryAsync<AlumnosAsistencia>(sql);
        }

        // MÉTODO 'Crear' REEMPLAZADO POR 'GuardarAsistencia' (UPSERT)
        public async Task<bool> GuardarAsistencia(AlumnosAsistencia asistencia)
        {
            using var conexion = new SqlConnection(_connectionString);

            // Esta es la lógica "UPSERT"
            var sql = @"
                -- 1. Intenta ACTUALIZAR si ya existe un registro
                UPDATE AlumnosAsistencia
                SET 
                    Asistencia = @Asistencia
                WHERE 
                    IdAlumno = @IdAlumno AND CONVERT(date, Fecha) = CONVERT(date, @Fecha);

                -- 2. Si no se afectó ninguna fila (@@ROWCOUNT = 0),
                --    significa que no existía, entonces lo INSERTAMOS.
                IF (@@ROWCOUNT = 0)
                BEGIN
                    INSERT INTO AlumnosAsistencia (IdAlumno, Fecha, Asistencia)
                    VALUES (@IdAlumno, @Fecha, @Asistencia);
                END
            ";

            var filasAfectadas = await conexion.ExecuteAsync(sql, new
            {
                asistencia.IdAlumno,
                asistencia.Fecha,
                asistencia.Asistencia
            });

            return filasAfectadas > 0;
        }

        public async Task<bool> EliminarAlumnosAsistencia(int id)
        {
            using var conexion = new SqlConnection(_connectionString);
            var sql = "DELETE FROM AlumnosAsistencia WHERE Id = @Id";
            var filas = await conexion.ExecuteAsync(sql, new { Id = id });
            return filas > 0;
        }

        public async Task<IEnumerable<AsistenciaReporte>> ObtenerPorFecha(DateTime fecha)
        {
            using var conexion = new SqlConnection(_connectionString);

            // Este SQL une las dos tablas para obtener el nombre del alumno
            var sql = @"
                SELECT 
                    a.Id,
                    a.IdAlumno,
                    al.NombreAlumno,
                    a.Fecha,
                    a.Asistencia
                FROM AlumnosAsistencia a
                INNER JOIN Alumnos al ON a.IdAlumno = al.Id
                WHERE CONVERT(date, a.Fecha) = CONVERT(date, @Fecha);
            ";

            return await conexion.QueryAsync<AsistenciaReporte>(sql, new { Fecha = fecha });
        }
    }
}