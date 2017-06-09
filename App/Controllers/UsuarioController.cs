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
    public class UsuarioController : Controller
    {
        private MsTradeEntities db = new MsTradeEntities();

        //
        // GET: /Usuario/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(Usuario user)
        {

            if (ModelState.IsValid)
            {
                if (IsValid(user.Email, user.Password))
                {
                    FormsAuthentication.SetAuthCookie(user.Email, false);
                    return RedirectToAction("Index", "Home");
                }

                else
                {
                    ModelState.AddModelError("", "Los Datos ingresado son Incorrectos.");

                }
            }
            return View(user);
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(Usuario user)
        {
            if (ModelState.IsValid)
            {
                db.Usuario.Add(user);
                db.SaveChanges();
                return RedirectToAction("LogIn", "Usuario");
            }
            return View(user);
        }


        public ActionResult Details()
        {
            Usuario usuario = db.Usuario.Find(User.Identity.Name);
            if (User.Identity.Name == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        public ActionResult DetailsAsignar()
        {
            Usuario usuario = db.Usuario.Find(User.Identity.Name);
            if (User.Identity.Name != null)
            {
                if (usuario.PerfilU == "admin")
                {
                    var userAsignar = db.Usuario.Include(s => s.Perfil);
                    return View(userAsignar.ToList());
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(usuario);

        }

        public ActionResult Asignar(int id=0)
        {
            Usuario usuario = db.Usuario.Where(x => x.IdUsuario == id).FirstOrDefault();
            if (usuario == null)
            {
                return HttpNotFound();

            }
            ViewBag.PerfilU = new SelectList(db.Perfil, "UserRol", "UserRol", usuario.PerfilU);
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Asignar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DetailsAsignar", "Usuario");
            }
            ViewBag.PerfilU = new SelectList(db.Perfil, "UserRol", "UserRol", usuario.PerfilU);
            return View(usuario);
        }

        public ActionResult Edit(string id = null)
        {
            Usuario usuario = db.Usuario.Find(User.Identity.Name);
            if (User.Identity.Name == null)
            {
                return HttpNotFound();
            }
            ViewBag.PerfilU = new SelectList(db.Perfil, "UserRol", "UserRol", usuario.PerfilU);
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                Usuario user = db.Usuario.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                user.Email = usuario.Email;
                user.Password = usuario.Password;
                user.PasswordSalt = usuario.PasswordSalt;
                user.Nombre = usuario.Nombre;
                user.Apellido = usuario.Apellido;
                user.TelefonoFijo = usuario.TelefonoFijo;
                user.TelefonoCellular = usuario.TelefonoCellular;
                user.Direccion = usuario.Direccion;
                user.Ciudad = usuario.Ciudad;
                db.SaveChanges();
                return RedirectToAction("Details", "Usuario");
            }
            ViewBag.PerfilU = new SelectList(db.Perfil, "UserRol", "UserRol", usuario.PerfilU);
            return View(usuario);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private bool IsValid(string email, string password)
        {

            bool isvalid = false;
            var user = db.Usuario.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                if (user.Password == password)
                {
                    isvalid = true;
                }
            }

            return isvalid;
        }
    }
}
