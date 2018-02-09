using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyController.Data.Model;
using SQLite.CodeFirst;

namespace SpotifyController.Data.Context
{
    public class DatabaseContext : DbContext
    {

        public DbSet<Album> Album { get; set; }
        public DbSet<Artist> Artist { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Song> Song { get; set; }

        public DatabaseContext() : base(
            new SQLiteConnection()
            {
                ConnectionString =
                    new SQLiteConnectionStringBuilder() {DataSource = "D:\\data.sqlite", ForeignKeys = true}
                        .ConnectionString
            }, true) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<DatabaseContext>(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializer);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
