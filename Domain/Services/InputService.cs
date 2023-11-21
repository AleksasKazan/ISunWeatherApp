using Microsoft.Extensions.Logging;

namespace Domain.Services
{
    public class InputService(ILogger<InputService> logger) : IInputService
    {
        public IEnumerable<string> GetCities(string[] args, IEnumerable<string> apiCities)
        {
            logger.LogInformation("Getting cities with args: {Args}", string.Join(", ", args));

            if (args.Length == 0)
            {
                return apiCities;
            }
            else
            {
                if (args[0] != "--cities")
                {
                    throw new ArgumentException("Usage: isun.exe --cities Vilnius, Kaunas, Klaipėda");
                }
                else
                {
                    var inputCities = ResolveArgs(args);

                    var citiesNotInApi = inputCities.Except(apiCities).ToList();

                    if (citiesNotInApi.Count != 0)
                    {
                        throw new ArgumentException("Cities not found in API: " + string.Join(", ", citiesNotInApi));
                    }

                    return inputCities;
                }
            }
        }

        private static IEnumerable<string> ResolveArgs(string[] args)
        {
            foreach (var city in args.Skip(1))
            {
                yield return city.TrimEnd(',');
            }
        }
    }
}