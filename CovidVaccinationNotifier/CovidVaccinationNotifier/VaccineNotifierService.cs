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
        private readonly List<string> mobileNumbers;
        private List<string> nextDates;
        private readonly string nextDays;
        public VaccineNotifierService()
        {
            isCallAPI = Convert.ToBoolean(ConfigurationManager.AppSettings["CALLAPI"]);
            pincode = ConfigurationManager.AppSettings["PINCODE"];
            date = ConfigurationManager.AppSettings["DATE"];
            district_id = ConfigurationManager.AppSettings["DISTRICT_ID"];
            _cowinAPIsFront = new CowinAPIsFront();
            _meesagaeService = new MessageService();
            mobileNumbers = ConfigurationManager.AppSettings["MOBILENUMBERS"].Split(';').ToList();
            nextDays = ConfigurationManager.AppSettings["NEXT_DAYS"];
            BuildDates(Convert.ToInt32(nextDays));
        }
        public void Start()
        {
            while (isCallAPI)
            {
                CowinVaccineSlotResponse findByDistrictResponse = null;
                //var findByPinResponse = _cowinAPIsFront.FindByPin(pincode, date);
                //if (findByPinResponse?.sessions?.Count > 0)
                //{
                //    CreateMessageRequest(findByPinResponse);
                //}
                foreach (var nDate in nextDates)
                {
                    findByDistrictResponse = _cowinAPIsFront.FindByDistrict(district_id, nDate);
                    if (findByDistrictResponse?.sessions?.Count > 0)
                    {
                        CreateMessageRequest(findByDistrictResponse);
                    }
                }
                if (messageRequests?.Messages?.Count>0)
                {
                    _meesagaeService.SendMessages(messageRequests);
                }

                //messageRequests.Dispose();
                
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
                    if (session.min_age_limit <= 18)
                    {
                        var isYoungAllowed = session.min_age_limit <= 18 ? "YES" : "NO";
                        string buildMessage = $"Slot Available\nPinCode : {session.pincode} | 18+ : {isYoungAllowed} | Address: {session.address} | " +
                            $"Available Capacity: {session.available_capacity}, " +
                            $"Date : {session.date}, Vaccine : {session.vaccine}";
                        foreach (var number in mobileNumbers)
                        {
                            messageRequests.Messages.Add(new Messages(number, buildMessage, Channel.sms, $"{Guid.NewGuid()}"));
                            //messageRequests.Messages.Add(new Messages("919685317178", buildMessage, Channel.sms, $"{Guid.NewGuid()}"));
                            //messageRequests.Messages.Add(new Messages("919340930902", buildMessage, Channel.sms, $"{Guid.NewGuid()}"));
                        }
                    }
                }
            }
            return messages;
        }

        private void BuildDates(int days)
        {
            int count = 0;
            nextDates = new List<string>();
            while (count <= days)
            {
                var day = DateTime.Now.Day;
                day += count;
                var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, day).ToShortDateString();
                nextDates.Add(date);
                count++;            
            }
        }
    }
}
