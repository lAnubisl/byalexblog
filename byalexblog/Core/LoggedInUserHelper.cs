using Microsoft.AspNetCore.Http;

namespace byalexblog.Core
{
    public class LoggedInUserHelper  : ILoggedInUserHelper
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public LoggedInUserHelper(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public bool IsAdmin()
        {
            return httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }
    }
}