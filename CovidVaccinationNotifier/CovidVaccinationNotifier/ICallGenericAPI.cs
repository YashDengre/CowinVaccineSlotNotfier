using RestSharp;
using System.Collections.Generic;

namespace CovidVaccinationNotifier
{
    public interface ICallGenericAPI
    {
        Result<T> CallAPI<T>(string url, 
            Method method, 
            object bodyContent, 
            Dictionary<string, string> pathParams = null, 
            Dictionary<string, string> queryParams = null, 
            Dictionary<string, string> authHeaders = null, 
            Dictionary<string, string> headers = null, 
            string resource = "");
    }
}