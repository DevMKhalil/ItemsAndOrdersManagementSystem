using CSharpFunctionalExtensions;
using ItemsAndOrdersManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ItemsAndOrdersManagementSystem.Aplication
{
    public interface IAppDbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
