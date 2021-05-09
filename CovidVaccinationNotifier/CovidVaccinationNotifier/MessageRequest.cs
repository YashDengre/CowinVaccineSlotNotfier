using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidVaccinationNotifier
{
    public class MessageRequest
    {
        public string To { get; set; }
        public string Message { get; set; }
        public string SeachBy { get; set; }
        [JsonProperty(PropertyName = "messages")]
        public List<Messages> Messages { get; set; }

    }

    public class Messages
    {
        public Messages(string to, string message, Channel channel, string msgTrxId)
        {
            this.Channel = channel;
            this.Content = message;
            this.To = to;
            this.MessageTrasactionId = msgTrxId;
        }
        [JsonProperty(PropertyName = "channel")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Channel Channel { get; set; }
        [JsonProperty(PropertyName = "to")]
        public string To { get; set; }
        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }
        [JsonIgnore]
        public string MessageTrasactionId { get; set; }
    }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Channel
    {
        sms,
        whatsapp
    }
}
