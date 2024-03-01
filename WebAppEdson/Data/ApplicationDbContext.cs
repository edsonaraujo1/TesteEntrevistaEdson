using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAppEdson.Models;

namespace WebAppEdson.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Seguro> Seguro { get; set; }

        public DbSet<Usuario> Usuario { get; set; }

        public DbSet<ContatoEmail> ContatoEmails { get; set; }
    }
}
