using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace WheelRecognitionSystem.Public
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        /// <summary>
        /// 异步POST请求（发送JSON数据）- 兼容 C# 7.3
        /// </summary>
        public async Task<TResponse> PostJsonAsync<TRequest, TResponse>(string url, TRequest requestData)
        {
            // 1. 创建HttpRequestMessage（确保可控制整个生命周期）
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(requestData),
                    Encoding.UTF8,
                    "application/json"
                )
            };

            // 2. 使用传统using块确保资源释放
            HttpResponseMessage response = null;
            try
            {
                response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string responseJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResponse>(responseJson);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"HTTP请求失败: {ex.HResult} - {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception($"JSON序列化失败: {ex.Message}", ex);
            }
            finally
            {
                // 3. 手动释放资源
                request?.Dispose();
                response?.Dispose();
            }
        }


    }

    // 使用示例
    public class Program1
    {
        public static async Task Main1(string[] args)
        {
            // 使用传统using块管理HttpClient
            using (var httpClient = new HttpClient())
            {
                var apiClient = new ApiClient(httpClient);

                try
                {
                    var response = await apiClient.PostJsonAsync<LoginRequest, ApiResponse>(
                        "https://api.example.com/login",
                        new LoginRequest { stationNo = "test", guid = "pass" }
                    );

                    Console.WriteLine($"成功: {response.msg}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"失败: {ex.Message}");
                }
            }
        }
    }

    // 示例请求模型
    public class LoginRequest
    {
        public string stationNo;
        public string guid;
    }

    // 示例响应模型
    public class ApiResponse
    {
        public string code ;
        public string success ;
        public string msg ;
        public string data ;
        public DateTime time ;
    }
}
