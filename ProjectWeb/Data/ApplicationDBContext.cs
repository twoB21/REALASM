	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectWeb.Models;

namespace ProjectWeb.Data
{
	public class ApplicationDBContext:IdentityDbContext
	{
		public DbSet<Category> Categories { get; set; }
		public DbSet<Book> Books { get; set; }
		public DbSet<test> tests { get; set; }
        public DbSet<JobListing> JobListings { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
		public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

            modelBuilder.Entity<JobListing>()
            .HasOne(j => j.Employer)  // Liên kết với IdentityUser (Employer)
            .WithMany()               // Không cần mối quan hệ một nhiều ngược lại (Employer không cần giữ danh sách JobListings)
            .HasForeignKey(j => j.EmployerId);

            base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Category>().HasData(
				new Category { Id = 1, Name = "Adventure",Description="Give you a feel of challenge",DisplayPriority = 1},
				new Category { Id = 2, Name = "Roman", Description = "So romantique", DisplayPriority = 4 },
				new Category { Id = 3, Name = "Horror", Description = "So scary", DisplayPriority = 3 },
				new Category { Id = 4, Name = "Scifi", Description = "So interesting", DisplayPriority = 2 }
				);
			modelBuilder.Entity<Book>().HasData(
				new Book { Id = 1, Title ="C Programming", Description = "Basic C", Author = "Greenwich",Price = 100, CategoryId = 1 },
				new Book { Id = 2, Title = "Robinhood", Description = "ok", Author = "Who knows", Price = 300, CategoryId = 1 },
				new Book { Id = 3, Title = "Data Structures", Description = "Hard", Author = "FPT", Price = 200, CategoryId =  3},
				new Book { Id = 4, Title = ".NET advanced", Description = "Ok", Author = "StudyGuys", Price = 200, CategoryId = 3 },
				new Book { Id = 5, Title = "Application Development", Description = "You have to learn it", Author = "Greenwich", Price = 100, CategoryId = 2 }
				);
		}
	}
}
