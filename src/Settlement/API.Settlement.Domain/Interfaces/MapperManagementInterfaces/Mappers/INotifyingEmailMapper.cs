using API.Settlement.Domain.Entities.Emails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Interfaces.MapperManagementInterfaces.Mappers
{
    public interface INotifyingEmailMapper
    {
        BaseEmail CreateNotifyingEmailDTO(string userEmail, string subject, string message);

    }
}
