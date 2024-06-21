using Microsoft.EntityFrameworkCore;
using DR_Lista.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DR_Lista.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<ShoppingItem> ShoppingItems { get; set; }
        public DbSet<ShoppingList> ShoppingLists { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
