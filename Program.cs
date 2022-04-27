using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;


namespace ProjectInvestClient
{
    class Client
    {
        static void Main(string[] args)
        {
            GetsAsync().Wait();
            Console.ReadLine();
        }

        static async Task GetsAsync()                         // async methods return Task or Task<T>
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5204/");

                    // add an Accept header for JSON
                    client.DefaultRequestHeaders.
                        Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));            // or application/xml

                    // GET ../Stocks/all
                    // get all Stocks in DB
                    HttpResponseMessage response = await client.GetAsync("API/Stocks/all");              // async call, await suspends until task finished            
                    if (response.IsSuccessStatusCode)                                                   // 200.299
                    {
                        // read results 
                        var stocks = await response.Content.ReadAsAsync<IEnumerable<Stock>>();
                        foreach (var stock in stocks)
                        {
                            Console.WriteLine(stock);
                        }
                    }
                    else
                    {
                        Console.WriteLine(response.StatusCode + " " + response.ReasonPhrase);
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

    }
}