global using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MailService.MinWebAPI
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<MailMessage> MailMessages => Set<MailMessage>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MailMessage>()
                        .Property(e => e.Recipients)
                        .HasConversion(
                            v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                            v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null),
                            new ValueComparer<ICollection<string>>(
                                (c1, c2) => c1.SequenceEqual(c2),
                                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                                c => (ICollection<string>)c.ToList()));
        }
    }
}