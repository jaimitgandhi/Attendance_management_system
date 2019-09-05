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
using PagedList;
namespace MVCProject.Controllers
{
    public class FacultiesController : Controller
    {
        private MyDataContext db = new MyDataContext();

        // GET: Faculties
        public ActionResult Index()
        {
            var faculty = db.Faculties.Include(f => f.dept);
            return View(faculty.ToList());
        }

        // GET: Faculties/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Faculty faculty = db.Faculties.Find(id);
            if (faculty == null)
            {
                return HttpNotFound();
            }
            return View(faculty);
        }

        // GET: Faculties/Create
        public ActionResult Create()
        {
            ViewBag.Dept_Id = new SelectList(db.Departments, "Dept_Id", "Name");
            return View();
        }

        // POST: Faculties/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Fact_Id,Name,Password,Dept_Id")] Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                db.Faculties.Add(faculty);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Dept_Id = new SelectList(db.Departments, "Dept_Id", "Name", faculty.Dept_Id);
            return View(faculty);
        }

        // GET: Faculties/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Faculty faculty = db.Faculties.Find(id);
            /*if (faculty == null)
            {
                return HttpNotFound();
            }
            ViewBag.Dept_Id = new SelectList(db.Departments, "Dept_Id", "Name", faculty.Dept_Id);
            return View(faculty);*/
            return RedirectToAction("Edit","Students",id);
        }

        // POST: Faculties/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                db.Entry(faculty).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Dept_Id = new SelectList(db.Departments, "Dept_Id", "Name", faculty.Dept_Id);
            return View(faculty);
        }

        // GET: Faculties/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Faculty faculty = db.Faculties.Find(id);
            if (faculty == null)
            {
                return HttpNotFound();
            }
            return View(faculty);
        }

        // POST: Faculties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Faculty faculty = db.Faculties.Find(id);
            db.Faculties.Remove(faculty);
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
        public ActionResult Login(Faculty faculty)
        {

            var email = faculty.Email;
            var pass = faculty.Password;
            var data = db.Faculties.Where(add => add.Password.Trim() == pass && add.Email == email).FirstOrDefault();
            if (data != null && Session["myid"] == null)
            {
                Session["myid"] = email;
                Session["username"] = data.Name;
                var Department = db.Departments.ToList();
                var viewModel = new NewStudentViewModel()
                {
                    department = Department
                };

                Session["department_id"] = data.Dept_Id;
                return View("Faculty_level1", viewModel);
            }
             return RedirectToAction("Login");
        }

        public JsonResult IsUserExists(string email)
        {
            //check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.  
            return Json(!db.Faculties.Any(x => x.Email == email), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Submit(Student student)
        {
            if (Session["department_id"].ToString() != null && Session["myid"] != null)
            {
                student.Dept_Id = Int32.Parse(Session["department_id"].ToString());

                db.Students.Add(student);
                db.SaveChanges();
                // var stud = db.Students.ToList();
                //return View("Registration", stud);
                return View("StudentList");
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult StudentList(string searchstring,string sortorder,int? page)
        {
            if (Session["myid"] != null)
            {
                ViewBag.StudentName = string.IsNullOrEmpty(sortorder) ? "Name_Desc" : "";
                ViewBag.StudentAttendance = sortorder == "Attendance" ? "Attendance_Desc" : "Attendance";
                int facultyid = Int32.Parse(Session["department_id"].ToString());
                var stud = db.Students.Where(std => std.Dept_Id == facultyid);
                if (!string.IsNullOrEmpty(searchstring))
                {

                    stud = stud.Where(x => x.Name.Contains(searchstring));

                }
                stud = stud.OrderBy(x => x.Stud_Id);
                int pagesize = 3;
                int pagenumber = (page ?? 1);
                switch (sortorder)
                {
                    case "Name_Desc":
                        stud = stud.OrderByDescending(x => x.Name);
                        break;
                    case "Attendance_Desc":
                        stud = stud.OrderByDescending(x => x.Attendance);
                        break;
                    case "Attendance":
                        stud = stud.OrderBy(x => x.Attendance);
                        break;
                    default:
                        break;
                }
                return View(stud.ToPagedList(pagenumber, pagesize));
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            if (Session["myid"] != null)
            {
                Session.Abandon();
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");

        }
    }
}
