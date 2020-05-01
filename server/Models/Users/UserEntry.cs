using System.ComponentModel.DataAnnotations;

namespace server.Models.Users
{
    public class UserEntry
    {
        [Key]
        public string Name { get; set; }
        public string Pass { get; set; }
        public string PubKey { get; set; }
    }
}