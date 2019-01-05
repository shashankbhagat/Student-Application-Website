using StudentApplication.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace StudentApplication.DataLayer
{
    public class RepositoryAdmin
    {
        public static bool AddStudent(StudentVM svm)
        {
            bool result = false;
            try
            {
                string sql = "insert into Students(FirstName,LastName,Street,City,State,Email,Telephone) " +
                             "values(@fname, @lname, @street, @city, @state, @mail, @phone)";
                List<SqlParameter> pList = new List<SqlParameter>();
                SqlParameter p1 = new SqlParameter("@fname", SqlDbType.VarChar);
                SqlParameter p2 = new SqlParameter("@lname", SqlDbType.VarChar);
                SqlParameter p3 = new SqlParameter("@street", SqlDbType.VarChar);
                SqlParameter p4 = new SqlParameter("@city", SqlDbType.VarChar);
                SqlParameter p5 = new SqlParameter("@state", SqlDbType.VarChar);
                SqlParameter p6 = new SqlParameter("@mail", SqlDbType.VarChar);
                SqlParameter p7 = new SqlParameter("@phone", SqlDbType.VarChar);
                p1.Value = svm.FirstName;
                p2.Value = svm.LastName;
                p3.Value = svm.Street;
                p4.Value = svm.City;
                p5.Value = svm.State;
                p6.Value = svm.Email;
                p7.Value = svm.Telephone;
                pList.Add(p1);
                pList.Add(p2);
                pList.Add(p3);
                pList.Add(p4);
                pList.Add(p5);
                pList.Add(p6);
                pList.Add(p7);

                int ret = DataAccess.InsertUpdateDelete(sql, pList);
                if (ret > 0)
                    result = true;
            }
            catch(Exception)
            {
                throw;
            }
            return result;
        }

        public static List<StudentVM> listStudents()
        {
            List<StudentVM> svmList = new List<StudentVM>();
            try
            {
                string sql = "select * from Students";
                DataTable dt = DataAccess.GetManyRowsCols(sql, null);
                foreach(DataRow dr in dt.Rows)
                {
                    StudentVM svm = new StudentVM();
                    svm.StudentId = (long)dr["StudentId"];
                    svm.FirstName = dr["FirstName"].ToString();
                    svm.LastName = dr["LastName"].ToString();
                    svm.Street = dr["Street"].ToString();
                    svm.City = dr["City"].ToString();
                    svm.State = dr["State"].ToString();
                    svm.Email = dr["Email"].ToString();
                    svm.Telephone = dr["Telephone"].ToString();
                    svmList.Add(svm);
                }
            }
            catch(Exception)
            {
                throw;
            }
            return svmList;
        }
    }
}