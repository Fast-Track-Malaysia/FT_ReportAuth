using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FT_ReportAuth.Services
{
    public class AuthMessageSenderOptions
    {
        public const string AuthMessageSender = "AuthMessageSender";
        public string SendGridEmail { get; set; }
        public string SendGridKey { get; set; }
    }
}
