namespace PharmacyAdminWebApp.Models
{
    public class FormSelectionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public float Dose { get; set; }
        public float Volume { get; set; }
    }
}
