using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ProjectInvestClient
{
    public class Stock
    {
        [Key]
        [DisplayName("Stock ID")]
        public int StockID { get; set; }
        [Required(ErrorMessage = "Stock Name is required")]
        [DisplayName("Stock Name")]
        public string StockName { get; set; }
        [Required(ErrorMessage = "Stock Ticker is required")]
        [DisplayName("Stock Ticker ")]
        public string StockTicker { get; set; }
        [Required(ErrorMessage = "Stock Price is required")]
        [DisplayName("Purchase Price (€)")]
        public float StockPrice { get; set; }
        [Required(ErrorMessage = "Sell Price is required")]
        [DisplayName("Sell Price (€)")]
        public float SellPrice { get; set; }
        [Required(ErrorMessage = "Total Shares is required")]
        [DisplayName("Total Shares")]
        public float TotalShares { get; set; }
        [Required(ErrorMessage = "Exchange Name is required")]
        [DisplayName("Exhange Name")]
        public string ExchangeName { get; set; }

        //navigation properties

        public virtual ICollection<Order> Order { get; set; }

    }
}
