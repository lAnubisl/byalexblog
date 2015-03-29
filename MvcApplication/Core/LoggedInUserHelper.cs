using System.Web;

namespace MvcApplication.Core
{
    public class LoggedInUserHelper  : ILoggedInUserHelper
    {
        public bool IsAdmin()
        {
            return HttpContext.Current != null && 
                    HttpContext.Current.User != null &&
                    HttpContext.Current.User.Identity != null && 
                    HttpContext.Current.User.Identity.Name == "BlogAdmin";
        }
    }
}