using PharmacyDB.Interfaces;
using PharmacyDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyInfrastructure.Repositories
{
    public class ChoiceRepository : GenericRepository<Choice>, IChoiceRepository
    {
        public ChoiceRepository(PharmacyDbContext context) : base(context)
        {

        }
    }
}
