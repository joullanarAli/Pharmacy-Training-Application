using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PharmacyDB.Models
{
    public partial class DrugActiveIngredient
    {
        
        public int Id { get; set; }
        public int DrugId { get; set; }
        public int ActiveIngredientId { get; set; }

        public virtual ActiveIngredient ActiveIngredient { get; set; } = null!;


        public virtual Drug Drug { get; set; } = null!;
    }
}
