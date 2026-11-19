using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TP_FINAL_MOGUELEVSKY.Models;

namespace TP_FINAL_MOGUELEVSKY.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        string idUsuario = HttpContext.Session.GetString("IdUsuario");
        if (string.IsNullOrEmpty(idUsuario))
        {
            return RedirectToAction("Login", "Account");
        }

        List<Equipo> primerosEquipos = BD.ObtenerEquiposPaginados(0, 8);
        int totalEquipos = BD.ContarEquiposActivos();

        ViewBag.Equipos = primerosEquipos;
        ViewBag.HayMas = totalEquipos > 8;

        return View();
    }

    public IActionResult ObtenerMasEquipos(int saltar)
    {
        string idUsuario = HttpContext.Session.GetString("IdUsuario");
        if (string.IsNullOrEmpty(idUsuario))
        {
            return Unauthorized();
        }

        List<Equipo> siguientesEquipos = BD.ObtenerEquiposPaginados(saltar, 8);
        int totalEquipos = BD.ContarEquiposActivos();
        bool hayMas = (saltar + siguientesEquipos.Count) < totalEquipos;

        return Json(new { equipos = siguientesEquipos, hayMas = hayMas });
    }
}
