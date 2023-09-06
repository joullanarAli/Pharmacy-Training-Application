using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyInfrastructure
{
    public class UserManagerResponse
    {
        public string Message { get; set; } = null!;
        public bool IsSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; } = null!;
        public DateTime? ExpireDate { get; set; }
    }
}
