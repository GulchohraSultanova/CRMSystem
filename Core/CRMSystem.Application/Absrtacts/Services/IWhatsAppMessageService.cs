using CRMSystem.Application.Dtos.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Application.Absrtacts.Services
{
    public interface IWhatsAppMessageService
    {
        Task SendMessageAsync(WhatsAppMessageDto dto);
    }
}
