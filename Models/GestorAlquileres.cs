using System;

namespace TP_FINAL_MOGUELEVSKY.Models;

public static class GestorAlquileres
{
    public static string ValidarReserva(int idUsuario, int idEquipo, DateTime fechaInicio, DateTime fechaFin)
    {
        DateTime hoy = DateTime.Today;

        if (fechaInicio < hoy)
        {
            return "La fecha de inicio no puede ser anterior a hoy.";
        }

        if (fechaFin < fechaInicio)
        {
            return "La fecha de fin no puede ser menor a la fecha de inicio.";
        }

        if (fechaInicio > hoy.AddDays(14))
        {
            return "Solo podés reservar dentro de los próximos 14 días.";
        }

        int dias = (fechaFin - fechaInicio).Days + 1;
        if (dias > 5)
        {
            return "Un alquiler no puede durar más de 5 días.";
        }

        int cancelaciones = BD.ContarCancelacionesMesActual(idUsuario);
        if (cancelaciones >= 2)
        {
            return "Ya tenés 2 cancelaciones este mes y no podés realizar nuevas reservas.";
        }

        int superpuestos = BD.ContarAlquileresSuperpuestos(idEquipo, fechaInicio, fechaFin);
        if (superpuestos > 0)
        {
            return "El equipo no está disponible en esas fechas.";
        }

        return "";
    }

    public static decimal CalcularTotal(decimal precioPorDia, DateTime fechaInicio, DateTime fechaFin)
    {
        int dias = (fechaFin - fechaInicio).Days + 1;
        return dias * precioPorDia;
    }
}
