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
    public class ServicioComisionController : Controller
    {
        private MsTradeEntities db = new MsTradeEntities();

        //
        // GET: /ServicioComision/

        public ActionResult Index()
        {
            var serviciocomision = db.ServicioComision.Include(s => s.Usuario);
            return View(serviciocomision.ToList());
        }

        //
        // GET: /ServicioComision/Details/5

        public ActionResult Details(int id = 0)
        {
            ServicioComision serviciocomision = db.ServicioComision.Find(id);
            if (serviciocomision == null)
            {
                return HttpNotFound();
            }
            return View(serviciocomision);
        }

        //
        // GET: /ServicioComision/Create

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
        // POST: /ServicioComision/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServicioComision serviciocomision)
        {
            if (ModelState.IsValid)
            {
                ServicioComision serviC = new ServicioComision();
                Usuario user = db.Usuario.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                serviC.IdCliente = user.Email;
                serviC.Ciudad = serviciocomision.Ciudad;
                serviC.Barrio = serviciocomision.Barrio;
                serviC.Direccion = serviciocomision.Direccion;
                serviC.PrecioVenta = serviciocomision.PrecioVenta;
                serviC.Area = serviciocomision.Area;
                serviC.Estrato = serviciocomision.Estrato;
                serviC.PrecioServicio = 10000.0;
                serviC.Caracteristicas = serviciocomision.Caracteristicas;
                serviC.EmailS = serviciocomision.EmailS;
                serviC.PorcentajeComision = serviciocomision.PorcentajeComision;
                serviC.TotalComision = ((serviC.PrecioVenta) * ((serviC.PorcentajeComision)/100));
                db.ServicioComision.Add(serviC);
                db.SaveChanges();
                return RedirectToAction("Index", "Catalogo");
            }

            ViewBag.EmailS = new SelectList(db.Usuario, "Email", "Email", serviciocomision.EmailS);
            return View(serviciocomision);
        }

        //
        // GET: /ServicioComision/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ServicioComision serviciocomision = db.ServicioComision.Find(id);
            if (serviciocomision == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmailS = new SelectList(db.Usuario, "Email", "Email", serviciocomision.EmailS);
            return View(serviciocomision);
        }

        //
        // POST: /ServicioComision/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ServicioComision serviciocomision)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serviciocomision).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmailS = new SelectList(db.Usuario, "Email", "Email", serviciocomision.EmailS);
            return View(serviciocomision);
        }

        //
        // GET: /ServicioComision/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ServicioComision serviciocomision = db.ServicioComision.Find(id);
            if (serviciocomision == null)
            {
                return HttpNotFound();
            }
            return View(serviciocomision);
        }

        //
        // POST: /ServicioComision/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ServicioComision serviciocomision = db.ServicioComision.Find(id);
            db.ServicioComision.Remove(serviciocomision);
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