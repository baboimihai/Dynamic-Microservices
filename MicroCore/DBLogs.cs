using MicroCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;

namespace MicroCore
{
    public class DBLogs : DbContext
    {
        
        public DBLogs() : base("Data Source=DESKTOP-FEBPNCC;Initial Catalog=CloudEmployee;Integrated Security=True")//@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MD;Integrated Security=True")
        {

        }
        public DbSet<LogClientCall> LogClientCalls { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DBLogs, MigrateDBConfiguration>());
        }
        public class MigrateDBConfiguration : System.Data.Entity.Migrations.DbMigrationsConfiguration<DBLogs>
        {
            public MigrateDBConfiguration()
            {
                AutomaticMigrationsEnabled = true;
                AutomaticMigrationDataLossAllowed = true;
            }
        }
    }
    public class LogClientCall
    {
        [Key]
        public int Id { get; set; }
        public bool Success { get; set; }
        public DateTime Date { get; set; }
        public string ClientToken { get; set; }
        public string ClientKey { get; set; }
        public string IP { get; set; }
        public string Port { get; set; }
        public string Function { get; set; }
    }
    public static class WriteLog
    {
        public static List<LogClientCall> Calls = new List<LogClientCall>();
        private static DBLogs context = new DBLogs();
        public static void LogClient(MicroClientInfo client, bool success, string function)
        {
            var key = "";
            if (client.ClientKey != null && client.ClientKey.Length > 0)
            {
                key = System.Text.Encoding.ASCII.GetString(client.ClientKey);
            }
            Calls.Add(new LogClientCall
            {
                ClientKey = key,
                ClientToken = client.ClientToken,
                Date = DateTime.Now,
                Function = function,
                IP = client.IP,
                Port = client.Port,
                Success = success
            });
            //context.SaveChanges();
        }
        public static List<LogClientCall> GetLogs()
        {
            return Calls.Where(x => true).OrderByDescending(x => x.Date).Take(20).ToList();
        }
    }

}
