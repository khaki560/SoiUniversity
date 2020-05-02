using System.Data.Entity;

namespace server.Models.Messages
{
    class MessagesContext : DbContext
    {
        public MessagesContext(string connectionString) : base(connectionString)
        {
        }

        public DbSet<MessageEntry> Entries { get; set; }
    }
}