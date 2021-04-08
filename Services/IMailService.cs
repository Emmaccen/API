namespace CityInfo.API.Services
{
    public interface IMailService
    {
        void ActivateMailService(string message, string subject);
    }
}