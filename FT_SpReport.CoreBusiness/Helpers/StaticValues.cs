using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FT_SpReport.CoreBusiness.Helpers
{
    public class StaticValues
    {
        public const string SPParam = "SPParam";
        public const string SPName = "SPName";
        public const string USERLS = "User";
        public const string Content_Type = "Content-Type";
        public const string APP_JSON = "application/json";
        public const string APP_CORS = "Access-Control-Allow-Origin";
        public const string COOKIE = "Cookie";
        public const string COOKIE_NAME = "SpReport.CookieAuth";
        public const string BEARER = "Bearer";
        public const string Authorization = "Authorization";

        public const string AdministratorRole = "AdministratorRole";
        public const string ManagerRole = "ManagerRole";
        public const string UserRole = "UserRole";

        public const string RequireAdminPolicy = "RequireAdminPolicy";
        public const string RequireManagerPolicy = "RequireManagerPolicy";
        public const string RequireUserPolicy = "RequireUserPolicy";

        public const int PasswordLength = 4;
    }
}
