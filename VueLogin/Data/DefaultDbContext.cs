using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VueLogin.Data
{
    public class DefaultDbContext : DbContext 
    {

        public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options)
        {

        }

    }
}
