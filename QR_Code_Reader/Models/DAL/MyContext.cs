using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QR_Code_Reader.Models.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Code_Reader.Models.DAL
{
    public class MyContext : IdentityDbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        public DbSet<UserCovidTest> UserCovidTests { get; set; }
    }
}
