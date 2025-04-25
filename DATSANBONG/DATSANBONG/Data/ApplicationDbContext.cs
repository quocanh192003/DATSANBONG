using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DATSANBONG.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace DATSANBONG.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }   
    }
}
