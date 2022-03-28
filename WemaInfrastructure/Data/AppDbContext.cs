using Microsoft.EntityFrameworkCore;
using WemaCore.Entities;

namespace WemaInfrastructure
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}

		public DbSet<Customer> Customers { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlite("Data Source=WemaApp.db");
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

		}
	}
}
