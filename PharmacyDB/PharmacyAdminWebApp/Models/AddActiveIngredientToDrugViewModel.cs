using PharmacyDB.Models;

namespace PharmacyAdminWebApp.Models
{
    public class AddActiveIngredientToDrugViewModel
    {
        public int DrugId { get; set; }
        public int SelectedActiveIngredientId { get; set; }
        public List<ActiveIngredient> AvailableActiveIngredients{ get; set; }
    }
}
