using System.Collections.Generic;

namespace CovidVaccinationNotifier
{
    public interface IMessageService
    {
        MessageResponse SendMessageToMobile(string Number, string Message);
        MessageResponse SendMessageToWhatsApp(string Number, string Message);
        List<MessageResponseRoot> SendMessages(MessageRequest requests);
    }
}