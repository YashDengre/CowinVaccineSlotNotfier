using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidVaccinationNotifier
{
    public class MessageResponseRoot
    {
        public int ResultCode { get; set; }
        public string ResponseMessage { get; set; }
        public string To { get; set; }
        public string MessageTransactionId { get; set; }

        [JsonProperty(PropertyName = "messages")]
        public List<MessageResponse> messageResponse { get; set; }
       
    }
    public class MessageResponse
    {
        public string apiMessageId { get; set; }
        public bool accepted { get; set; }
        public string to { get; set; }
        public Error error
        {
            get; set;
        }
    }
    public class Error
    {
        public int code { get; set; }
        public string description { get; set; }
    }
}
