using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {

        Employee_CRUDEntities1 db = new Employee_CRUDEntities1();
        public ActionResult Index()
        {
            var data = db.Employees.ToList();
            return View(data);
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Employee e)
        {
            if(ModelState.IsValid == true)
            {
                string filename = Path.GetFileNameWithoutExtension(e.imagefile.FileName);
                string extension = Path.GetExtension(e.imagefile.FileName);
                HttpPostedFileBase postedFile = e.imagefile;
                int length = postedFile.ContentLength;

                if(extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                {
                    if(length <=  1000000)
                    {
                        filename = filename + extension;
                        e.image_path = "~/Images/" + filename;
                        filename = Path.Combine(Server.MapPath("~/Images/"), filename);
                        e.imagefile.SaveAs(filename);
                        db.Employees.Add(e);
                        int a = db.SaveChanges();

                        if(a > 0) { 
                        
                        TempData["CreateMessage"] = "<script>alert('Data inserted successfully')</script>";

                            ModelState.Clear();
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            TempData["CreateMessage"] = "<script>alert('Data not inserted successfuuly')</script>";

                        }
                    }
                    else
                    {
                        TempData["SizeMessage"] = "<script>alert('Size should not greater than 1MB')</script>";
                    }
                }
                else
                {
                    TempData["ExtensionMessage"] = "<script>alert('Format not supported')</script>";
                }
            }
            return View();
        }

        public ActionResult Edit(int id)
        {

            var EmployeeRow = db.Employees.Where(model => model.id == id).FirstOrDefault();
            Session["Image"] = EmployeeRow.image_path;
            return View(EmployeeRow);
        }

        [HttpPost]
        public ActionResult Edit(Employee e)
        {
            if (ModelState.IsValid == true)
            {
                if (e.imagefile != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(e.imagefile.FileName);
                    string extension = Path.GetExtension(e.imagefile.FileName);
                    HttpPostedFileBase postedFile = e.imagefile;
                    int length = postedFile.ContentLength;

                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                    {
                        if (length <= 1000000)
                        {
                            filename = filename + extension;
                            e.image_path = "~/Images/" + filename;
                            filename = Path.Combine(Server.MapPath("~/Images/"), filename);
                            e.imagefile.SaveAs(filename);
                           
                            db.Entry(e).State=EntityState.Modified;
                        
                            int a = db.SaveChanges();

                            if (a > 0)
                            {

                                TempData["UpdateMessage"] = "<script>alert('Data updated successfully')</script>";

                                ModelState.Clear();
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                TempData["UpdateMessage"] = "<script>alert('Data not updated successfuuly')</script>";

                            }
                        }
                        else
                        {
                            TempData["SizeMessage"] = "<script>alert('Size should not greater than 1MB')</script>";
                        }
                    }
                    else
                    {
                        TempData["ExtensionMessage"] = "<script>alert('Format not supported')</script>";
                    }

                }
                else
                {
                    e.image_path = Session["Image"].ToString();
                    db.Entry(e).State = EntityState.Modified;

                    int a = db.SaveChanges();

                    if (a > 0)
                    {
                        string ImagePath = Request.MapPath(Session["Image"].ToString());
                        if (System.IO.File.Exists(ImagePath))
                        {
                            System.IO.File.Delete(ImagePath);
                        }

                        TempData["UpdateMessage"] = "<script>alert('Data updated successfully')</script>";

                        ModelState.Clear();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["UpdateMessage"] = "<script>alert('Data not updated successfully')</script>";

                    }

                }
            }
            

            return View();
        }
        public ActionResult Delete(int id)
            
        {
            if (id > 0)
            {

                var EmployeeRow = db.Employees.Where(model => model.id == id).FirstOrDefault();
                if(EmployeeRow != null)
                {
                    db.Entry(EmployeeRow).State= EntityState.Deleted;
                  int a=  db.SaveChanges();
                    if (a > 0){
                        TempData["DeleteMessage"]= "<script>alert('Employee deleted successfuuly')</script>";
                        string ImagePath = Request.MapPath(EmployeeRow.image_path.ToString());
                        if (System.IO.File.Exists(ImagePath)) { 
                        System.IO.File.Delete(ImagePath);
                        }
                    
                    }
                    else
                    {
                        TempData["DeleteMessage"] = "<script>alert('Employee not deleted')</script>";
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Details(int id)
            
        {
            var EmployeeRow = db.Employees.Where(model => model.id == id).FirstOrDefault();
            Session["Image2"] = EmployeeRow.image_path.ToString();
            return View(EmployeeRow);
        }

    }
}