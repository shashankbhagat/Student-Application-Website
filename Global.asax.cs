using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace StudentApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public void Application_PostAuthenticateRequest(object sender,EventArgs e)
        {
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Context.Request.Cookies[cookieName];
            if (authCookie == null)
                return;
            FormsAuthenticationTicket authticket = null;
            try
            {
                authticket = FormsAuthentication.Decrypt(authCookie.Value);
            }
            catch(Exception)
            {
                throw;
            }
            if (authticket == null)
                return;

            string roles = authticket.UserData;
            string[] rolesArray = roles.Split(new char[] { '|' });
            HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(User.Identity, rolesArray);
        }
    }
}
