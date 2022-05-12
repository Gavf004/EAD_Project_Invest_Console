using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace ProjectInvestClient
{
    class Client
    {
        //public static float stockValue = 0;
        //public static List<String> tickers = new List<string>();
        //public static List<float> number_of_shares = new List<float>();
        public static float stockValue;

        static void Main(string[] args)
        {
            GetsAsync().Wait();
            GetsAsyncUsers().Wait();
            
        }

        static async Task GetsAsync()                         // async methods return Task or Task<T>
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    //client.BaseAddress = new Uri("http://localhost:5204/api/"); //Uncomment to use local server
                    client.BaseAddress = new Uri("https://eadcaprojectinvest20220510165713.azurewebsites.net/api/"); 

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
                            //tickers.Add(stock.StockName);
                            //.Add(stock.TotalShares);
                            string price = await GetStockPrice(stock.StockTicker);
                            Console.WriteLine("The current stock price is: {0} $", price);
                            stockValue += stock.TotalShares * float.Parse(price);

                        }
                        Console.WriteLine("The portfolio value is: {0} $\n", stockValue);
                        
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
                    Stock s1 = new Stock() { StockName = "Ford",  StockTicker = "F", StockPrice = 50, SellPrice =0, TotalShares =5, ExchangeName="NASDAQ" };
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
           





        }



        static async Task GetsAsyncUsers()                         // async methods return Task or Task<T>
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    //client.BaseAddress = new Uri("http://localhost:5204/api/"); //Uncomment to use local server
                    client.BaseAddress = new Uri("https://eadcaprojectinvest20220510165713.azurewebsites.net/api/"); // Uncomment to use Azure

                    // add an Accept header for JSON
                    client.DefaultRequestHeaders.
                        Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));            // or application/xml

                    // GET ../Users/all
                    // get all Users in DB
                    HttpResponseMessage response = await client.GetAsync("APIUser");              // async call, await suspends until task finished            
                    if (response.IsSuccessStatusCode)                                                   // 200.299
                    {
                        // read results 
                        var Users = await response.Content.ReadAsAsync<IEnumerable<User>>();
                        foreach (var user in Users)
                        {
                            Console.WriteLine(user);

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
        
            // Method to access Yahoo api and retrieve regular market price.
            public static async Task<String> GetStockPrice(string name)
        {
            string Name = name;
            string base1 = "/v6/finance/quote?symbols=";
            string queryString = base1+Name;
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
                    queryString);
                    //"/v6/finance/quote?symbols={Name}"
                    //"v11/finance/quoteSummary/AAPL?lang=en&region=US&modules=defaultKeyStatistics%2CassetProfile");

                    var responseBody = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {

                        dynamic stockInfos = JObject.Parse(responseBody);
                        //Console.WriteLine(stockInfos);
                        string currentStockPrice = stockInfos.quoteResponse.result[0].regularMarketPrice;
                        return currentStockPrice;
                        //Console.WriteLine("The current stock price is: {0} $", currentStockPrice);

                    }


                    else
                    {
                        Console.WriteLine(response.StatusCode + " " + response.ReasonPhrase);
                        return null;
                    }




                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }


    }
}