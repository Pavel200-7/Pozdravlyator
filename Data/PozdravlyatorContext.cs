using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pozdravlyator.Models;

namespace Pozdravlyator.Data
{
    public class PozdravlyatorContext : DbContext
    {
        public PozdravlyatorContext (DbContextOptions<PozdravlyatorContext> options)
            : base(options)
        {
        }

        public DbSet<Pozdravlyator.Models.Person> Person { get; set; } = default!;
    }
}
