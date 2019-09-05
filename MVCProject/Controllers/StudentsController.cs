using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCProject.Models;

namespace MVCProject.Controllers
{
    public class StudentsController : Controller
    {
        private MyDataContext db = new MyDataContext();

        // GET: Students
        public ActionResult Index()
        {
            var students = db.Students.Include(s => s.dept);
            return View(students.ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.Dept_Id = new SelectList(db.Departments, "Dept_Id", "Name");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Stud_Id,Name,Password,Dept_Id,Attendance,TotalDays")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Dept_Id = new SelectList(db.Departments, "Dept_Id", "Name", student.Dept_Id);
            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.Dept_Id = new SelectList(db.Departments, "Dept_Id", "Name", student.Dept_Id);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Stud_Id,Name,Password,Dept_Id,Attendance,TotalDays")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Dept_Id = new SelectList(db.Departments, "Dept_Id", "Name", student.Dept_Id);
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
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

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Student student)
        {

            var email = student.Email;
            var pass = student.Password;
            var data = db.Students.Where(add => add.Password.Trim() == pass && add.Email == email).FirstOrDefault();
            if (data != null && Session["myid"] == null)
            {
                Session["myid"] = email;
                Session["username"] = data.Name;
                return View("Student_Details",data);    
            }
            return RedirectToAction("Login");
        }

        public JsonResult IsUserExists(string email)
        {
            //check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.  
            return Json(!db.Students.Any(x => x.Email == email), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            if (Session["myid"] != null)
            {
                Session.Abandon();
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index","Home");

        }

    }
}
