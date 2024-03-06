using Microsoft.AspNetCore.Mvc;
using MvcCoreEmpleadosSession.Extensions;
using MvcCoreEmpleadosSession.Models;
using MvcCoreEmpleadosSession.Repositories;

namespace MvcCoreEmpleadosSession.Controllers
{
    public class EmpleadosController : Controller
    {
        private RepositoryEmpleados repo;
        public EmpleadosController(RepositoryEmpleados repo)
        {
            this.repo = repo;
        }     
        /* VERSION 1*/
        public async Task<IActionResult> SessionSalarios(int? salario)
        {
            if(salario != null)
            {
                int sumaSalarial = 0;
                if (HttpContext.Session.GetString("SUMASALARIAL") != null)
                {
                    sumaSalarial = int.Parse(HttpContext.Session.GetString("SUMASALARIAL"));
                }
                sumaSalarial += salario.Value;
                HttpContext.Session.SetString("SUMASALARIAL", sumaSalarial.ToString());
                ViewData["MENSAJE"] = "Salario almacenado: " + sumaSalarial;
            }
            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
            return View(empleados);
        }

        public IActionResult SumaSalarios()
        {
            return View();
        }        
        public async Task<IActionResult> SessionEmpleados(int? idEmpleado)
        {
            if (idEmpleado != null)
            {
                Empleado empleado = await this.repo.FindEmpleadoAsync(idEmpleado.Value);
                List<Empleado> empleadosList;
                if (HttpContext.Session.GetObject<List<Empleado>>("EMPLEADOS") != null)
                {
                    empleadosList = HttpContext.Session.GetObject<List<Empleado>>("EMPLEADOS");
                }
                else
                {
                    empleadosList = new List<Empleado>();
                }
                empleadosList.Add(empleado);
                HttpContext.Session.SetObject("EMPLEADOS", empleadosList);
                ViewData["MENSAJE"] = "Empleado almacenado: " + empleado.Apellido;
            }
            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
            return View(empleados);
        }        
        public async Task<IActionResult> SessionEmpleadosOk(int? idempleado)
        {
            if (idempleado != null)
            {
                List<int> idsEmpleados;
                if (HttpContext.Session.GetString("IDSEMPLEADOS") == null)
                {
                    idsEmpleados = new List<int>();
                }
                else
                {
                    idsEmpleados = HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");

                }
                idsEmpleados.Add(idempleado.Value);
                HttpContext.Session.SetObject("IDSEMPLEADOS", idsEmpleados);
                ViewData["MENSAJE"] = "Empleados almacenados: " + idsEmpleados.Count;
            }
            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
            return View(empleados);
            //Version 4, eliminandolos de la lista si ya están en Session.
            /*            List<int> ids = HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
                        if (ids == null)
                        {
                            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
                            return View(empleados);
                        }
                        else
                        {
                            List<Empleado> empleados = await this.repo.GetEmpleadosNotSessionAsync(ids);
                            return View(empleados);
                        }*/

        }

        public IActionResult EmpleadosSession()
        {
            return View();
        }        
        public IActionResult EmpleadosAlmacenados()
        {
            return View();
        }        
        public async Task<IActionResult> EmpleadosAlmacenadosOk(int? ideliminar)
        {
            List<int> idsEmpleados = HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");

            if(idsEmpleados != null)
            {
                //Debemos eliminar de Session
                if (ideliminar != null)
                {
                    idsEmpleados.Remove(ideliminar.Value);

                    if (idsEmpleados.Count == 0)
                    {
                        HttpContext.Session.Remove("IDEMPLEADOS");
                    }
                    else
                    {
                        HttpContext.Session.SetObject("IDSEMPLEADOS", idsEmpleados);
                    }
                }
                List<Empleado> empleados = await 
                    this.repo.GetEmpleadosSessionAsync(idsEmpleados);
                return View(empleados);
            }
            return View();
        }
    }
}
