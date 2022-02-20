using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebAppMVC.Models
{
    public class DetailClientListViewModel
    {
        public ShopProduct shopProduct { get; set; }
        public SelectList DetailClients { get; set;}
    }
}
