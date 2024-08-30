
using API_NEW.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace API_NEW.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) 
        {


        }

        public DbSet<ApplicationUser> ApplicationUsers  { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }


    }
}
