using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using POE_p2_s4.Models;
using System.Reflection.Emit;

namespace POE_p2_s4.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<User> Users {  get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>()
                .HasDiscriminator<string>("UserType")
                .HasValue<Lecturer>("Lecturer")
                .HasValue<ProgrammeCo_ordinator>("ProgrammeCo_ordinator")
                .HasValue<HR>("HR")
                .HasValue<AcademicManager>("AcademicManager")
            .HasValue<Admin>("Admin");
           
        }

    }
}
