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
    public class DrugFormRepository : GenericRepository<DrugForm>,IDrugFormRepository
    {
        public DrugFormRepository(PharmacyDbContext context) : base(context)
        {

        }
        public List<DrugForm> GetAllWithForms()
        {
            return _context.DrugForms.Include(e => e.Form)
                .ToList();
        }
        public List<DrugForm> GetAllWithDrugs()
        {
            return _context.DrugForms.Include(e => e.Drug)
                .ToList();
        }
        
    }
}
