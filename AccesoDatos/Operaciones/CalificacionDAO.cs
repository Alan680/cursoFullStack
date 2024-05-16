using AccesoDatos.Context;
using AccesoDatos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.Operaciones
{
    public class CalificacionDAO
    {
        ProyectoContext Contexto = new ProyectoContext();

        public List<Calificacion> seleccionar(int id)
        {
            var calificaciones = Contexto.Calificacions.Where(c => c.MatriculaId == id).ToList();

            return calificaciones;
        }
        public bool agregarCalificacion (Calificacion calif)
        {
            try
            {
                Contexto.Calificacions.Add(calif);
                Contexto.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public bool eliminarCalificacion (int id)
        {
            try
            {
                var calificacion = Contexto.Calificacions.Where(c => c.Id == id).FirstOrDefault();
                if (calificacion != null)
                {
                    Contexto.Calificacions.Remove(calificacion);
                    Contexto.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
