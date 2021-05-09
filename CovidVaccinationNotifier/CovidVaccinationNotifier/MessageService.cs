using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidVaccinationNotifier
{
    public class MessageService : IMessageService
    {
        private readonly ICallGenericAPI _callGenericAPI;
        private string URL { get; set; }
        private string resource { get; set; }
        private Dictionary<string, string> Dict_Headers;
        private Dictionary<string, string> Dict_AuthHeader;

        public MessageService()
        {
            _callGenericAPI = new CallGenericAPI();
            URL = ConfigurationManager.AppSettings["CLICK_A_TELL_URL"];
            resource = ConfigurationManager.AppSettings["CLICK_A_TELL_RESOURCE"];
            BuildHeaders(ref Dict_Headers, ref Dict_AuthHeader);
        }
        public MessageResponse SendMessageToMobile(string Number, string Message)
        {
            return new MessageResponse();
        }
        public MessageResponse SendMessageToWhatsApp(string Number, string Message)
        {
            return new MessageResponse();
        }

        public List<MessageResponseRoot> SendMessages(MessageRequest requests)
        {
            List<MessageResponseRoot> responses = new List<MessageResponseRoot>();
            int index = 0;
            int partSize = 10;
            int runCount = requests.Messages.Count / partSize;
            while (runCount > 0)
            {
                var messages = requests.Messages.Skip(index * partSize).Take(partSize);
                var jsonBodyData = new
                {
                    messages = new List<Messages>()
                };
                foreach (var msg in messages)
                {
                    jsonBodyData.messages.Add(msg);
                }
                var bodyJson = Newtonsoft.Json.JsonConvert.SerializeObject(jsonBodyData);
                var result = _callGenericAPI.CallAPI<MessageResponseRoot>(URL,
                    RestSharp.Method.POST,
                    bodyContent: bodyJson,
                    pathParams: null, queryParams: null,
                    authHeaders: Dict_AuthHeader, headers: Dict_Headers, resource);
                result.Object.ResultCode = result.resultCode;
                result.Object.ResponseMessage = result.message;
                responses.Add(result.Object);
                runCount--;
            }
            return responses;
        }

        private static void BuildHeaders(ref Dictionary<string, string> headersDict,
            ref Dictionary<string, string> authHeadersDict)
        {
            var HeaderString = ConfigurationManager.AppSettings["HEADERS"];
            var AuthHearderString = ConfigurationManager.AppSettings["AUTH_HEADERS"];
            if (headersDict == null)
            {
                headersDict = new Dictionary<string, string>();
            }
            if (authHeadersDict == null)
            {
                authHeadersDict = new Dictionary<string, string>();
            }
            var Headers = HeaderString.Split(',');
            var AuthHeaders = AuthHearderString.Split(',');
            foreach (var header in Headers)
            {
                if (!string.IsNullOrEmpty(header))
                {
                    var tmpHeader = header.Split(':');
                    if (tmpHeader[0] != "" && tmpHeader[1] != "")
                        headersDict.Add(tmpHeader[0], tmpHeader[1]);
                }
            }
            foreach (var authHeader in AuthHeaders)
            {
                if (!string.IsNullOrEmpty(authHeader))
                {
                    var tmpAuthHeader = authHeader.Split(':');
                    if (tmpAuthHeader[0] != "" && tmpAuthHeader[1] != "")
                        authHeadersDict.Add(tmpAuthHeader[0], tmpAuthHeader[1]);
                }
            }

        }
    }
}
