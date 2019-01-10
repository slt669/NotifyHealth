using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System.Web;
using System.Web.Mvc;

namespace NotifyHealth
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {

            var cao = new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider { OnApplyRedirect = ApplyRedirect }
            };
            app.UseCookieAuthentication(cao);
        }
        private static void ApplyRedirect(CookieApplyRedirectContext context)
        {

            UrlHelper _url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            string actionUri = _url.Action("Login", "Account", new { });
            context.Response.Redirect(actionUri);
        }


        //// Enable the application to use a cookie to store information for the signed in user
        //app.UseCookieAuthentication(new CookieAuthenticationOptions
        //{
        //    ExpireTimeSpan = System.TimeSpan.FromMinutes(1),
        //    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
        //    LoginPath = new PathString("/Account/Logoff")
        //});
        ////// Use a cookie to temporarily store information about a user logging in with a third party login provider
        //app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

        // Uncomment the following lines to enable logging in with third party login providers
        //app.UseMicrosoftAccountAuthentication(
        //    clientId: "",
        //    clientSecret: "");

        //app.UseTwitterAuthentication(
        //   consumerKey: "",
        //   consumerSecret: "");

        //app.UseFacebookAuthentication(
        //   appId: "",
        //   appSecret: "");

        //app.UseGoogleAuthentication();

    }
}