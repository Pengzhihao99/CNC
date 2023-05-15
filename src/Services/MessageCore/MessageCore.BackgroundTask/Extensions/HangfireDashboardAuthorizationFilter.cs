using Hangfire.Dashboard;

namespace MessageCore.BackgroundTask.Extensions
{
    /// <summary>
    /// Hangfire Dashboard 远程访问权限
    /// </summary>
    public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            // In case you need an OWIN context, use the next line, `OwinContext` class
            // is the part of the `Microsoft.Owin` package.
            //var owinContext = new OwinContext(context.GetOwinEnvironment());

            // Allow all authenticated users to see the Dashboard (potentially dangerous).
            //return owinContext.Authentication.User.Identity.IsAuthenticated;

            return true;
        }
    }
}
