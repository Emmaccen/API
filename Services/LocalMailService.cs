using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public class LocalMailService : IMailService
    {
        private readonly IConfiguration appSettings;

        public LocalMailService(IConfiguration appSettings)
        {
            this.appSettings = appSettings ?? throw new ArgumentNullException();
        }
        public void ActivateMailService(String message, string subject)
        {
            Debug.WriteLine($"Mail from {appSettings["mailSettings:mailTo"]} to {appSettings["mailSettings:mailFrom"]}, with local mail service");
            Debug.WriteLine($"Mail Subject: {subject}");
            Debug.WriteLine($"Mail Message: {message}");
        }
    }
}
