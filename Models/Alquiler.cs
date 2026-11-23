using System;

namespace TP_FINAL_MOGUELEVSKY.Models;

public class Alquiler
{
    public int Id { get; set; }
<<<<<<< HEAD
    public int IdUsuario { get; set; }
    public int IdEquipo { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public decimal Total { get; set; }
    public bool Cancelado { get; set; }
=======
    public int IdEquipo { get; set; }
    public int IdUsuario { get; set; }
    public DateTime FechaRetiro { get; set; }
    public DateTime FechaDevolucion { get; set; }
    public decimal Total { get; set; }
    public bool Cancelado { get; set; }
    public DateTime? FechaCancelacion { get; set; }
>>>>>>> 0b635b3ae5ccb8a21210f8aa1ba8417681a0c172
    public string NombreEquipo { get; set; }
    public string ImagenEquipo { get; set; }
}
