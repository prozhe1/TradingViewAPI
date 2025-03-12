using System;
using System.Security.Cryptography;
using System.Text;
using RestSharp;
using System.Net;

namespace TradingViewAPI
{
    public class APIClient
    {
        private static readonly string PrivateKey = Environment.GetEnvironmentVariable("TRADINGVIEW_PRIVATE_KEY") 
            ?? throw new InvalidOperationException("Private key is missing.");
        private static readonly string Token = Environment.GetEnvironmentVariable("TRADINGVIEW_TOKEN") 
            ?? throw new InvalidOperationException("Token is missing.");
        private const string BaseUrl = "https://tradingview.sourcearena.ir/bourse-data/";

        private static string GenerateHmacKey()
        {
            string text = $"{Token}::{DateTime.UtcNow.Hour}";
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(PrivateKey)))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(text));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        public static string GetSymbolData(string symbol, string interval)
        {
            try
            {
                string key = GenerateHmacKey();
                string url = $"{BaseUrl}{key}?symbol={symbol}&interval={interval}";

                var client = new RestClient();
                var request = new RestRequest(url, Method.Get);

                request.AddHeader("Authorization", $"Bearer {Token}");

                RestResponse response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return response.Content ?? "No data received.";
                }
                else
                {
                    return $"خطا: {response.StatusCode} - {response.Content}";
                }
            }
            catch (Exception ex)
            {
                return $"خطای داخلی: {ex.Message}";
            }
        }
    }
}
