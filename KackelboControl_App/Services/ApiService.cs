using KackelboControl_App.Models.Application;
using System.Diagnostics;
using System.Text.Json;

namespace KackelboControl_App.Services;

public interface IApiService
{
    Task<SensorTriggers> GetSensorTriggers();
}
public class ApiService : IApiService
{

    HttpClient _client;
    JsonSerializerOptions _serializerOptions;
    private string url = "https://kackelbocontrolapi.azurewebsites.net/Application/";

    public ApiService()
    {
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        _client = new HttpClient();
        _client.BaseAddress = new Uri(url);
        _client.DefaultRequestHeaders.Add("ApiKey", "");
    }

    public async Task<SensorTriggers> GetSensorTriggers()
    {
        Uri uri = new Uri($"{url}sensorTriggers");
        var sensorTriggers = new SensorTriggers();
        try
        {

            HttpResponseMessage response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                sensorTriggers = JsonSerializer.Deserialize<SensorTriggers>(content, _serializerOptions);
            }
            return sensorTriggers;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
            throw;
        }

    }
}

