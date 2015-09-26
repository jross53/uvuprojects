using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CSStudent.Models;

namespace CSStudent.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            List<StudentModel> list = DatabaseQueries.ReadAllStudents();
            return View(list);
        }

        // GET: Student/Details/5
        public ActionResult Details(int id)
        {
            StudentModel student = DatabaseQueries.ReadStudent(id);
            return View(student);
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                bool canCode = collection["CanCode"].Split(',')[0].Equals("true");
                StudentModel student = new StudentModel()
                {
                    ExpGraduation = Convert.ToDateTime(collection["ExpGraduation"]),
                    Name = collection["Name"],
                    CanCode = canCode,
                    CreditsLeft = Int32.Parse(collection["CreditsLeft"]),
                    Advisor = collection["Advisor"]
                };

                DatabaseQueries.CreateStudent(student);

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError,
                    "Error commiting info to database: " + ex.Message);
            }
        }

        // GET: Student/Edit/5
        public ActionResult Edit(int id)
        {
            StudentModel student = DatabaseQueries.ReadStudent(id);
            return View(student);
        }

        // POST: Student/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                var canCodeStringValue = collection["CanCode"];
                bool canCode;
                if (canCodeStringValue.Contains(","))
                {
                    canCode = collection["CanCode"].Split(',')[0].Equals("true");
                }
                else
                {
                    canCode = !canCodeStringValue.Equals("false");
                }
                
                StudentModel student = new StudentModel()
                {
                    Id = id,
                    ExpGraduation = Convert.ToDateTime(collection["ExpGraduation"]),
                    Name = collection["Name"],
                    CanCode = canCode,
                    CreditsLeft = Int32.Parse(collection["CreditsLeft"]),
                    Advisor = collection["Advisor"]
                };

                DatabaseQueries.UpdateStudent(student);

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError,
                    "Error commiting info to database: " + ex.Message);
            }
        }

        // GET: Student/Delete/5
        public ActionResult Delete(int id)
        {
            StudentModel student = DatabaseQueries.ReadStudent(id);
            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                DatabaseQueries.DeleteStudent(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError,
                    "Error commiting info to database: " + ex.Message);
            }
        }
    }
}
