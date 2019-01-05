using StudentApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentApplication.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult AdminDefault()
        {
            return View();
        }

        public ActionResult AddStudent()
        {
            StudentVM svm = new StudentVM();
            return View(svm);
        }

        [HttpPost]
        public ActionResult AddStudent(StudentVM svm)
        {
            if(ModelState.IsValid)
            {
                bool result = BusinessLayer.BusinessAdmin.AddStudent(svm);
                if (result == true)
                    ViewBag.msg = "Student added successfully....";
                else
                    ViewBag.msg = "Problem adding student..";
            }
            return View(svm);
        }
        
        public ActionResult listStudents()
        {
            StudentListVM slvm = new StudentListVM();
            slvm.SList = BusinessLayer.BusinessAdmin.listStudents();
            return View(slvm);
        }

        public ActionResult DeleteStudent(int id)
        {
            try
            {
                bool res = BusinessLayer.BusinessStudent.DeleteStudent(id);
                if (res)
                    return new HttpStatusCodeResult(200);
                else
                    return new HttpStatusCodeResult(500);
            }
            catch(Exception ex)
            {
                ViewBag.msg = ex.Message;
                return new HttpStatusCodeResult(500);
            }
        }

        public ActionResult EditStudent(int id)
        {
            StudentVM svm = BusinessLayer.BusinessStudent.getStudentDetails(id);
            return PartialView("_EditStudent", svm);
        }

        [HttpPost]
        public ActionResult EditStudent(StudentVM svm)
        {
            if (!ModelState.IsValid)
                return View(svm);
            bool res = false;
            try
            {
                res = BusinessLayer.BusinessStudent.UpdateStudentDetails(svm);
            }
            catch(Exception ex)
            {
                ViewBag.Status = ex.Message;
            }

            if (res)
                ViewBag.Status = "Student data updated successfully..";
            return Json(new { result = true, responseText = "Student data updated successfully.." });
        }

        public ActionResult Details(int id)
        {
            StudentVM svm = BusinessLayer.BusinessStudent.getStudentDetails(id);
            return PartialView("_StudentDetail", svm);
        }
    }

}