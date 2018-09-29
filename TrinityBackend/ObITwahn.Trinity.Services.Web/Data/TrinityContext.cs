using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ObITwahn.Services.Meeting.Model;

namespace ObITwahn.Trinity.Services.Web.Data
{
    public class TrinityContext : DbContext
    {
        public TrinityContext() : base()
        { }

        public TrinityContext(DbContextOptions options) : base(options)
        {
        }


        public DbSet<ObITwahn.Services.Meeting.Model.Employee> Emloyees { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Meeting>().HasMany(m => m.Participants);

            modelBuilder.Entity<ObITwahn.Services.Meeting.Model.Employee>().HasMany(e => e.Meetings);

        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ChangeTracker.DetectChanges();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            ChangeTracker.DetectChanges();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            ChangeTracker.DetectChanges();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}