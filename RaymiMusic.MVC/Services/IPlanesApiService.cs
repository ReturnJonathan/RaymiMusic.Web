using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RaymiMusic.Modelos;

namespace RaymiMusic.MVC.Services
{
    public interface IPlanesApiService
    {
        Task<IEnumerable<PlanSuscripcion>> ObtenerTodosAsync();
        Task<PlanSuscripcion> CrearAsync(PlanSuscripcion plan);
        // luego podrás añadir ActualizarAsync y EliminarAsync si lo necesitas
    }
}
