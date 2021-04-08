using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public class LocalMailService : ILocalMailService
    {
        private readonly string _MailTo = "@admin.gmail.com";
        private readonly string _MailFrom = "@noreply.gmail.com";

        public void ActivateMailService(String message, string subject)
        {
            Debug.WriteLine($"Mail from {_MailFrom} to {_MailTo}, with local mail service");
            Debug.WriteLine($"Mail Subject: {subject}");
            Debug.WriteLine($"Mail Message: {message}");
        }
    }
}
