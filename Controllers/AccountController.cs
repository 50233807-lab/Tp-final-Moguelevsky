using System;
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
            ViewBag.Error = "Completá usuario y contraseña.";
            return View();
        }

        Usuario usuario = BD.Login(nombreUsuario, contrasena);
        if (usuario == null)
        {
            ViewBag.Error = "Usuario o contraseña incorrectos.";
            return View();
        }

        HttpContext.Session.SetString("IdUsuario", usuario.Id.ToString());
        HttpContext.Session.SetString("NombreUsuario", usuario.Nombre);

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Registro()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Registro(string nombreUsuario, string contrasena, string nombre, string apellido)
    {
        if (string.IsNullOrEmpty(nombreUsuario) || string.IsNullOrEmpty(contrasena) || string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido))
        {
            ViewBag.Error = "Todos los campos son obligatorios.";
            return View();
        }

        if (contrasena.Length < 8)
        {
            ViewBag.Error = "La contraseña debe tener al menos 8 caracteres.";
            return View();
        }

        string caracteresEspeciales = "!@#$%^&*()_+-=[]{};':\"\\|,.<>/?";
        bool tieneEspecial = false;

        for (int i = 0; i < contrasena.Length; i++)
        {
            if (caracteresEspeciales.Contains(contrasena[i]))
            {
                tieneEspecial = true;
                break;
            }
        }

        if (!tieneEspecial)
        {
            ViewBag.Error = "La contraseña debe incluir al menos un carácter especial.";
            return View();
        }

        if (BD.ExisteUsuario(nombreUsuario))
        {
            ViewBag.Error = "Ese nombre de usuario ya está en uso.";
            return View();
        }

        Usuario nuevoUsuario = new Usuario();
        nuevoUsuario.NombreUsuario = nombreUsuario;
        nuevoUsuario.Contrasena = contrasena;
        nuevoUsuario.Nombre = nombre;
        nuevoUsuario.Apellido = apellido;

        BD.RegistrarUsuario(nuevoUsuario);

        return RedirectToAction("Login");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
