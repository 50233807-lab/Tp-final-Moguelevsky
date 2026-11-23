using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;

namespace TP_FINAL_MOGUELEVSKY.Models;

public static class BD
{
    private static string _connectionString = @"Server=localhost\SQLEXPRESS;DataBase=RentaYa;Integrated Security=True;TrustServerCertificate=True;";

    public static Usuario Login(string nombreUsuario, string contrasena)
    {
        Usuario usuarioEncontrado = null;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Usuarios WHERE NombreUsuario = @pNombreUsuario AND Contrasena = @pContrasena";
            usuarioEncontrado = connection.QueryFirstOrDefault<Usuario>(query, new { pNombreUsuario = nombreUsuario, pContrasena = contrasena });
        }
        return usuarioEncontrado;
    }

    public static Usuario BuscarUsuarioPorNombre(string nombreUsuario)
    {
        Usuario usuarioEncontrado = null;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Usuarios WHERE NombreUsuario = @pNombreUsuario";
            usuarioEncontrado = connection.QueryFirstOrDefault<Usuario>(query, new { pNombreUsuario = nombreUsuario });
        }
        return usuarioEncontrado;
    }

    public static void RegistrarUsuario(Usuario nuevoUsuario)
    {
        string query = "INSERT INTO Usuarios (NombreUsuario, Contrasena, Nombre, Apellido) VALUES (@pNombreUsuario, @pContrasena, @pNombre, @pApellido)";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new
            {
                pNombreUsuario = nuevoUsuario.NombreUsuario,
                pContrasena = nuevoUsuario.Contrasena,
                pNombre = nuevoUsuario.Nombre,
                pApellido = nuevoUsuario.Apellido
            });
        }
    }
        public static List<Equipo> ObtenerEquiposPaginados(int cantidadSaltar, int cantidadTraer)
    {
        List<Equipo> listaEquipos = new List<Equipo>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = @"SELECT E.Id, E.Nombre, E.Descripcion, E.Imagen, E.PrecioPorDia, E.IdCategoria, E.Activo, C.Nombre AS NombreCategoria
                             FROM Equipos E
                             INNER JOIN Categorias C ON E.IdCategoria = C.Id
                             WHERE E.Activo = 1
                             ORDER BY E.Id
                             OFFSET @pSaltar ROWS FETCH NEXT @pTraer ROWS ONLY";
            listaEquipos = connection.Query<Equipo>(query, new { pSaltar = cantidadSaltar, pTraer = cantidadTraer }).ToList();
        }
        return listaEquipos;
    }

    public static int ContarEquiposActivos()
    {
        int total = 0;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT COUNT(*) FROM Equipos WHERE Activo = 1";
            total = connection.QueryFirstOrDefault<int>(query);
        }
        return total;
    }
        public static Equipo ObtenerEquipoPorId(int idEquipo)
    {
        Equipo equipo = null;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = @"SELECT E.Id, E.Nombre, E.Descripcion, E.Imagen, E.PrecioPorDia, E.IdCategoria, E.Activo, C.Nombre AS NombreCategoria
                             FROM Equipos E
                             INNER JOIN Categorias C ON E.IdCategoria = C.Id
                             WHERE E.Id = @pId AND E.Activo = 1";
            equipo = connection.QueryFirstOrDefault<Equipo>(query, new { pId = idEquipo });
        }
        return equipo;
    }

    public static int ContarAlquileresSuperpuestos(int idEquipo, DateTime fechaInicio, DateTime fechaFin)
    {
        int cantidad = 0;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = @"SELECT COUNT(*) FROM Alquileres
                             WHERE IdEquipo = @pIdEquipo
                             AND Cancelado = 0
                             AND FechaInicio <= @pFechaFin
                             AND FechaFin >= @pFechaInicio";
            cantidad = connection.QueryFirstOrDefault<int>(query, new { pIdEquipo = idEquipo, pFechaInicio = fechaInicio, pFechaFin = fechaFin });
        }
        return cantidad;
    }

    public static int ContarCancelacionesMesActual(int idUsuario)
    {
        int cantidad = 0;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = @"SELECT COUNT(*) FROM Alquileres
                             WHERE IdUsuario = @pIdUsuario
                             AND Cancelado = 1
                             AND MONTH(FechaInicio) = MONTH(GETDATE())
                             AND YEAR(FechaInicio) = YEAR(GETDATE())";
            cantidad = connection.QueryFirstOrDefault<int>(query, new { pIdUsuario = idUsuario });
        }
        return cantidad;
    }

    public static void CrearAlquiler(Alquiler nuevoAlquiler)
    {
        string query = @"INSERT INTO Alquileres (IdUsuario, IdEquipo, FechaInicio, FechaFin, Total, Cancelado)
                         VALUES (@pIdUsuario, @pIdEquipo, @pFechaInicio, @pFechaFin, @pTotal, 0)";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new
            {
                pIdUsuario = nuevoAlquiler.IdUsuario,
                pIdEquipo = nuevoAlquiler.IdEquipo,
                pFechaInicio = nuevoAlquiler.FechaInicio,
                pFechaFin = nuevoAlquiler.FechaFin,
                pTotal = nuevoAlquiler.Total
            });
        }
    }

    public static List<Alquiler> ObtenerAlquileresPorUsuario(int idUsuario)
    {
        List<Alquiler> lista = new List<Alquiler>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            lista = connection.Query<Alquiler>(
                "sp_ObtenerAlquileresPorUsuario",
                new { pIdUsuario = idUsuario },
                commandType: CommandType.StoredProcedure
            ).ToList();
        }
        return lista;
    }

    public static Alquiler ObtenerAlquilerPorId(int idAlquiler)
    {
        Alquiler alquiler = null;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Alquileres WHERE Id = @pId";
            alquiler = connection.QueryFirstOrDefault<Alquiler>(query, new { pId = idAlquiler });
        }
        return alquiler;
    }

    public static void CancelarAlquiler(int idAlquiler, int idUsuario)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(
                "sp_CancelarAlquiler",
                new { pIdAlquiler = idAlquiler, pIdUsuario = idUsuario },
                commandType: CommandType.StoredProcedure
            );
        }
    }


}
