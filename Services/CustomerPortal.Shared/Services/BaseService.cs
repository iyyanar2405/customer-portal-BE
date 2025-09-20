using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace CustomerPortal.Shared.Services
{
    /// <summary>
    /// Base service for inter-service communication
    /// </summary>
    public abstract class BaseService
    {
        protected readonly HttpClient _httpClient;
        protected readonly ILogger _logger;

        protected BaseService(HttpClient httpClient, ILogger logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Generic GET request
        /// </summary>
        protected async Task<T?> GetAsync<T>(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }

                _logger.LogWarning($"GET request to {endpoint} failed with status code: {response.StatusCode}");
                return default(T);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while making GET request to {endpoint}");
                throw;
            }
        }

        /// <summary>
        /// Generic POST request
        /// </summary>
        protected async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(endpoint, content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<TResponse>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }

                _logger.LogWarning($"POST request to {endpoint} failed with status code: {response.StatusCode}");
                return default(TResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while making POST request to {endpoint}");
                throw;
            }
        }

        /// <summary>
        /// Generic PUT request
        /// </summary>
        protected async Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync(endpoint, content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<TResponse>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }

                _logger.LogWarning($"PUT request to {endpoint} failed with status code: {response.StatusCode}");
                return default(TResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while making PUT request to {endpoint}");
                throw;
            }
        }

        /// <summary>
        /// Generic DELETE request
        /// </summary>
        protected async Task<bool> DeleteAsync(string endpoint)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(endpoint);
                
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                _logger.LogWarning($"DELETE request to {endpoint} failed with status code: {response.StatusCode}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while making DELETE request to {endpoint}");
                throw;
            }
        }
    }
}