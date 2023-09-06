using PharmacyDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyDB.Interfaces
{
    public interface IDrugRepository : IGenericRepository<Drug>
    {
        public List<Drug> GetAllWithBrands();
        public List<Drug> GetAllWithCategories();
    }
}
