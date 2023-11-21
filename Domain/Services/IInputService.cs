namespace Domain.Services
{
    public interface IInputService
    {
        IEnumerable<string> GetCities(string[] args, IEnumerable<string> apiCities);
    }
}