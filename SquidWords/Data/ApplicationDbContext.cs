using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SquidWords.Models;
using Pomelo.EntityFrameworkCore.MySql;

namespace SquidWords.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Word> Words { get; set; }
        public DbSet<PersonalWord> PersonalWords { get; set; }
        public DbSet<Dictionary> Dictionaries { get; set; }
        public DbSet<PersonalDictionary> PersonalDictionaries { get; set; }
        public DbSet<Language> Languages { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("connections.json");
            var config = builder.Build();
            string connectionString = config.GetConnectionString("ReleaseConnection");

            optionsBuilder.UseMySql(connectionString);
        }

    }
}
