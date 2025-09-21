using Cars.BLL.Service.Abstraction;
using Cars.DAL.Entities.Accidents;
using Microsoft.AspNetCore.Http;
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
        private readonly IAccidentService accidentService;
        private readonly IAppUserService appUserService;
        public EmailService(IAccidentService accidentService,IAppUserService appUserService,IConfiguration configuration)
        {
            this.configuration = configuration;
            this.accidentService = accidentService;
            this.appUserService = appUserService;
        }
        public async Task SendEmail(string ToEmail, string Subject, string Body, string FromEmail = "zeyadazzap0@gmail.com")
        {
            //var FromEmail = configuration["EmailSettings:From"];
            var SmtpServer = configuration["EmailSettings:SmtpServer"];
            var Port = int.Parse(configuration["EmailSettings:Port"]);
            var UserName = configuration["EmailSettings:UserName"];
            var Password = configuration["EmailSettings:Password"];
            var message = new MailMessage(FromEmail, ToEmail, Subject, Body);
            message.IsBodyHtml = true;
            using var Client = new SmtpClient(SmtpServer, Port)
            {
                Credentials = new NetworkCredential(UserName, Password),
                EnableSsl = true
            };
            await Client.SendMailAsync(message);
        }
        public async Task SendAcceptedOfferEmail(string MechanicEmail, string mechanicName, int accidentId , string acceptedOfferLink)
        {
            var AllAccidents = await accidentService.GetAllAccidents();
            var Accident = await accidentService.GetAccidentById(accidentId);
            var user = await appUserService.GetById(Accident.UserId);

            var subject = "Your Offer Has Been Accepted!";
            var body = $@"
        <div style='font-family: Arial, sans-serif; color: #333;'>
            <h2>Your offer has been accepted 🎉</h2>
            <p>Hello {mechanicName},</p>
            <p>Great news! Your offer for <strong>{user.UserName}</strong>'s accident report has been <span style='color:green; font-weight:bold;'>accepted</span>.</p>
            <p>Click the button below to view the accident details and proceed:</p>
            <a href='{acceptedOfferLink}' 
               style='display:inline-block; padding:10px 20px; margin-top:10px;
                      background-color:#007bff; color:#fff; text-decoration:none;
                      border-radius:5px;'>
               View Accepted Offer
            </a>
            <p style='margin-top:20px; font-size:12px; color:#777;'>If you believe this was a mistake, please contact support.</p>
        </div>
    ";


            await SendEmail(MechanicEmail, subject, body);
    }


    }
}
