using PharmacyDB.Interfaces;
using PharmacyDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyInfrastructure.Repositories
{
    public class BrandRepository : GenericRepository<Brand>, IBrandRepository
    {
        public BrandRepository(PharmacyDbContext context): base(context)
        {

        }
    }
}
