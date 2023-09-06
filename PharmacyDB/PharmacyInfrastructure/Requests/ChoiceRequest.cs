using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyInfrastructure.Requests
{
    public class ChoiceRequest
    {
        public int Id { get; set; }
        public string ChoiceText { get; set; }=null!;
        public bool Score { get; set; }
    }
}
