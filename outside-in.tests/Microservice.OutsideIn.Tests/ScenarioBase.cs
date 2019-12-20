using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microservice.OutsideIn.Tests
{
   public abstract class ScenarioBase
   {
      protected const string GuidRegex = "([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}";
      
      protected readonly string TenantID = Guid.NewGuid().ToString();

      protected async Task<(HttpStatusCode statusCode, string location)> PostAsync(object body, string url)
      {
         var json = JsonConvert.SerializeObject(body);
         var request = new HttpRequestMessage(HttpMethod.Post, MakeUri(url))
         {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
         };

         request.Headers.Add("X-Tenant", TenantID);

         using (var httpClient = new HttpClient(IgnoreSslCertificateHandler()))
         {
            var response = await httpClient.SendAsync(request);

            return (response.StatusCode, response.Headers.Location?.ToString());
         }
      }

      protected async Task<(HttpStatusCode statusCode, string content)> GetAsync(string url)
      {
         var request = new HttpRequestMessage(HttpMethod.Get, MakeUri(url));

         request.Headers.Add("X-Tenant", TenantID);

         using (var httpClient = new HttpClient(IgnoreSslCertificateHandler()))
         {
            var response = await httpClient.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
            
            return (response.StatusCode, content);
         }
      }

      protected Uri BaseUri
      {
         get
         {
            var uri = Environment.GetEnvironmentVariable("APP_URI");

            if (string.IsNullOrWhiteSpace(uri))
            {
               uri ="https://localhost:5001/";
            }

            return new Uri(uri);
         }
      }

      private Uri MakeUri(string url)
      {
         var uri = new Uri(url, UriKind.RelativeOrAbsolute);

         if (uri.IsAbsoluteUri)
         {
            return uri;
         }
         
         return new Uri(BaseUri, uri);
      }
      
      private static HttpClientHandler IgnoreSslCertificateHandler()
      {
         return new HttpClientHandler {ServerCertificateCustomValidationCallback = (request, certificate, chain, policyErrors) => true};
      }
   }
}