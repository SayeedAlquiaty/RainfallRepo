using System.Text;
using System.Text.Json;
using System.IO.Compression;
using Rainfall.Models;
using System;
using System.Net.Http;

namespace Rainfall.Utilities
{
    public class HttpClientHelper
    {
        public static async Task<string> GetAsync(string url)
        {
            // Create a new HttpClient instance
            using HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(10);


            try
            {
                // Send a GET request to the specified URL
                HttpResponseMessage response = client.GetAsync(url).Result;

                // Ensure the request was successful
                response.EnsureSuccessStatusCode();
                
                //var buffer = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                //var destination = Decompress(response);
                //response.Content = new ByteArrayContent(destination);
                
                // Read the response content as a string
                var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                

                // Output the response body
                return responseBody;
            }
            catch (HttpRequestException e)
            {
                // Handle any request exceptions
                return $"Request exception: {e.Message}";
            }
        }

        static async  Task<byte[]> Decompress(HttpResponseMessage response)
        {
            //var data = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            using (var outputStream = new MemoryStream())
            {
                using (var inputStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                {
                    using (var brotliStream = new BrotliStream(inputStream, CompressionMode.Decompress))
                    {
                        brotliStream.CopyTo(outputStream);
                    }
                }
                return outputStream.ToArray();
            }
        }

        static async Task<byte[]> BrotliDecompress(HttpResponseMessage response)
        {
            var data = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            var destination = new byte[data.Length];
            int bytesRead = 0;
            int bytesWriten = 0;
            using (var bd = new BrotliDecoder())
            {
                var status = bd.Decompress(data, destination, out bytesRead, out bytesWriten);
            }
            return destination;
        }
    }
}
