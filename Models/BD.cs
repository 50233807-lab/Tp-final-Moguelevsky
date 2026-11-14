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
}
