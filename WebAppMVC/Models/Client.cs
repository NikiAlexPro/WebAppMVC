using System.ComponentModel.DataAnnotations;

namespace WebAppMVC.Models
{
    public class Client
    {
        public Guid id { get; set; }
        [Required(ErrorMessage = "Не указано Имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Не указана Фамилия")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Не указано Отчество")]
        public string Patronymic { get; set; }
        public List<DetailClient> DetailClients { get; set; } = new List<DetailClient>();
        public bool Compare(Client client)
        {
            if (FirstName == client.FirstName &&
                LastName == client.LastName &&
                Patronymic == client.Patronymic)
                return true;
            else
                return false;
        }
    }
}
