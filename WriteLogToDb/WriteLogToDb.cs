
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;

namespace WriteLogToDb
{
    public class WriteLogToDb : DbContext
    {

        public WriteLogToDb() : base(@"Data Source=tcp:den1.mssql6.gear.host,1433;Initial Catalog=dinamicmicrodemo;User ID=dinamicmicrodemo;Password=Xr0s!~21ISQb;Connection Timeout=3000;")
        {

        }
        public DbSet<LogClientCall> LogClientCallTests { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<WriteLogToDb, MigrateDBConfiguration>());
        }
        public class MigrateDBConfiguration : System.Data.Entity.Migrations.DbMigrationsConfiguration<WriteLogToDb>
        {
            public MigrateDBConfiguration()
            {
                AutomaticMigrationsEnabled = true;
                AutomaticMigrationDataLossAllowed = true;
            }
        }
    }


}
