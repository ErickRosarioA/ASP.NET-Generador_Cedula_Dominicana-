using ParcialErickGenerator.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace ParcialErickGenerator.Controllers
{
    public class HomeController : Controller
    {
        private GenerateContext Modelo_Generator = new GenerateContext();
     
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(REGISTRO login)
        {
           
            foreach (var item in Modelo_Generator.REGISTRO.ToList())
            {
                ViewBag.Login = "";
                if (login.USUARIO==item.USUARIO && login.PASSAWORD==item.PASSAWORD)
                {

                    return RedirectToAction("Inicio");
                }

            }

            ViewBag.Login = "USUARIOS INCORRECTO";
            return View();
        }

        public ActionResult Inicio()
        {
            return View();
        }


        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(REGISTRO registro)
        {
            if (ModelState.IsValid)
            {
                registro.CEDULARPERSONAL= new Random().Next(0, 100);
                Modelo_Generator.REGISTRO.Add(registro);
                Modelo_Generator.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Busqueda()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Busqueda(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CEDULAS cedula = Modelo_Generator.CEDULAS.Find(id);
            if (cedula == null)
            {

                TempData["Message"] = "NO EXISTE LA CEDULA DIGITADA";
               
                return Content("<script language='javascript' type='text/javascript'> alert('NO EXISTE LA CEDULA SOLCITADA') </script>");
            }

            return RedirectToAction("CedulaVista",new { id=cedula.ID });
        }

        public ActionResult CedulaVista(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CEDULAS cedula = Modelo_Generator.CEDULAS.Find(id);
            if (cedula == null)
            {
                return HttpNotFound();
            }
            return View(cedula);
        }

        public ActionResult List()
        {
            
            return View(Modelo_Generator.CEDULAS.ToList());
        }

    
        public byte[] Imagen(HttpPostedFileBase img)
        {
            byte[] imageByte = null;
            BinaryReader leer = new BinaryReader(img.InputStream);
            imageByte = leer.ReadBytes((int)img.ContentLength);
            return imageByte;

        }

        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Create(CEDULAS cedula, HttpPostedFileBase FOTO)
        {
                        
            if (ModelState.IsValid)
            {

                if (cedula.FOTO!=null)
                {
                    try
                    {

                        string path = Path.Combine(Server.MapPath("~/Content/Images"), Path.GetFileName(FOTO.FileName));
                        FOTO.SaveAs(path);

                        cedula.FOTO = FOTO.FileName;

                    }
                    catch(Exception)
                    {

                    }

                   

                }


                cedula.ID_CEDULAS = Guid.NewGuid();
                cedula.REGISTRONACIMIENTO = Guid.NewGuid();
                Modelo_Generator.CEDULAS.Add(cedula);
                Modelo_Generator.SaveChanges();
                return RedirectToAction("CedulaVista",new { id=cedula.ID });
            }
            return View(cedula);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CEDULAS cedula = Modelo_Generator.CEDULAS.Find(id);
            if (cedula == null)
            {
                return HttpNotFound();
            }
            return View(cedula);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CEDULAS cedula, HttpPostedFileBase FOTO)
        {
            if (ModelState.IsValid)
            {

                if (cedula.FOTO != null)
                {
                    try
                    {

                        string path = Path.Combine(Server.MapPath("~/Content/Images"), Path.GetFileName(FOTO.FileName));
                        FOTO.SaveAs(path);

                        cedula.FOTO = FOTO.FileName;

                    }
                    catch (Exception)
                    {

                    }



                }


                Modelo_Generator.Entry(cedula).State = EntityState.Modified;
                Modelo_Generator.SaveChanges();
                return RedirectToAction("List");
            }
            return View(cedula);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CEDULAS cedula = Modelo_Generator.CEDULAS.Find(id);
            if (cedula == null)
            {
                return HttpNotFound();
            }
            return View(cedula);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CEDULAS cedula = Modelo_Generator.CEDULAS.Find(id);
            Modelo_Generator.CEDULAS.Remove(cedula);
            Modelo_Generator.SaveChanges();
            return RedirectToAction("List");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Modelo_Generator.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}