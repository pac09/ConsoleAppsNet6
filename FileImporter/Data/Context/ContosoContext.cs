using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileImporter.Data.Context
{
    public class ContosoContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            optionsBuilder.UseSqlServer(@"Server=ALIENWARE-15R3\MSSQLSERVER2019;Database=ContosoLtd;User Id=sa;Password=Pastor1987;");
        }

        public DbSet<Models.File> Files { get; set; }
        public DbSet<Models.LeadsData> LeadsData { get; set; }
    }
}
