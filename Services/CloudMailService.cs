using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public class CloudMailService : IMailService
    {
        private readonly IConfiguration appSettings;

        public CloudMailService(IConfiguration appSettings)
        {
            this.appSettings = appSettings;
        }

        public void ActivateMailService(String message, string subject)
        {
            Debug.WriteLine($"Mail from {appSettings["mailSettings:mailTo"]} to {appSettings["mailSettings:mailFrom"]}, with Cloud mail service");
            Debug.WriteLine($"Mail Subject: {subject}");
            Debug.WriteLine($"Mail Message: {message}");
        }
    }
}
