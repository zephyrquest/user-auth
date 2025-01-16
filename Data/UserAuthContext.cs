using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserAuth.Models;

namespace UserAuth.Data
{
    public class UserAuthContext : DbContext
    {
        public UserAuthContext (DbContextOptions<UserAuthContext> options)
            : base(options)
        {
        }

        public DbSet<UserAuth.Models.Movie> Movie { get; set; } = default!;
    }
}
