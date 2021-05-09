namespace CovidVaccinationNotifier
{
    public interface ICowinAPIsFront
    {
        CowinVaccineSlotResponse FindByPin(string pincode, string date);
        CowinVaccineSlotResponse FindByDistrict(string districtId, string date);
    }
}