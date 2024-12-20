using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using user_auth.Models;

namespace user_auth.Data
{
    public class UserAuthContext : DbContext
    {
        public UserAuthContext (DbContextOptions<UserAuthContext> options)
            : base(options)
        {
        }

        public DbSet<user_auth.Models.Movie> Movie { get; set; } = default!;
    }
}
