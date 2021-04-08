namespace CityInfo.API.Services
{
    public interface ILocalMailService
    {
        void ActivateMailService(string message, string subject);
    }
}