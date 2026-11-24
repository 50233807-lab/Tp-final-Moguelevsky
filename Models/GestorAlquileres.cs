using System;

namespace TP_FINAL_MOGUELEVSKY.Models;

public static class GestorAlquileres
{
    public static string ValidarReserva(int idUsuario, int idEquipo, DateTime fechaInicio, DateTime fechaFin)
    {
        DateTime hoy = DateTime.Today;
        DateTime limite14Dias = hoy.AddDays(14);

        if (fechaInicio < hoy)
        {
            return "No podés alquilar en una fecha anterior a hoy.";
        }

        if (fechaFin < fechaInicio)
        {
            return "La fecha de fin no puede ser anterior a la fecha de inicio.";
        }

        if (fechaInicio > limite14Dias || fechaFin > limite14Dias)
        {
            return "Solo se puede alquilar un equipo para los próximos 14 días.";
        }

        int dias = (fechaFin - fechaInicio).Days + 1;
        if (dias > 5)
        {
            return "Un alquiler no puede durar más de 5 días.";
        }

        int cancelaciones = BD.ContarCancelacionesMesActual(idUsuario);
        if (cancelaciones >= 2)
        {
            return "Ya cancelaste 2 alquileres este mes. No podés volver a alquilar hasta el mes siguiente.";
        }

        int superpuestos = BD.ContarAlquileresSuperpuestos(idEquipo, fechaInicio, fechaFin);
        if (superpuestos > 0)
        {
            return "El equipo no está disponible porque ya está alquilado en esas fechas.";
        }

        return "";
    }

    public static decimal CalcularTotal(decimal precioPorDia, DateTime fechaInicio, DateTime fechaFin)
    {
        int dias = (fechaFin - fechaInicio).Days + 1;
        return dias * precioPorDia;
    }
}
