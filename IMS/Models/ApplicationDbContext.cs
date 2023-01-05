using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMF.Models
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //TODO, hard code the db path for now, can add into app.Config for different path for different env dev/uat/prod etc
            optionsBuilder.UseSqlite(@"Data Source=D:\git\IMS\DB\IMS.db");

        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Symbol> Symbols { get; set; }

    }
}
