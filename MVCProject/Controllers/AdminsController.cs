using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCProject.Models;
using MVCProject.ViewModel;
namespace MVCProject.Controllers
{
    public class AdminsController : Controller
    {
        private MyDataContext db = new MyDataContext();

        // GET: Admins
        public ActionResult Index()
        {
            return View(db.Admins.ToList());
        }

        // GET: Admins/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // GET: Admins/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Admin_Id,Password")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admin);
        }

        // GET: Admins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Admin_Id,Password")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admin);
        }

        // GET: Admins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Admin admin = db.Admins.Find(id);
            db.Admins.Remove(admin);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Admin_Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Admin_Login(Admin admin)
        {
            var email = admin.Email;
            var pass = admin.Password;
            //var data = db.Admins.Where(add => add.Password.Trim() == pass && add.Admin_Id == id).FirstOrDefault();
            var data = db.Admins.Where(add => add.Password == pass && add.Email.Trim()==email).FirstOrDefault();
            if (data != null && Session["myid"]==null)
            {  
                Session["myid"] = email;
                Session["username"] = "Admin";
                var Department = db.Departments.ToList();
                var viewModel = new NewFacultyViewModel()
                {
                    department = Department
                };
                return View("Admin_level1", viewModel);
            }
             return RedirectToAction("Admin_Login");
            //return RedirectToAction("Submit");
        }

        public JsonResult IsUserExists(string email)
        {
            //check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.  
            return Json(!db.Admins.Any(x => x.Email == email), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Submit(Faculty faculty)
        {
            if (Session["myid"] != null)
            {
                db.Faculties.Add(faculty);
                db.SaveChanges();
                var fact = db.Faculties.ToList();
                return View("Registration", fact);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult FacultyList(string searchstring,string sortorder)
        {
            if (Session["myid"] != null)
            {
                //ViewBag.DepartmentNameSortParam = string.IsNullOrEmpty(sortorder) ? "DName_Desc" : "";
                ViewBag.FacultyName = sortorder == "Name" ? "Name_Desc" : "Name";
                var list_faculty = from f in db.Faculties select f;
                if (!string.IsNullOrEmpty(searchstring))
                {
                    
                    list_faculty = list_faculty.Where(x => x.Name.Contains(searchstring));

                }
                switch (sortorder)
                {
                    case "Name_Desc":
                        list_faculty = list_faculty.OrderByDescending(x => x.Name);
                            break;
                    case "Name":
                        list_faculty = list_faculty.OrderBy(x => x.Name);
                        break;
                    default:
                        break;
                }
                return View(list_faculty);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            if (Session["myid"] != null)
            {
                Session.Abandon();
                return RedirectToAction("Index","Home");
            }
            return RedirectToAction("Index", "Home");

        }
    }
}
