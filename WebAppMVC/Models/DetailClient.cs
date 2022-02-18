using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppMVC.Models
{
    public class DetailClient
    {
        public Guid id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public Guid Client_id { get; set; }
        [ForeignKey("Client_id")]
        public Client Client { get; set; }
        public List<ShopProduct> Product { get; set; } = new List<ShopProduct>();
    }
}
