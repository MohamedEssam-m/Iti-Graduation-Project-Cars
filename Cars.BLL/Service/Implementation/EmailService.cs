using Cars.BLL.Service.Abstraction;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.Service.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;
        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task SendEmail(string ToEmail, string Subject, string Body)
        {
            var from = configuration["EmailSettings:From"];
            var SmtpServer = configuration["EmailSettings:SmtpServer"];
            var Port = int.Parse( configuration["EmailSettings:Port"]);
            var UserName = configuration["EmailSettings:UserName"];
            var Password = configuration["EmailSettings:Password"];
            var message = new MailMessage(from, ToEmail ,Subject , Body);
            message.IsBodyHtml = true;
            using var Client = new SmtpClient(SmtpServer, Port)
            {
                Credentials = new NetworkCredential(UserName, Password),
                EnableSsl = true
            };
            await Client.SendMailAsync(message);
        }
        
    }
}
