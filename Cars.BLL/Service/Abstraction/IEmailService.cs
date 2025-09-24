namespace Cars.BLL.Service.Abstraction
{
    public interface IEmailService
    {
        Task SendEmail(string ToEmail, string Subject, string Body, string FromEmail);
        Task SendAcceptedOfferEmail(string MechanicEmail, string mechanicName, int accidentId , string acceptedOfferLink);
    }
}
