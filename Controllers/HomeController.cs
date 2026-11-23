using System;
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

    public IActionResult Detalle(int id)
    {
        string idUsuario = HttpContext.Session.GetString("IdUsuario");
        if (string.IsNullOrEmpty(idUsuario))
        {
            return RedirectToAction("Login", "Account");
        }

        Equipo equipo = BD.ObtenerEquipoPorId(id);
        if (equipo == null)
        {
            return RedirectToAction("Index");
        }

        ViewBag.Equipo = equipo;
        return View();
    }

    public IActionResult CalcularAlquiler(int idEquipo, DateTime fechaInicio, DateTime fechaFin)
    {
        string idUsuarioStr = HttpContext.Session.GetString("IdUsuario");
        if (string.IsNullOrEmpty(idUsuarioStr))
        {
            return Unauthorized();
        }

        int idUsuario = int.Parse(idUsuarioStr);
        Equipo equipo = BD.ObtenerEquipoPorId(idEquipo);
        if (equipo == null)
        {
            return Json(new { valido = false, mensaje = "Equipo no encontrado." });
        }

        string error = GestorAlquileres.ValidarReserva(idUsuario, idEquipo, fechaInicio, fechaFin);
        if (error != "")
        {
            return Json(new { valido = false, mensaje = error });
        }

        int dias = (fechaFin - fechaInicio).Days + 1;
        decimal total = GestorAlquileres.CalcularTotal(equipo.PrecioPorDia, fechaInicio, fechaFin);

        return Json(new { valido = true, dias = dias, total = total });
    }

    [HttpPost]
    public IActionResult ConfirmarAlquiler(int idEquipo, DateTime fechaInicio, DateTime fechaFin)
    {
        string idUsuarioStr = HttpContext.Session.GetString("IdUsuario");
        if (string.IsNullOrEmpty(idUsuarioStr))
        {
            return RedirectToAction("Login", "Account");
        }

        int idUsuario = int.Parse(idUsuarioStr);
        Equipo equipo = BD.ObtenerEquipoPorId(idEquipo);
        if (equipo == null)
        {
            return RedirectToAction("Index");
        }

        string error = GestorAlquileres.ValidarReserva(idUsuario, idEquipo, fechaInicio, fechaFin);
        if (error != "")
        {
            ViewBag.Equipo = equipo;
            ViewBag.Error = error;
            return View("Detalle");
        }

        Alquiler nuevoAlquiler = new Alquiler();
        nuevoAlquiler.IdUsuario = idUsuario;
        nuevoAlquiler.IdEquipo = idEquipo;
        nuevoAlquiler.FechaInicio = fechaInicio;
        nuevoAlquiler.FechaFin = fechaFin;
        nuevoAlquiler.Total = GestorAlquileres.CalcularTotal(equipo.PrecioPorDia, fechaInicio, fechaFin);

        BD.CrearAlquiler(nuevoAlquiler);

        return RedirectToAction("MisAlquileres");
    }

    public IActionResult MisAlquileres()
    {
        string idUsuarioStr = HttpContext.Session.GetString("IdUsuario");
        if (string.IsNullOrEmpty(idUsuarioStr))
        {
            return RedirectToAction("Login", "Account");
        }

        int idUsuario = int.Parse(idUsuarioStr);
        List<Alquiler> alquileres = BD.ObtenerAlquileresPorUsuario(idUsuario);

        ViewBag.Alquileres = alquileres;
        return View();
    }

    [HttpPost]
    public IActionResult CancelarAlquiler(int idAlquiler)
    {
        string idUsuarioStr = HttpContext.Session.GetString("IdUsuario");
        if (string.IsNullOrEmpty(idUsuarioStr))
        {
            return RedirectToAction("Login", "Account");
        }

        int idUsuario = int.Parse(idUsuarioStr);
        Alquiler alquiler = BD.ObtenerAlquilerPorId(idAlquiler);

        if (alquiler != null && alquiler.IdUsuario == idUsuario && !alquiler.Cancelado && alquiler.FechaInicio >= DateTime.Today)
        {
            BD.CancelarAlquiler(idAlquiler, idUsuario);
        }

        return RedirectToAction("MisAlquileres");
    }
}
