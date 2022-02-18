using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppMVC.Models
{
    public class ShopProduct
    {
        public Guid id { get; set; }
        public DateTime BuyDate { get; set; }
        public string CheckAmmount { get; set; }
        public byte[] ScreenImage { get; set; }
        public string ImageFormat  { get; set; }
        public Guid DetailClient_id { get; set; }
        [ForeignKey("DetailClient_id")]
        public DetailClient DetailClient { get; set; }
    }
}
