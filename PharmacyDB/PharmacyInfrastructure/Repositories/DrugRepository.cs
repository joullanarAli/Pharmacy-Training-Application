using Microsoft.EntityFrameworkCore;
using PharmacyDB.Interfaces;
using PharmacyDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyInfrastructure.Repositories
{
    public class DrugRepository : GenericRepository<Drug>, IDrugRepository
    {
        public DrugRepository(PharmacyDbContext context) : base (context)
        {

        }
        public  List<Drug> GetAllWithBrands()
        {
            return _context.Drugs.Include(e => e.Brand)
                .ToList();
        }
        public  List<Drug> GetAllWithCategories()
        {
            return _context.Drugs.Include(e => e.Category)
                .ToList();
        }
    }
}
