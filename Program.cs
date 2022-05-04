using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

                    // Delete stock with ID 4
                    Console.WriteLine("Deleting Stock with id 11");
                    response = await client.DeleteAsync("APIStocks/11");
                    if (response.IsSuccessStatusCode)
                    {
                        // Print delete was a success
                        Console.WriteLine("Successfully deleted Stock with id 11");
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

            // Print stock current price
            
            try
            {
                
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("https://yfapi.net/");
                    httpClient.DefaultRequestHeaders.Add("X-API-KEY",
                        "ZFQ4vi02Lh8CgPerjwv6C2ETLvRr6F8i3EIonNXH");
                    httpClient.DefaultRequestHeaders.Add("accept",
                        "application/json");

                    var response = await httpClient.GetAsync(
                    "/v6/finance/quote?symbols=AAPL");
                    //"v11/finance/quoteSummary/AAPL?lang=en&region=US&modules=defaultKeyStatistics%2CassetProfile");
                    
                    var responseBody = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)                                                   
                    {

                        dynamic stockInfos = JObject.Parse(responseBody);
                        string currentStockPrice = stockInfos.quoteResponse.result[0].ask;
                        Console.WriteLine("The current stock price is: {0} $",currentStockPrice);

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