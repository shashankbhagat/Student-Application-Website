using StudentApplication.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentApplication.BusinessLayer
{
    public class BusinessAuth
    {
        public static bool VerifyLogin(string Username, string Password)
        {
            return RepositoryAuth.VerifyLogin(Username, Password);
        }

        public static string GetRolesForUser(string Username)
        {
            return RepositoryAuth.GetRolesForUser(Username);
        }
    }
}