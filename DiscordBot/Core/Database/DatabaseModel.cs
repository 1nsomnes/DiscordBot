using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;


namespace DiscordBot.Core.Database
{
    public class DatabaseModel : DbContext
    {

        public DatabaseModel()
        {
            this.Database.EnsureCreated();
        }

        public DbSet<Infraction> Infractions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=infractions.db");
        }
    }

    public class Infraction
    {
        public ulong id { get; set; }
        public ulong userId { get; set; }

        public string severity { get; set; }
        public string description { get; set; }

        public string creationDate { get; set; }
        public string modificationDate { get; set; }

        public ulong modId { get; set; }
    }
}
