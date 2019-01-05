using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace StudentApplication.DataLayer
{
    public class RepositoryCourse
    {
        public static List<string> GetPrerequisites(string courseNum)
        {
            List<string> prerequisites = new List<string>();
            try
            {
                string sql = "select PreReqCourseNum from CoursePreRequisites where CourseNum=@CourseNum";
                List<SqlParameter> plist = new List<SqlParameter>();
                SqlParameter p = new SqlParameter("@CourseNum", SqlDbType.VarChar);
                p.Value = courseNum;
                plist.Add(p);
                DataTable dt = DataAccess.GetManyRowsCols(sql, plist);
                foreach(DataRow dr in dt.Rows)
                {
                    string preReq = dr["PreReqCourseNum"].ToString();
                    prerequisites.Add(preReq);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return prerequisites;
        }

        public static int GetEnrollmentCount(string semester, string courseNum)
        {
            int enrollmentCnt = 0;
            try
            {
                string sql = "Select count(*) from CourseEnrollments where SemesterId=@SemesterId and CourseNum=@CourseNum;";
                List<SqlParameter> param = new List<SqlParameter>();
                SqlParameter p1 = new SqlParameter("@SemesterId", SqlDbType.VarChar);
                SqlParameter p2 = new SqlParameter("@CourseNum", SqlDbType.VarChar);
                p1.Value = semester;
                p2.Value = courseNum;
                param.Add(p1);
                param.Add(p2);

                object count = DataAccess.GetSingleAnswer(sql, param);
                if (count != null)
                    enrollmentCnt = int.Parse(count.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            return enrollmentCnt;
        }

        public static bool IsVacancyThereInCourse(string CourseNum, string semester)
        {
            bool vacancy = false;
            try
            {
                string sql = "select Capacity from CoursesOffered where CourseNum=@CourseNum and " +
                    "SemesterId=@SemesterId";
                List<SqlParameter> plist = new List<SqlParameter>();
                SqlParameter p1 = new SqlParameter("@SemesterId", SqlDbType.VarChar);
                SqlParameter p2 = new SqlParameter("@CourseNum", SqlDbType.VarChar);
                p1.Value = semester;
                p2.Value = CourseNum;
                plist.Add(p1);
                plist.Add(p2);
                object result = DataAccess.GetSingleAnswer(sql, plist);
                if (result != null)
                {
                    int capacity = int.Parse(result.ToString());
                    int enrollment = GetEnrollmentCount(semester, CourseNum);
                    if (enrollment < capacity)
                        vacancy = true;
                }
                else
                    vacancy = false;
            }
            catch (Exception)
            {
                throw;
            }
            return vacancy;
        }
    }
}