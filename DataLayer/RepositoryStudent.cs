using StudentApplication.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentApplication.DataLayer
{
    public class RepositoryStudent
    {
        public static List<SelectListItem> GetCourses()
        {
            List<SelectListItem> CList = new List<SelectListItem>();
            try
            {
                string sql = "select * from Courses";
                DataTable dt = DataAccess.GetManyRowsCols(sql, null);
                foreach(DataRow dr in dt.Rows)
                {
                    SelectListItem si = new SelectListItem();
                    si.Value = dr["CourseNum"].ToString();
                    si.Text = dr["CourseNum"].ToString();
                    CList.Add(si);
                }
            }
            catch(Exception)
            {
                throw;
            }
            return CList;
        }

        public static List<SelectListItem> GetSemesters()
        {
            List<SelectListItem> SList = new List<SelectListItem>();
            try
            {
                string sql = "select SemesterId from Semesters";
                DataTable dt = DataAccess.GetManyRowsCols(sql, null);
                foreach (DataRow dr in dt.Rows)
                {
                    SelectListItem si = new SelectListItem();
                    si.Value = dr["SemesterId"].ToString();
                    si.Text = dr["SemesterId"].ToString();
                    SList.Add(si);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return SList;
        }

        public static List<CoursesEnrolled> GetenrollmentForASemester(string semester,string StudentId)
        {
            List<CoursesEnrolled> ECList = new List<CoursesEnrolled>();
            try
            {
                string sql = "select ce.CourseNum,c.CourseName,c.Description,d.DepartmentName,c.Credits,'-' Grade " +
                             ",case when REVERSE(SUBSTRING(REVERSE(rtrim(ltrim(SemesterId))),0,5))= YEAR(GETDATE()) then 'In Progress' else 'Yet to Start'   end Status " +
                             "from CourseEnrollments ce inner join Courses c on (ce.CourseNum = c.CourseNum) " +
                             "inner join Departments d on (d.DepartmentID = c.DepartmentID) " +
                             "where 1 = 1 and not exists(select 1 from CoursesCompleted cc where cc.StudentId = ce.StudentId and cc.CourseNum = ce.CourseNum) " +
                             "and ce.SemesterId = @semester and ce.studentid=@studentId " +
                             "union all " +
                             "select c.CourseNum,c.CourseName,c.Description,d.DepartmentName,c.Credits,CONVERT(varchar, cc.Grade) Grade, 'Completed' Status " +
                             "from CoursesCompleted cc inner join Courses c on(cc.CourseNum = c.CourseNum) " +
                             "inner join Departments d on(d.DepartmentID = c.DepartmentID) " +
                             "where 1 = 1 and cc.SemesterId = @semester and cc.studentid=@studentId ";
                List<SqlParameter> PList = new List<SqlParameter>();
                SqlParameter p1 = new SqlParameter("@semester", SqlDbType.VarChar);
                SqlParameter p2 = new SqlParameter("@studentId", SqlDbType.VarChar);
                p1.Value = semester;
                p2.Value = StudentId;
                PList.Add(p1);
                PList.Add(p2);
                DataTable dt = DataAccess.GetManyRowsCols(sql, PList);
                foreach(DataRow dr in dt.Rows)
                {
                    CoursesEnrolled crm = new CoursesEnrolled();
                    crm.CourseNum = dr["CourseNum"].ToString();
                    crm.CourseName = dr["CourseName"].ToString();
                    crm.Description = dr["Description"].ToString();
                    crm.DepartmentName = dr["DepartmentName"].ToString();
                    crm.Credits = (int)dr["Credits"];
                    crm.Grade = dr["Grade"].ToString();
                    crm.Status = dr["Status"].ToString();
                    ECList.Add(crm);
                }
            }
            catch(Exception)
            {
                throw;
            }
            return ECList;
        }

        public static List<CoursesEnrolled> dispCourse(string course)
        {
            List<CoursesEnrolled> CEList = new List<CoursesEnrolled>();
            try
            {
                string sql = "select c.CourseNum,c.CourseName,c.Description,d.DepartmentName,c.Credits " +
                             "from Courses c inner join Departments d on (c.departmentID = d.departmentid) " +
                             "where 1 = 1 and c.coursenum = @course ";
                List<SqlParameter> pList = new List<SqlParameter>();
                SqlParameter p1 = new SqlParameter("@course", SqlDbType.VarChar);
                p1.Value = course;
                pList.Add(p1);
                DataTable dt = DataAccess.GetManyRowsCols(sql, pList);
                foreach(DataRow dr in dt.Rows)
                {
                    CoursesEnrolled crm = new CoursesEnrolled();
                    crm.CourseNum = dr["CourseNum"].ToString();
                    crm.CourseName = dr["CourseName"].ToString();
                    crm.Description = dr["Description"].ToString();
                    crm.DepartmentName = dr["DepartmentName"].ToString();
                    crm.Credits = (int)dr["Credits"];
                    CEList.Add(crm);
                }
            }
            catch(Exception)
            {
                throw;
            }
            return CEList;
        }

        public static bool RegisterCourse(int StudentID, string semester, string course)
        {
            bool result = false;
            try
            {
                string sql = "Insert into CourseEnrollments(StudentId,SemesterId,CourseNum) values" +
                             "(@StudentId,@SemesterId,@CourseNum)";
                List<SqlParameter> Plist = new List<SqlParameter>();
                SqlParameter p1 = new SqlParameter("@StudentId", SqlDbType.Int);
                SqlParameter p2 = new SqlParameter("@SemesterId", SqlDbType.VarChar);
                SqlParameter p3 = new SqlParameter("@CourseNum", SqlDbType.VarChar);
                p1.Value = StudentID;
                p2.Value = semester;
                p3.Value = course;
                Plist.Add(p1);
                Plist.Add(p2);
                Plist.Add(p3);
                int rowsAffected = DataAccess.InsertUpdateDelete(sql, Plist);
                if (rowsAffected > 0)
                    result = true;
            }
            catch(Exception)
            {
                throw;
            }
            return result;
        }

        public static bool DoesStudentExists(int studentID)
        {
            bool result = false;
            try
            {
                string sql= "select StudentId from Students where StudentId=@StudentId";
                List<SqlParameter> Plist = new List<SqlParameter>();
                SqlParameter p = new SqlParameter("@StudentId", SqlDbType.Int);
                p.Value = studentID;
                Plist.Add(p);
                object obj = DataAccess.GetSingleAnswer(sql, Plist);
                if (obj != null)
                    result = true;
            }
            catch(Exception)
            {
                throw;
            }
            return result;
        }

        public static float? GetGradeForACourse(int studentID, string courseNum)
        {
            float? grade = null;
            try
            {
                string sql = "select grade from CoursesCompleted where " +
                    "CourseNum=@CourseNum and StudentId=@StudentId ";
                List<SqlParameter> pList = new List<SqlParameter>();
                SqlParameter p1 = new SqlParameter("@StudentId", SqlDbType.Int);                
                SqlParameter p2 = new SqlParameter("@CourseNum", SqlDbType.VarChar);
                p1.Value = studentID;
                p2.Value = courseNum;
                pList.Add(p1);
                pList.Add(p2);

                object obj = DataAccess.GetSingleAnswer(sql, pList);
                if (obj != null)
                {
                    grade = float.Parse(obj.ToString());
                }
            }
            catch (Exception)
            {
                throw;
            }
            return grade;
        }

        public static bool HasStudentTakenPreRequisites(int studentID, string courseNum, float minGrade)
        {
            bool result = true;
            try
            {                
                List<string> PreReqList = RepositoryCourse.GetPrerequisites(courseNum);

                foreach (string course in PreReqList)
                {
                    float? grade = GetGradeForACourse(studentID, course);
                    if (grade == null || grade < minGrade)
                    {
                        result = false;
                        break;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public static bool UnRegisterCourse(int StudentID, string CourseNum)
        {
            bool result = false;
            try
            {
                string sql = "Delete  from CourseEnrollments where StudentId = @StudentID and CourseNum = @CourseNum";
                List<SqlParameter> pList = new List<SqlParameter>();
                SqlParameter p1 = new SqlParameter("@StudentID", SqlDbType.Int);
                SqlParameter p2 = new SqlParameter("@CourseNum", SqlDbType.VarChar);
                p1.Value = StudentID;
                p2.Value = CourseNum;
                pList.Add(p1);
                pList.Add(p2);

                int rowsAffected = DataAccess.InsertUpdateDelete(sql, pList);
                if (rowsAffected > 0)
                    result = true;
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public static bool WasStudentRegisteredForCourse(int StudentID, string CourseNum)
        {
            bool result = false;
            try
            {
                string sql = "select StudentId from CourseEnrollments where StudentId=@StudentId and CourseNum=@CourseNum";
                List<SqlParameter> pList = new List<SqlParameter>();
                SqlParameter p1 = new SqlParameter("@StudentId", SqlDbType.Int);
                SqlParameter p2 = new SqlParameter("@CourseNum", SqlDbType.VarChar);
                p1.Value = StudentID;
                p2.Value = CourseNum;
                pList.Add(p1);
                pList.Add(p2);
                Object obj = DataAccess.GetSingleAnswer(sql, pList);
                if (obj != null)
                    result = true;
            }
            catch (Exception)

            {
                throw;
            }
            return result;
        }

        public static bool HasStudentAlreadyCompletedCourse(int StudentID, string CourseNum)
        {
            bool result = false;
            try
            {
                string sql = "select StudentId from CoursesCompleted where StudentId=@StudentId and CourseNum=@CourseNum";
                List<SqlParameter> pList = new List<SqlParameter>();
                SqlParameter p1 = new SqlParameter("@StudentId", SqlDbType.Int);
                SqlParameter p2 = new SqlParameter("@CourseNum", SqlDbType.VarChar);
                p1.Value = StudentID;
                p2.Value = CourseNum;
                pList.Add(p1);
                pList.Add(p2);
                Object obj = DataAccess.GetSingleAnswer(sql, pList);
                if (obj != null)
                    result = true;
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public static List<StudentTranscriptVM> Transcript(int studentId)
        {
            List<StudentTranscriptVM> svmList = new List<StudentTranscriptVM>();
            try
            {
                string sql = "select s.FirstName+' '+s.LastName Name,cc.SemesterId,c.CourseNum,c.CourseName,c.Description,d.DepartmentName,convert(varchar,cc.Grade) Grade " +
                             "from CoursesCompleted cc " +
                             "inner join Courses c on (c.CourseNum = cc.CourseNum) " +
                             "inner join Departments d on (c.departmentID = d.departmentid) " +
                             "inner join Students s on (cc.StudentId = s.StudentId) " +
                             "where 1 = 1 and s.StudentId = @studentID";
                List<SqlParameter> pList = new List<SqlParameter>();
                SqlParameter p = new SqlParameter("@studentID", SqlDbType.Int);
                p.Value = studentId;
                pList.Add(p);
                DataTable dt = DataAccess.GetManyRowsCols(sql, pList);
                foreach(DataRow dr in dt.Rows)
                {
                    StudentTranscriptVM svm = new StudentTranscriptVM();                    
                    svm.CourseNum = dr["CourseNum"].ToString();
                    svm.CourseName = dr["CourseName"].ToString();
                    svm.Description = dr["Description"].ToString();
                    svm.DepartmentName = dr["DepartmentName"].ToString();
                    svm.Grade = dr["Grade"].ToString();
                    svm.SemeterId = dr["SemesterId"].ToString();
                    svm.Name = dr["Name"].ToString();
                    svmList.Add(svm);
                }
            }
            catch(Exception)
            {
                throw;
            }
            return svmList;
        }

        public static string CalculateGPA(int StudentId)
        {
            string res = "";
            try
            {
                string sql = "select convert(varchar,isnull(sum(cc.Grade)/(case count(*) when 0 then 1 else count(*) end),0)) GPA " +
                             "from CoursesCompleted cc " +
                             "where 1=1 and cc.StudentId = @studentID ";
                List<SqlParameter> pList = new List<SqlParameter>();
                SqlParameter p = new SqlParameter("@studentID", SqlDbType.Int);
                p.Value = StudentId;
                pList.Add(p);
                DataTable dt = DataAccess.GetManyRowsCols(sql, pList);
                foreach(DataRow dr in dt.Rows)
                {
                    res = dr["GPA"].ToString();
                }
            }
            catch(Exception)
            {
                throw;
            }
            return res;
        }

        public static bool DeleteStudent(int StudentId)
        {
            bool res = false;
            try
            {
                string sql = "Delete from Students where Studentid = @studentID";
                List<SqlParameter> pList = new List<SqlParameter>();
                SqlParameter p = new SqlParameter("@studentID", SqlDbType.Int);
                p.Value = StudentId;
                pList.Add(p);
                int cnt = DataAccess.InsertUpdateDelete(sql, pList);
                if (cnt > 0)
                    res = true;
            }
            catch(Exception)
            {
                throw;
            }
            return res;
        }

        public static StudentVM getStudentDetails(int StudentId)
        {
            StudentVM svm = new StudentVM();
            try
            {
                string sql = "select StudentId,FirstName,LastName,Street,City,State,Email,Telephone from students where StudentId=@studentID";
                List<SqlParameter> pList = new List<SqlParameter>();
                SqlParameter p = new SqlParameter("@studentID", SqlDbType.Int);
                p.Value = StudentId;
                pList.Add(p);
                DataTable dt = DataAccess.GetManyRowsCols(sql, pList);
                foreach(DataRow dr in dt.Rows)
                {
                    svm.StudentId = (long)dr["StudentId"];
                    svm.FirstName = dr["FirstName"].ToString();
                    svm.LastName = dr["LastName"].ToString();
                    svm.Street = dr["Street"].ToString();
                    svm.City = dr["City"].ToString();
                    svm.State = dr["State"].ToString();
                    svm.Email = dr["Email"].ToString();
                    svm.Telephone = dr["Telephone"].ToString();
                }
            }
            catch(Exception)
            {
                throw;
            }
            return svm;
        }

        public static bool UpdateStudentDetails(StudentVM svm)
        {
            bool res = false;
            try
            {
                string sql = "update Students " +
                            "set FirstName = @FirstName, LastName = @LastName, Street = @Street, City = @City, State = @State, Email = @Email, Telephone = @Telephone " +
                            "where StudentId = @studentID";
                List<SqlParameter> pList = new List<SqlParameter>();
                SqlParameter p = new SqlParameter("@studentID", SqlDbType.Int);
                SqlParameter p1 = new SqlParameter("@FirstName", SqlDbType.VarChar);
                SqlParameter p2 = new SqlParameter("@LastName", SqlDbType.VarChar);
                SqlParameter p3 = new SqlParameter("@Street", SqlDbType.VarChar);
                SqlParameter p4 = new SqlParameter("@City", SqlDbType.VarChar);
                SqlParameter p5 = new SqlParameter("@State", SqlDbType.VarChar);
                SqlParameter p6 = new SqlParameter("@Email", SqlDbType.VarChar);
                SqlParameter p7 = new SqlParameter("@Telephone", SqlDbType.VarChar);
                p.Value = svm.StudentId;
                p1.Value = svm.FirstName;
                p2.Value = svm.LastName;
                p3.Value = svm.Street;
                p4.Value = svm.City;
                p5.Value = svm.State;
                p6.Value = svm.Email;
                p7.Value = svm.Telephone;

                pList.Add(p);
                pList.Add(p1);
                pList.Add(p2);
                pList.Add(p3);
                pList.Add(p4);
                pList.Add(p5);
                pList.Add(p6);
                pList.Add(p7);

                int cnt = DataAccess.InsertUpdateDelete(sql, pList);
                if (cnt > 0)
                    res = true;
            }
            catch(Exception)
            {
                throw;
            }
            return res;
        }
    }
}