using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PharmacyDB.Models
{
    public partial class DrugForm
    {
        public int Id { get; set; }
        public int FormId { get; set; }

        public int DrugId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than zero.")]
        public float Dose { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than zero.")]
        public float Volume { get; set; }
        
        public virtual Drug Drug { get; set; } 

        public virtual Form Form { get; set; }
    }
}
