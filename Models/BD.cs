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

}
