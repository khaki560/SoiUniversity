using System.Data.Entity;

namespace server.Models.Users
{
    class UsersContext : DbContext
    {
        public UsersContext(string connectionString) : base(connectionString)
        {
        }

        public DbSet<UserEntry> Entries { get; set; }
    }
}