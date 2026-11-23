<<<<<<< HEAD
=======
using System;
using System.Collections.Generic;
>>>>>>> 0b635b3ae5ccb8a21210f8aa1ba8417681a0c172
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;

namespace TP_FINAL_MOGUELEVSKY.Models;

public static class BD
{
    private static string _connectionString = @"Server=localhost;DataBase=RentaYa;Integrated Security=True;TrustServerCertificate=True;";

    public static Usuario Login(string nombreUsuario, string contrasena)
    {
        Usuario usuarioEncontrado = null;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            usuarioEncontrado = connection.QueryFirstOrDefault<Usuario>(
                "sp_Login",
                new { NombreUsuario = nombreUsuario, Contrasena = contrasena },
                commandType: CommandType.StoredProcedure
            );
        }
        return usuarioEncontrado;
    }

    public static Usuario BuscarUsuarioPorNombre(string nombreUsuario)
    {
        Usuario usuarioEncontrado = null;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            usuarioEncontrado = connection.QueryFirstOrDefault<Usuario>(
                "sp_BuscarUsuarioPorNombre",
                new { NombreUsuario = nombreUsuario },
                commandType: CommandType.StoredProcedure
            );
        }
        return usuarioEncontrado;
    }

    public static void RegistrarUsuario(Usuario nuevoUsuario)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(
                "sp_RegistrarUsuario",
                new
                {
                    NombreUsuario = nuevoUsuario.NombreUsuario,
                    Contrasena = nuevoUsuario.Contrasena,
                    Nombre = nuevoUsuario.Nombre,
                    Apellido = nuevoUsuario.Apellido
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }

    public static IEnumerable<Equipo> ObtenerEquiposPaginados(int cantidadSaltar, int cantidadTraer)
    {
        IEnumerable<Equipo> listaEquipos = null;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            listaEquipos = connection.Query<Equipo>(
                "sp_ObtenerEquiposPaginados",
                new { Saltar = cantidadSaltar, Traer = cantidadTraer },
                commandType: CommandType.StoredProcedure
            );
        }
        return listaEquipos;
    }

    public static int ContarEquiposActivos()
    {
        int total = 0;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            total = connection.QueryFirstOrDefault<int>(
                "sp_ContarEquiposActivos",
                commandType: CommandType.StoredProcedure
            );
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


    public static Equipo ObtenerEquipoPorId(int idEquipo)
    {
        Equipo equipoEncontrado = null;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            equipoEncontrado = connection.QueryFirstOrDefault<Equipo>(
                "sp_ObtenerEquipoPorId",
                new { Id = idEquipo },
                commandType: CommandType.StoredProcedure
            );
        }
        return equipoEncontrado;
    }

    public static Alquiler BuscarAlquilerSuperpuesto(int idEquipo, DateTime fechaRetiro, DateTime fechaDevolucion)
    {
        Alquiler alquilerEncontrado = null;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            alquilerEncontrado = connection.QueryFirstOrDefault<Alquiler>(
                "sp_BuscarAlquilerSuperpuesto",
                new { IdEquipo = idEquipo, FechaRetiro = fechaRetiro, FechaDevolucion = fechaDevolucion },
                commandType: CommandType.StoredProcedure
            );
        }
        return alquilerEncontrado;
    }

    public static int ContarCancelacionesMesActual(int idUsuario, int mes, int anio)
    {
        int cantidadCancelaciones = 0;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            cantidadCancelaciones = connection.QueryFirstOrDefault<int>(
                "sp_ContarCancelacionesMesActual",
                new { IdUsuario = idUsuario, Mes = mes, Anio = anio },
                commandType: CommandType.StoredProcedure
            );
        }
        return cantidadCancelaciones;
    }

    public static void InsertarAlquiler(Alquiler nuevoAlquiler)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(
                "sp_InsertarAlquiler",
                new
                {
                    IdEquipo = nuevoAlquiler.IdEquipo,
                    IdUsuario = nuevoAlquiler.IdUsuario,
                    FechaRetiro = nuevoAlquiler.FechaRetiro,
                    FechaDevolucion = nuevoAlquiler.FechaDevolucion,
                    Total = nuevoAlquiler.Total
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
