using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidVaccinationNotifier
{
    public class VaccineNotifierService : IVaccineNotifierService
    {
        private bool isCallAPI;
        private string date;
        private string pincode;
        private string district_id;
        private readonly ICowinAPIsFront _cowinAPIsFront;
        private readonly IMessageService _meesagaeService;
        private MessageRequest messageRequests;
        public VaccineNotifierService()
        {
            isCallAPI = Convert.ToBoolean(ConfigurationManager.AppSettings["CALLAPI"]);
            pincode = ConfigurationManager.AppSettings["PINCODE"];
            date = ConfigurationManager.AppSettings["DATE"];
            district_id = ConfigurationManager.AppSettings["DISTRICT_ID"];
            _cowinAPIsFront = new CowinAPIsFront();
            _meesagaeService = new MessageService();

        }
        public void Start()
        {
            while (isCallAPI)
            {
                var findByPinResponse = _cowinAPIsFront.FindByPin(pincode, date);
                if (findByPinResponse?.sessions?.Count > 0)
                {
                    CreateMessageRequest(findByPinResponse);
                }

                var findByDistrictResponse = _cowinAPIsFront.FindByDistrict(district_id, date);
                if (findByDistrictResponse?.sessions?.Count > 0)
                {
                    CreateMessageRequest(findByDistrictResponse);
                }
                if(messageRequests?.Messages?.Count>0)
                {
                    _meesagaeService.SendMessages(messageRequests);
                }
            }
        }
        public void Stop()
        {

        }

        private MessageRequest CreateMessageRequest(CowinVaccineSlotResponse response)
        {

            MessageRequest messages = new MessageRequest();
            if(messageRequests==null)
            {
                messageRequests = new MessageRequest()
                {
                    Messages = new List<Messages>()
                };
            }
            if (response?.sessions?.Count > 0)
            {
                foreach (var session in response.sessions)
                {
                    if (session.min_age_limit >= 18)
                    {
                        var isYoungAllowed = session.min_age_limit <= 18 ? "YES" : "NO";
                        string buildMessage = $"Slot Available\nPinCode : {session.pincode} | 18+ : {isYoungAllowed} | Address: {session.address} | " +
                            $"Available Capacity: {session.available_capacity}, " +
                            $"Date : {session.date}, Vaccine : {session.vaccine}";
                        messageRequests.Messages.Add(new Messages("918109729582", buildMessage, Channel.sms, $"{Guid.NewGuid()}"));
                        messageRequests.Messages.Add(new Messages("919685317178", buildMessage, Channel.sms, $"{Guid.NewGuid()}"));
                        messageRequests.Messages.Add(new Messages("919340930902", buildMessage, Channel.sms, $"{Guid.NewGuid()}"));
                    }
                }
            }
            return messages;
        }
    }
}
