namespace WebAppMVC.Models
{
    public class Client
    {
        public Guid id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public List<DetailClient> DetailClients { get; set; } = new List<DetailClient>();
        
    }
}
