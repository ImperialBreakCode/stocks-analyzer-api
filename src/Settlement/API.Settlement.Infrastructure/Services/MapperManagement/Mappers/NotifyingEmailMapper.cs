using API.Settlement.Domain.Entities.Emails;
using API.Settlement.Domain.Interfaces.MapperManagementInterfaces.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Infrastructure.Services.MapperManagement.Mappers
{
    public class NotifyingEmailMapper : INotifyingEmailMapper
    {
        public NotifyingEmail CreateNotifyingEmailDTO(string userEmail, string subject, string message)
        {
            return new NotifyingEmail()
            {
                To = userEmail,
                Subject = subject,
                Body = message
            };
        }
    }
}
