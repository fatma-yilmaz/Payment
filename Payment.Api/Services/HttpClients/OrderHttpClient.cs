using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Payment.Api.Data.HttpClients
{
    public class OrderHttpClient
    {
        private readonly HttpClient _httpClient;

        public OrderHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public virtual async Task<CreateOrderResponse> CreateOrder(string consumerFullName, string consumerAddress, CancellationToken cancellationToken)
        {
            try
            {
                var requestObject = new
                {
                    ConsumerFullName = consumerFullName,
                    ConsumerAddress = consumerAddress
                };

                var json = JsonSerializer.Serialize(requestObject);
                var url = "api/order";
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };

                var response = await _httpClient.SendAsync(httpRequest, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    return new CreateOrderResponse() { isSuccess = false};
                }

                var jsonResult = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(jsonResult))
                {
                    return new CreateOrderResponse() { isSuccess = false };
                }

                var createdResponse = JsonSerializer.Deserialize<CreateOrderResponse>(jsonResult);

                return createdResponse;
            }
            catch (Exception ex)
            {
                return new CreateOrderResponse() { isSuccess = false };
            }
        }

        public virtual async Task<GetOrderResponse> GetOrder(Guid orderId, CancellationToken cancellationToken)
        {
            var result = new GetOrderResponse();

            var url = string.Empty;
            try
            {
                url = $"api/order/" + orderId;
                var response = await _httpClient.GetAsync(url, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    return result;
                }

                var jsonResult = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(jsonResult))
                {
                    return result;
                }

                var getOrderResponse = JsonSerializer.Deserialize<GetOrderResponse>(jsonResult);

                return getOrderResponse;
            }
            catch (Exception ex)
            {
                return result;
            }
        }
    }


    public class CreateOrderResponse
    {
        public bool isSuccess { get; set; }
        public Guid orderId { get; set; }

    }

    public class GetOrderResponse
    {
        public Guid id { get; set; }
        public string consumerFullName { get; set; }
        public string consumerAddress { get; set; }

    }
}
