using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Music_Portal.Models
{
    public class MusicPortalContext : DbContext
    {
        public MusicPortalContext(DbContextOptions<MusicPortalContext> options) : base(options)
        {
            if (Database.EnsureCreated())
            {
                Styles.Add(new Style { Name = "Рок" });
                Styles.Add(new Style { Name = "Поп" });
                Styles.Add(new Style { Name = "Метал" });
                Users.Add(new User { Name = "Andrey", Surname = "Kyrpa", Login = "andrey", Email = "andrey@gmail.com", Access_Level = 0, Password = "andrey" });
                Users.Add(new User { Name = "admin", Surname = "admin", Login = "admin", Email = "admin@gmail.com", Access_Level = 1, Password = "admin" });
                SaveChanges();
                SaveChanges();
            }
        }
        public DbSet<Style> Styles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Song> Songs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // метод UseLazyLoadingProxies() делает доступной ленивую загрузку.
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
