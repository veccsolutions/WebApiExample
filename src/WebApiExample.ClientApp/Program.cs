using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApiExample.Contracts;

namespace WebApiExample.ClientApp
{
    public class Program
    {
        public void Main(string[] args)
        {
            QueryApi(new Query
            {
                Filters = new FilterBase[]
                {
                        new Filter1 { FilterString = "hi" },
                        new Filter2 { DifferentFilterString = "bye" }
                }
            }).Wait();
        }

        public async Task<QueryResult> QueryApi(Query query)
        {
            var clientHandler = new HttpClientHandler();

            //comment or remove the proxy lines if you don't have a local proxy to see the requests
            clientHandler.Proxy = new WebProxy { Address = new Uri("http://localhost:8888"), BypassProxyOnLocal = false };
            clientHandler.UseProxy = true;



            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri("http://desktop.veccsolutions.org:5554");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonFormatter = new JsonMediaTypeFormatter();
                jsonFormatter.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
                var content = new ObjectContent<Query>(query, jsonFormatter);
                var httpRequestMessage = new HttpRequestMessage
                {
                    Content = content,
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("http://desktop.veccsolutions.org:5554/Account"),
                    Version = HttpVersion.Version11
                };

                //posted json data looks like this:
                //{"Filters":[{"$type":"WebApiExample.Contracts.Filter1, WebApiExample.Contracts","FilterString":"hi"},{"$type":"WebApiExample.Contracts.Filter2, WebApiExample.Contracts","DifferentFilterString":"bye"}]}

                var response = await client.SendAsync(httpRequestMessage);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<QueryResult>();
                }
            }

            return null;
        }
    }
}
