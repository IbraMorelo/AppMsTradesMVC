using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Models;

namespace App.Controllers
{
    public class ServicioHostController : Controller
    {
        private MsTradeEntities db = new MsTradeEntities();

        //
        // GET: /ServicioHost/

        public ActionResult Index()
        {
            var serviciohost = db.ServicioHost.Include(s => s.Usuario);
            return View(serviciohost.ToList());
        }

        //
        // GET: /ServicioHost/Details/5

        public ActionResult Details(int id = 0)
        {
            ServicioHost serviciohost = db.ServicioHost.Find(id);
            if (serviciohost == null)
            {
                return HttpNotFound();
            }
            return View(serviciohost);
        }

        //
        // GET: /ServicioHost/Create

        public ActionResult Create()
        {
            Usuario usuario = db.Usuario.Find(User.Identity.Name);
            if (User.Identity.Name == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmailS = new SelectList(db.Usuario.Where(x => x.PerfilU == "agente"), "Email", "Email");
            return View();
        }

        //
        // POST: /ServicioHost/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServicioHost serviciohost)
        {
            if (ModelState.IsValid)
            {
                ServicioHost serviH =new ServicioHost();
                Usuario user = db.Usuario.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                serviH.IdCliente = user.Email;
                serviH.Ciudad = serviciohost.Ciudad;
                serviH.Barrio = serviciohost.Barrio;
                serviH.Direccion = serviciohost.Direccion;
                serviH.PrecioVenta = serviciohost.PrecioVenta;
                serviH.Area = serviciohost.Area;
                serviH.Estrato = serviciohost.Estrato;
                serviH.PrecioServicio = 10000.0;
                serviH.Caracteristicas =serviciohost.Caracteristicas;
                serviH.EmailS = serviciohost.EmailS;
                db.ServicioHost.Add(serviH);
                db.SaveChanges();
                return RedirectToAction("Index", "Catalogo");
            }

            ViewBag.EmailS = new SelectList(db.Usuario, "Email", "Email", serviciohost.EmailS);
            return View(serviciohost);
        }

        //
        // GET: /ServicioHost/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ServicioHost serviciohost = db.ServicioHost.Find(id);
            if (serviciohost == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmailS = new SelectList(db.Usuario, "Email", "Email", serviciohost.EmailS);
            return View(serviciohost);
        }

        //
        // POST: /ServicioHost/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ServicioHost serviciohost)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serviciohost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmailS = new SelectList(db.Usuario, "Email", "Email", serviciohost.EmailS);
            return View(serviciohost);
        }

        //
        // GET: /ServicioHost/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ServicioHost serviciohost = db.ServicioHost.Find(id);
            if (serviciohost == null)
            {
                return HttpNotFound();
            }
            return View(serviciohost);
        }

        //
        // POST: /ServicioHost/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ServicioHost serviciohost = db.ServicioHost.Find(id);
            db.ServicioHost.Remove(serviciohost);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}