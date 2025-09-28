
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;
using System.Reflection;

namespace Ordering.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    DbSet<Customer> Customers => Set<Customer>();
    DbSet<Product> Products => Set<Product>();
    DbSet<Order> Orders => Set<Order>();
    DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}
