using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using testeBitzen.Models;

namespace testeBitzen.Data
{
    public class DataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            DotNetEnv.Env.Load();
            var server = System.Environment.GetEnvironmentVariable("SERVER");
            var database = System.Environment.GetEnvironmentVariable("DATABASE");
            var user = System.Environment.GetEnvironmentVariable("USER");
            var password = System.Environment.GetEnvironmentVariable("PASSWORD");
            optionsBuilder.UseSqlServer(
                @"Server=" + server + ";Database=" + database + ";User Id=" + user + ";Password=" + password + ";");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Abastecimento> Abastecimentos { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }


    }
}
