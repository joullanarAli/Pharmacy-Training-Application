using PharmacyDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyDB.Interfaces
{
    public interface IDrugActiveIngredientRepository : IGenericRepository<DrugActiveIngredient>
    {
        public List<DrugActiveIngredient> GetAllWithActiveIngredients();
        public List<DrugActiveIngredient> GetAllWithDrugs();
    }
}
