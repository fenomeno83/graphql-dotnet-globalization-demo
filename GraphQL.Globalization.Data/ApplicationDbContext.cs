using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQL.Globalization.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public static implicit operator ApplicationDbContext(Func<ApplicationDbContext> v)
        {
            throw new NotImplementedException();
        }
    }
}
