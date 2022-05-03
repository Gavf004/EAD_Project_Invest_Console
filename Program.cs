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
        }

        static async Task GetsAsync()                         // async methods return Task or Task<T>
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5204/api/");

                    // add an Accept header for JSON
                    client.DefaultRequestHeaders.
                        Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));            // or application/xml

                    // GET ../Stocks/all
                    // get all Stocks in DB
                    HttpResponseMessage response = await client.GetAsync("APIStocks");              // async call, await suspends until task finished            
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

                    // Get stock with ID 2
                    Console.WriteLine("Stock 2:");
                    response = await client.GetAsync("APIStocks/2");
                    if (response.IsSuccessStatusCode)
                    {
                        // read results 
                        var stock = await response.Content.ReadAsAsync<Stock>();
                        Console.WriteLine(stock);
                    }
                    else
                    {
                        Console.WriteLine(response.StatusCode + " " + response.ReasonPhrase);
                    }

                    // Test POST to create Stock
                    Console.WriteLine("Add new Stock");
                    Stock s1 = new Stock() { StockName = "Facebook",  StockTicker = "F", StockPrice = 50, SellPrice =0, TotalShares =5, ExchangeName="NASDAQ" };
                    response = await client.PostAsJsonAsync("APIStocks", s1);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Stock {0} added", s1.StockName);
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