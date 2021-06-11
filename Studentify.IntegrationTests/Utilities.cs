using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Mime;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Studentify.Models.HttpBody;
using Studentify.IntegrationTests.GetDto;

namespace Studentify.IntegrationTests
{
    class Utilities
    {
        public static async Task Register(HttpClient client, RegisterDto user)
        {
            using var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("api/authenticate/register", UriKind.Relative),
                Content = new StringContent(
                    JsonSerializer.Serialize(user),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json)
            };

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public static async Task<LoginGetDto> Login(HttpClient client, LoginDto user)
        {
            using var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("api/authenticate/login", UriKind.Relative),
                Content = new StringContent(
                    JsonSerializer.Serialize(user),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json)
            };

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<LoginGetDto>(
                body, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public static async Task<HttpResponseMessage> SendAuthorizedRequest<DtoType>(
            HttpClient client,
            LoginGetDto loginData,
            HttpMethod method,
            string uri,
            DtoType dto)
        {
            using var request = new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri(uri, UriKind.Relative),
                Content = new StringContent(JsonSerializer.Serialize(dto),
                                            Encoding.UTF8,
                                            MediaTypeNames.Application.Json)
            };
            request.Headers.Add("Authorization", "Bearer " + loginData.Token);
            return await client.SendAsync(request);
        }

        public static async Task<HttpResponseMessage> SendAuthorisedNoBodyRequest(
            HttpClient client,
            LoginGetDto loginData,
            HttpMethod method,
            string uri)
        {
            using var request = new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri(uri, UriKind.Relative)
            };
            request.Headers.Add("Authorization", "Bearer " + loginData.Token);
            return await client.SendAsync(request);
        }

        public static async Task<T> Deserialize<T>(HttpResponseMessage response)
        {
            var body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(
                body, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
    }
}
