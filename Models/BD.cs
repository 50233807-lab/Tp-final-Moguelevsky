using System;
using System.Collections.Generic;
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
