using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using App.Models;
using System.Text;

namespace App.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        private MsTradeEntities db = new MsTradeEntities();
        public ActionResult Index()
        {
            StringBuilder htmlOutput = new StringBuilder();
           
            try{
                var usuario = db.Usuario.FirstOrDefault(u => u.Email == User.Identity.Name);
                if (User.Identity.Name != null)
                {
                    if (usuario.PerfilU == "admin")
                    {
                        htmlOutput.Append("<li><a href=\"/Usuario/DetailsAsignar\">Asignar Agente</a></li>");

                    }
                    else if (usuario.PerfilU == "agente")
                    {
                        htmlOutput.Append("<li><a href=\"#\">Manejar Publicaciones</a></li>");
                    }
                    else if (usuario.PerfilU == "cliente")
                    {
                        htmlOutput.Append("<li><a href=\"/ServicioComision/Create\">Servicio Comision</a></li>");
                        htmlOutput.Append("<li><a href=\"/ServicioHost/Create\">Servicio Host</a></li>");
                    }
                    else if (usuario.PerfilU == null)
                    {
                        htmlOutput.Append("<li><a href=\"/ServicioComision/Create\">Servicio Comision</a></li>");
                        htmlOutput.Append("<li><a href=\"/ServicioHost/Create\">Servicio Host</a></li>");
                    }

                }
            }catch(Exception e){

            }
            
            ViewBag.HtmlOutput = htmlOutput.ToString();
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
    }
}
