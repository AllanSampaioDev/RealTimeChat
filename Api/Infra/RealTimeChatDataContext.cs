namespace Api.Infra;

public class RealTimeChatDataContext(DbContextOptions<RealTimeChatDataContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Domain.Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Domain.Message>().ToTable("Messages");
    }
}
