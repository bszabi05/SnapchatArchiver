using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SnapchatArchiver
{
    public static class Downloader
    {
        private static readonly HttpClient _client;
        static Downloader()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
            _client.Timeout = TimeSpan.FromMinutes(5);
        }
        public static async Task<byte[]> DownloadAsync(string url)
        {
            var response = await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync();
        }

        public static async Task<string> GetContentTypeAsync(string url)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Head, url))
            {
                var response = await _client.SendAsync(request);
                return response.Content.Headers.ContentType?.MediaType ?? "";
            }
        }

        public static void SaveToFile(string path, byte[] data)
        {
            if (File.Exists(path)) {
                File.Delete(path);
            }
             File.WriteAllBytes(path, data);
            
            
        }

    }
}
