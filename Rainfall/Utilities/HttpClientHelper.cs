using System.Text;
using System.Text.Json;
using System.IO.Compression;
using Rainfall.Models;
using System;
using System.Net.Http;
using Microsoft.Extensions.ObjectPool;

namespace Rainfall.Utilities
{
    public class HttpClientHelper
    {
        public static async Task<HttpResponseMessage> GetAsync(string url)
        {
            // Create a new HttpClient instance
            using HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(0.1);

            try
            {
                // Send a GET request to the specified URL
                HttpResponseMessage response = client.GetAsync(url).Result;

                // Ensure the request was successful
                response.EnsureSuccessStatusCode();
                // Output the response
                return response;
            }
            catch (HttpRequestException e)
            {
                // Handle any request exceptions
                return null;
            }
        }

    }
}
