using StudentApplication.DataLayer;
using StudentApplication.Models;
using StudentApplication.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentApplication.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult StudentDefault()
        {
            return View();
        }

        [Authorize(Roles ="Student")]
        public ActionResult ShowEnrollmentSemwise()
        {
            CourseEnrolledVM cvm = new CourseEnrolledVM();
            cvm.SList = BusinessLayer.BusinessStudent.GetSemesters();
            cvm.semSelected = cvm.SList[0].Value;
            string studentId = SessionFacade.LOGGEDIN;
            if (studentId == null)
                return RedirectToAction("Login", "Auth");
            cvm.CEList = BusinessLayer.BusinessStudent.GetenrollmentForASemester(cvm.semSelected,studentId);

            return View(cvm);
        }

        public ActionResult ShowEnrollmentSemwise_Ajaxform(string semSelected)
        {
            List<CoursesEnrolled> CEnrolledList = null;
            string studentId = SessionFacade.LOGGEDIN;
            if (semSelected != null && studentId != null)
                CEnrolledList= BusinessLayer.BusinessStudent.GetenrollmentForASemester(semSelected, studentId);
            return PartialView("_ShowEnrollmentSemwise", CEnrolledList);
        }

        [Authorize(Roles ="Student")]
        public ActionResult RegisterCourse()
        {
            RegisterCourseVM rvm = new RegisterCourseVM();
            rvm.CList = BusinessLayer.BusinessStudent.GetCourses();
            rvm.CSelected = rvm.CList[0].Value;
            rvm.semList = BusinessLayer.BusinessStudent.GetSemesters();
            rvm.semselected = rvm.semList[0].Value;
            string studentId = SessionFacade.LOGGEDIN;
            if (studentId == null)
                return RedirectToAction("Login", "Auth");
            rvm.course = BusinessLayer.BusinessStudent.dispCourse(rvm.CSelected);
            return View(rvm);
        }

        
        //debug from here
        [HttpPost]
        public ActionResult RegisterCourse(string btnRegister, string CSelected, RegisterCourseVM rvm)
        {
            List<CoursesEnrolled> CEnrolledList = null;
            bool res=false;
            rvm.CList = BusinessLayer.BusinessStudent.GetCourses();
            rvm.semList = BusinessLayer.BusinessStudent.GetSemesters();
            string studentId = SessionFacade.LOGGEDIN;
            if (studentId == null)
                return RedirectToAction("Login", "Auth");
            rvm.course = BusinessLayer.BusinessStudent.dispCourse(rvm.CSelected);
            int StudID = int.Parse(studentId);
            if (CSelected != null && studentId != null)
                CEnrolledList = BusinessLayer.BusinessStudent.dispCourse(CSelected);

            if (btnRegister != null)
            {
                if (btnRegister.ToUpper() == "SUBMIT")
                {
                    try
                    {
                        res = BusinessLayer.BusinessStudent.RegisterCourse(StudID, rvm.semselected, rvm.CSelected);
                    }
                    catch (Exception e)
                    {
                        ViewBag.Exmsg = e.Message;
                    }
                    if (res == true)
                        ViewBag.msg = "Course Registered.....";
                    else
                        ViewBag.msg = "Problem registering course.....";
                }
                
            }
                //List<CoursesEnrolled> CEnrolledList = null;
                //string studentId = SessionFacade.LOGGEDIN;
                
            
            return PartialView("_RegisterCourse", CEnrolledList);
        }        
        

        [Authorize(Roles ="Student")]
        public ActionResult UnRegisterCourse()
        {
            RegisterCourseVM rvm = new RegisterCourseVM();
            rvm.CList = BusinessLayer.BusinessStudent.GetCourses();
            rvm.CSelected = rvm.CList[0].Value;
            string studentId = SessionFacade.LOGGEDIN;
            if (studentId == null)
                return RedirectToAction("Login", "Auth");
            rvm.course = BusinessLayer.BusinessStudent.dispCourse(rvm.CSelected);
            return View(rvm);
        }


        [HttpPost]
        public ActionResult unRegisterCourse(RegisterCourseVM rvm, string btnSubmit,string CSelected)
        {
            bool res = false;
            rvm.CList = BusinessLayer.BusinessStudent.GetCourses();            
            string studentId = SessionFacade.LOGGEDIN;
            if (studentId == null)
                return RedirectToAction("Login", "Auth");
            rvm.course = BusinessLayer.BusinessStudent.dispCourse(rvm.CSelected);
            int StudID = int.Parse(studentId);
            if(btnSubmit!=null)
            {
                if (btnSubmit.ToUpper() == "SUBMIT")
                {
                    try
                    {
                        res = BusinessLayer.BusinessStudent.UnregisterCourse(StudID, rvm.CSelected);
                    }
                    catch(Exception e)
                    {
                        ViewBag.Exmsg = e.Message;
                    }
                    if (res == true)
                        ViewBag.msg = "Course has been un-registered.....";
                    else
                        ViewBag.msg = "Problem un-registering course.....";
                }
            }

            return PartialView("_RegisterCourse", rvm.course);
        }

        public ActionResult StudentTranscript()
        {
            List<StudentTranscriptVM> svmList = new List<StudentTranscriptVM>();
            string studentId = SessionFacade.LOGGEDIN;
            if (studentId == null)
                return RedirectToAction("Login", "Auth");
            ViewBag.StudentID = studentId;
            string gpa = RepositoryStudent.CalculateGPA(int.Parse(studentId));
            ViewBag.GPA = gpa;
            svmList = BusinessLayer.BusinessStudent.Transcript(int.Parse(studentId));
            return View(svmList);
        }
    }
}