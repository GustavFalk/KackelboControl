using KackelboControl_App.Models;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;

namespace KackelboControl_App.Services;

public interface IApiService
{
    Task<int?> GetEggCount(DateOnly forDate);
    Task<List<EggCount>> GetEggCountLog();
    Task<SensorTriggers> GetSensorTriggers();
    Task<SensorValueHistory> GetSensorValueHistory(DateTime forDate);
    Task PostEggCount(EggCount eggCount);
    Task<SensorTriggers> UpdateSensorTriggers(UpdateSensorTriggers updateSensorTriggers);
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
            PropertyNameCaseInsensitive = true,
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

    public async Task<SensorTriggers> UpdateSensorTriggers(UpdateSensorTriggers updateSensorTriggers)
    {
        try
        {
            Uri uri = new Uri($"{url}sensorTriggers");
            SensorTriggers sensorTriggers = new SensorTriggers();
            
            var response = await _client.PostAsJsonAsync(uri, updateSensorTriggers);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                sensorTriggers = JsonSerializer.Deserialize<SensorTriggers>(responseContent, _serializerOptions);
                return sensorTriggers ?? new();
            }
            throw new Exception($"Failed when posting sensorvalues to server. ErrorCode: {response.StatusCode}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
            throw;
        }
    }

    public async Task<SensorValueHistory> GetSensorValueHistory(DateTime forDate)
    {

        Uri uri = new Uri($"{url}sensorValueHistory?fordate={forDate}");
        var sensorHistory = new SensorValueHistory();
        try
        {
            HttpResponseMessage response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                sensorHistory = JsonSerializer.Deserialize<SensorValueHistory>(content, _serializerOptions);
            }
            return sensorHistory;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
            throw;
        }
    }

    public async Task<int?> GetEggCount(DateOnly forDate)
    {
        Uri uri = new Uri($"{url}eggCount?fordate={forDate}");

        try
        {
            HttpResponseMessage response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<int>(content, _serializerOptions);
            }
            return null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
            throw;
        }
    }

    public async Task<List<EggCount>> GetEggCountLog()
    {
        Uri uri = new Uri($"{url}eggCountLog");
        List<EggCount>? eggCountLog = new List<EggCount>();
        try
        {
            HttpResponseMessage response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                eggCountLog = JsonSerializer.Deserialize<List<EggCount>>(content, _serializerOptions);
            }
            return eggCountLog;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
            throw;
        }
    }

    public async Task PostEggCount(EggCount eggCount)
    {
        Uri uri = new Uri($"{url}eggCount");
        try
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync(uri, eggCount);
            if (response.IsSuccessStatusCode)
            {
                return;
            }
            throw new Exception($"Failed when posting eggcount to server. ErrorCode: {response.StatusCode}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
            throw;
        }
    }
}

