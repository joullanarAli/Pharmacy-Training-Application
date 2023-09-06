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
    public class DrugActiveIngredientRepository : GenericRepository<DrugActiveIngredient>,IDrugActiveIngredientRepository
    {
        public DrugActiveIngredientRepository(PharmacyDbContext context) : base(context)
        {

        }
        public List<DrugActiveIngredient> GetAllWithActiveIngredients()
        {
            return _context.DrugActiveIngredients.Include(e => e.ActiveIngredient)
                .ToList();
        }
        public List<DrugActiveIngredient> GetAllWithDrugs()
        {
            return _context.DrugActiveIngredients.Include(e => e.Drug)
                .ToList();
        }
    }
}
