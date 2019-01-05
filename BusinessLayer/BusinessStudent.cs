using StudentApplication.DataLayer;
using StudentApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentApplication.BusinessLayer
{
    public class BusinessStudent
    {
        public static List<SelectListItem> GetCourses()
        {
            return RepositoryStudent.GetCourses();
        }

        public static List<SelectListItem> GetSemesters()
        {
            return RepositoryStudent.GetSemesters();
        }

        public static List<CoursesEnrolled> GetenrollmentForASemester(string semester,string studentId)
        {
            return RepositoryStudent.GetenrollmentForASemester(semester,studentId);
        }

        public static List<CoursesEnrolled> dispCourse(string course)
        {
            return RepositoryStudent.dispCourse(course);
        }

        public static bool RegisterCourse(int StudentID, string semester, string course)
        {
            bool result = false;
            try
            {
                if (RepositoryStudent.DoesStudentExists(StudentID))
                {
                    if (RepositoryStudent.HasStudentTakenPreRequisites(StudentID, course, 2.0f))
                    {
                        if (RepositoryCourse.IsVacancyThereInCourse(course, semester))
                        {
                            result = RepositoryStudent.RegisterCourse(StudentID, semester, course);
                        }
                        else
                            throw new Exception("Course capacity exceeded..");
                    }
                    else
                        throw new Exception("Missing Prerequisite for " + course);
                }
                else
                    throw new Exception("Student with ID: " + StudentID + " not registered");
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public static bool UnregisterCourse(int studentID, string course)
        {
            bool result = false;
            try
            {
                if (RepositoryStudent.DoesStudentExists(studentID))
                {
                    if (RepositoryStudent.WasStudentRegisteredForCourse(studentID, course))
                    {
                        if (!RepositoryStudent.HasStudentAlreadyCompletedCourse(studentID, course))
                        {
                            result = RepositoryStudent.UnRegisterCourse(studentID, course);
                        }
                        else
                            throw new Exception("Student is already graded for this course.");
                    }
                    else
                        throw new Exception("Student was not enrolled for this course.");
                }
                else
                {
                    throw new Exception("Student is not registered.......");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public static List<StudentTranscriptVM> Transcript(int studentId)
        {
            return RepositoryStudent.Transcript(studentId);
        }

        public static bool DeleteStudent(int studentId)
        {
            return RepositoryStudent.DeleteStudent(studentId);
        }

        public static StudentVM getStudentDetails(int StudentId)
        {
            return RepositoryStudent.getStudentDetails(StudentId);
        }

        public static bool UpdateStudentDetails(StudentVM svm)
        {
            return RepositoryStudent.UpdateStudentDetails(svm);
        }
    }
}