using HRD.WebApi.ViewModels.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRD.WebApi.Services
{
    public interface IEmailService
    {
        string EmailSubject(EnumEmailNotificationType type);
        string EmailBodyMessage(EnumEmailNotificationType type);
        void SendEmail(List<string> toAddresses, string emailSubject, string emailBody);
    }
}
