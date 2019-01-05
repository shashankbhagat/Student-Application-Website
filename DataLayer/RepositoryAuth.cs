using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using StudentApplication.Models;

namespace StudentApplication.DataLayer
{
    public class RepositoryAuth
    {
        public static bool VerifyLogin(string username, string password)
        {
            bool result = false;
            try
            {
                string sql = "select UserName from Users where Username=@UserName and Password=@Password";
                List<SqlParameter> pList = new List<SqlParameter>();
                SqlParameter p1 = new SqlParameter("@UserName", System.Data.SqlDbType.VarChar, 50);
                p1.Value = username;
                SqlParameter p2 = new SqlParameter("@Password", System.Data.SqlDbType.VarChar, 50);
                p2.Value = password;
                pList.Add(p1);
                pList.Add(p2);
                object obj = DataAccess.GetSingleAnswer(sql, pList);
                if (obj != null)
                    result = true;
            }
            catch(Exception)
            {
                throw;
            }
            return result;
        }

        public static string GetRolesForUser(string username)
        {
            string roles = "";
            try
            {
                string sql = "select r.RoleName from roles r inner join userroles ur on (r.roleid=ur.roleid)" +
                            " where 1 = 1 and ur.UserName = @username";
                List<SqlParameter> pList = new List<SqlParameter>();
                SqlParameter p1 = new SqlParameter("@username", SqlDbType.VarChar, 50);
                p1.Value = username;
                pList.Add(p1);
                DataTable dt = DataAccess.GetManyRowsCols(sql, pList);
                foreach(DataRow dr in dt.Rows)
                {
                    roles += dr["RoleName"].ToString() + "|";
                    if (roles.Length > 0)
                        roles = roles.Substring(0, roles.Length - 1);
                }
            }
            catch(Exception)
            {
                throw;
            }
            return roles;
        }
    }
}