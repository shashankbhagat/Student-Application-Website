using StudentApplication.DataLayer;
using StudentApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentApplication.BusinessLayer
{
    public class BusinessAdmin
    {
        public static bool AddStudent(StudentVM svm)
        {
            return RepositoryAdmin.AddStudent(svm);
        }

        public static List<StudentVM> listStudents()
        {
            return RepositoryAdmin.listStudents();
        }
    }
}