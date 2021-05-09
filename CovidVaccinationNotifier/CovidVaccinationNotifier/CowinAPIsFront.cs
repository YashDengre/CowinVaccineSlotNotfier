using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CovidVaccinationNotifier
{
    public class CowinAPIsFront : ICowinAPIsFront
    {
        private string URL { get; set; }
        private string findByPinResource { get; set; }
        private string findByDistrictResource { get; set; }



        public CowinAPIsFront()
        {
            URL = ConfigurationManager.AppSettings["COWIN-URL"];
            findByPinResource = ConfigurationManager.AppSettings["FIND-BY-PIN"];
            findByDistrictResource = ConfigurationManager.AppSettings["FIND-BY-DISTRICT"];


        }

        public CowinVaccineSlotResponse FindByPin(string pincode, string date)
        {

            var cowinVaccineSlotResonse = new CowinVaccineSlotResponse();
            var restClient = new RestClient(URL);
            var request = new RestRequest(Method.GET);
            request.Resource = findByPinResource;
            request.AddQueryParameter("pincode", pincode, true);
            request.AddQueryParameter("date", date, true);
            var response = restClient.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                cowinVaccineSlotResonse = Newtonsoft.Json.JsonConvert.DeserializeObject<CowinVaccineSlotResponse>(response.Content);
            }
            return cowinVaccineSlotResonse;

        }

        public CowinVaccineSlotResponse FindByDistrict(string districtId, string date)
        {

            var cowinVaccineSlotResonse = new CowinVaccineSlotResponse();
            var restClient = new RestClient(URL);
            var request = new RestRequest(Method.GET);
            request.Resource = findByDistrictResource;
            request.AddQueryParameter("district_id", districtId, true);
            request.AddQueryParameter("date", date, true);
            var response = restClient.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                cowinVaccineSlotResonse = Newtonsoft.Json.JsonConvert.DeserializeObject<CowinVaccineSlotResponse>(response.Content);
            }
            return cowinVaccineSlotResonse;

        }
    }
}
