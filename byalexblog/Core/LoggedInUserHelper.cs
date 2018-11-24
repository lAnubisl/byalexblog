using System.Web;

namespace byalexblog.Core
{
    public class LoggedInUserHelper  : ILoggedInUserHelper
    {
        public bool IsAdmin()
        {
            return true;
        }
    }
}