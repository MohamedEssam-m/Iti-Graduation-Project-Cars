using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.Service.Abstraction
{
    public interface IEmailService
    {
        Task SendEmail(string ToEmail, string Subject, string Body);
    }
}
