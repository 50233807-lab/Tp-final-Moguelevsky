using System;

namespace TP_FINAL_MOGUELEVSKY.Models;

public class Alquiler
{
    public int Id { get; set; }
    public int IdEquipo { get; set; }
    public int IdUsuario { get; set; }
    public DateTime FechaRetiro { get; set; }
    public DateTime FechaDevolucion { get; set; }
    public decimal Total { get; set; }
    public bool Cancelado { get; set; }
    public DateTime? FechaCancelacion { get; set; }
    public string NombreEquipo { get; set; }
    public string ImagenEquipo { get; set; }
}
