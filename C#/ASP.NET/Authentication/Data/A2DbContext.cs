using Microsoft.EntityFrameworkCore;
public class A2DbContext : DbContext
{
    public A2DbContext(DbContextOptions<A2DbContext> options) : base(options) { }
    public DbSet<Users> Users { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Organizer> Organizers { get; set; }

    public DbSet<Product> Products { get; set; }
}