using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;

namespace DAL.DB
{
    public partial class EasyImmoContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                              .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                              .AddJsonFile("appsettings.json", true, true)
                              .Build();

            optionsBuilder.UseLazyLoadingProxies()
                          .UseSqlServer(config.GetConnectionString("EasyImmoDBSQLConnectionString"));
        }
    }
}