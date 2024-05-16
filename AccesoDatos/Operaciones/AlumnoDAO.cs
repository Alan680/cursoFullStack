using AccesoDatos.Context;
using AccesoDatos.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AccesoDatos.Operaciones
{
    public class AlumnoDao
    {
        public ProyectoContext contexto = new ProyectoContext();

        public List<Alumno> seleccionarTodos()
        {
            var alumnos = contexto.Alumnos.ToList<Alumno>();
            return alumnos;
        }

        public Alumno seleccionar(int id)
        {
            var alumno = contexto.Alumnos.Where(a => a.Id == id).FirstOrDefault();
            return alumno;
        }

        public Alumno seleccionarDni(string dni)
        {
            var alumno = contexto.Alumnos.Where(a => a.Dni.Equals(dni)).FirstOrDefault();
            return alumno;
        }

        public bool insertar(string dni, string nombre, string direccion, int edad, string email)
        {
            try
            {
                Alumno alumno = new Alumno();
                alumno.Dni = dni;
                alumno.Nombre=nombre;
                alumno.Direccion=direccion;
                alumno.Edad = edad;
                alumno.Email=email;

                contexto.Alumnos.Add(alumno);
                contexto.SaveChanges();

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public bool actualizar(int id, string dni, string nombre, string direccion, int edad, string email)
        {
            try
            {
                var alumno = seleccionar(id);
                if (alumno == null)
                {
                    return  false;
                }
                else {
                    alumno.Dni = dni;
                    alumno.Nombre = nombre;
                    alumno.Direccion = direccion;
                    alumno.Edad = edad;
                    alumno.Email = email;

                    contexto.SaveChanges();

                    return true;
                }
            
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool eliminar(int id)
        {
            try
            {
                var alumno = seleccionar(id);
                if (alumno == null)
                {
                    return  false;
                }
                else
                {
                    contexto.Alumnos.Remove(alumno);

                    contexto.SaveChanges();

                    return true;
                }

            }
            catch (Exception e)
            {
                return false;
            }

        }
        public List<AlumnosAsignatura> seleccionarAlumnosAsignaturas()
        {
            var query = from a in contexto.Alumnos
                        join m in contexto.Matriculas on a.Id equals m.AlumnoId
                        join asig in contexto.Asignaturas on m.AsignaturaId equals asig.Id
                        select new AlumnosAsignatura
                        {
                            NombreAlumno = a.Nombre,
                            NombreAsignatura = asig.Nombre
                        };
            return query.ToList();
        }
        public List<AlumnoProfesor> seleccionarAlumnosProfesor(string Usuario)
        {
            var query = from a in contexto.Alumnos
                        join m in contexto.Matriculas on a.Id equals m.AlumnoId
                        join asig in contexto.Asignaturas on m.AsignaturaId equals asig.Id
                        where asig.Profesor == Usuario
                        select new AlumnoProfesor
                        {
                            Id = a.Id,
                            Dni = a.Dni,
                            Nombre = a.Nombre,
                            Direccion = a.Direccion,
                            Edad = a.Edad,
                            Email = a.Email,
                            Asignatura = asig.Nombre,
                            MatriculaId = m.Id
                        };
            return query.ToList();
        }
        public bool insertarYMatricular (string dni, string nombre, string direccion, int edad, string email, int id_asig)
        {
            try
            {
                var existe = seleccionarDni(dni);
                if (existe == null)
                {
                    insertar(dni, nombre, direccion, edad, email);

                    var insertado = seleccionarDni(dni);
                    Matricula m = new Matricula();
                    m.AlumnoId = insertado.Id;
                    m.AsignaturaId = id_asig;
                    contexto.Matriculas.Add(m);
                    contexto.SaveChanges();
                }
                else
                {
                    Matricula m = new Matricula();
                    m.AlumnoId = existe.Id;
                    m.AsignaturaId = id_asig;
                    contexto.Matriculas.Add(m);
                    contexto.SaveChanges();

                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool eliminarAlumno (int id)
        {
            try
            {
                var alumno = seleccionar(id);

                if (alumno != null)
                {
                    var matriculas = contexto.Matriculas.Where(m => m.AlumnoId == id);
                    foreach(Matricula m in matriculas)
                    {
                        var calificaciones = contexto.Calificacions.Where(c => c.MatriculaId == m.Id);
                        contexto.Calificacions.RemoveRange(calificaciones);
                    }
                    contexto.Matriculas.RemoveRange(matriculas);
                    contexto.Alumnos.Remove(alumno);
                    contexto.SaveChanges();
                    return true;

                }
                else
                {
                    return false;
                }

            }
            catch(Exception ex) 
            {
                return false; 
            }
        }
    }
}
