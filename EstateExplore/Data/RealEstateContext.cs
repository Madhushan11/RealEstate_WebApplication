using EstateExplore.Models;
using Microsoft.EntityFrameworkCore;

namespace EstateExplore.Data
{
    public class RealEstateContext: DbContext
    {
        public RealEstateContext(DbContextOptions<RealEstateContext> options)
            : base(options)
        {
        }

        public DbSet<EstateExplore.Models.Property> Property { get; set; } = default!;
        public DbSet<EstateExplore.Models.User> Users { get; set; } = default!;
        public DbSet<EstateExplore.Models.Price> Purchases { get; set; } = default!;
        public DbSet<EstateExplore.Models.VisitingCustomer> VisitingCustomers { get; set; } = default!;

    }
}
