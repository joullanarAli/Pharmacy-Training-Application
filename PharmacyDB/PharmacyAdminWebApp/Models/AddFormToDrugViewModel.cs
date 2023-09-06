using PharmacyDB.Models;

namespace PharmacyAdminWebApp.Models
{


    public class AddFormToDrugViewModel
    {
        public int DrugId { get; set; }
        public int SelectedFormId { get; set; }
        public float Dose { get; set; }
        public float Volume { get; set; }
        public List<Form> AvailableForms { get; set; }
    }
    
}
