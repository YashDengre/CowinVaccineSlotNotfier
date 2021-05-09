using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidVaccinationNotifier
{
    public class CallGenericAPI : ICallGenericAPI
    {
        private RestClient restClient;

        public CallGenericAPI()
        {

        }

        public Result<T> CallAPI<T>(string url,
            Method method,
            object bodyContent,
            Dictionary<string, string> pathParams = null,
            Dictionary<string, string> queryParams = null,
            Dictionary<string, string> authHeaders = null,
            Dictionary<string, string> headers = null,
            string resource = "")
        {
            var result = new Result<T>();
            var restClient = new RestClient(url);
            var request = new RestRequest(method);

            if (!string.IsNullOrEmpty(resource))
            {
                request.Resource = resource;
            }
            if (queryParams?.Count > 0)
            {
                foreach (var queryParam in queryParams)
                {
                    request.AddQueryParameter(queryParam.Key, queryParam.Value, true);
                }
            }
            if (pathParams?.Count > 0)
            {
                foreach (var pathParam in pathParams)
                {
                    request.AddParameter(pathParam.Key, pathParam.Value);
                }
            }
            if (authHeaders?.Count > 0)
            {
                foreach (var authHeader in authHeaders)
                {
                    request.AddHeader(authHeader.Key, authHeader.Value);
                }
            }
            if (headers?.Count > 0)
            {
                foreach (var header in headers)
                {
                    request.AddHeader(header.Key, header.Value);
                }
            }
            if(bodyContent!=null)
            {
                //request.AddJsonBody(bodyContent);
            }
            request.AddParameter("application/json; charset=utf-8", bodyContent, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;
            var response = restClient.Execute(request);

            result.resultCode = (int)response.StatusCode;
            result.message = $"Response Status : {response.ResponseStatus} \n" +
                $"Error Message : {response.ErrorMessage}";
            result.Object = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(response.Content);
            return result;
        }
    }
}
