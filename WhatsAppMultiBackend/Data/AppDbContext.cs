using Microsoft.EntityFrameworkCore;
using WhatsAppMultiBackend.Models;

namespace WhatsAppMultiBackend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<WhatsAppSession> WhatsAppSessions => Set<WhatsAppSession>();
    }
}
