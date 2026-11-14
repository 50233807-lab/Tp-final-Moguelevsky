using Microsoft.AspNetCore.Mvc;
using TP_FINAL_MOGUELEVSKY.Models;

namespace TP_FINAL_MOGUELEVSKY.Controllers;

public class AccountController : Controller
{
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string nombreUsuario, string contrasena)
    {
        if (string.IsNullOrEmpty(nombreUsuario) || string.IsNullOrEmpty(contrasena))
        {
            ViewBag.Error = "Debés completar usuario y contraseña.";
            return View();
        }

        Usuario user = BD.Login(nombreUsuario, contrasena);

        if (user == null)
        {
            ViewBag.Error = "Usuario o contraseña incorrectos.";
            return View();
        }

        HttpContext.Session.SetString("IdUsuario", user.Id.ToString());
        HttpContext.Session.SetString("NombreUsuario", user.NombreUsuario);

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Registro()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Registro(Usuario nuevoUsuario)
    {
        if (string.IsNullOrEmpty(nuevoUsuario.NombreUsuario) ||
            string.IsNullOrEmpty(nuevoUsuario.Contrasena) ||
            string.IsNullOrEmpty(nuevoUsuario.Nombre) ||
            string.IsNullOrEmpty(nuevoUsuario.Apellido))
        {
            ViewBag.Error = "Todos los campos son obligatorios.";
            return View();
        }

        Usuario usuarioExistente = BD.BuscarUsuarioPorNombre(nuevoUsuario.NombreUsuario);
        if (usuarioExistente != null)
        {
            ViewBag.Error = "Ese nombre de usuario ya está en uso.";
            return View();
        }

        BD.RegistrarUsuario(nuevoUsuario);
        return RedirectToAction("Login");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
