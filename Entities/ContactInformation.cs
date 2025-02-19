namespace bageri.api.Entities
{
    public class ContactInformation
    {
        public int ContactInformationId { get; set; }
        public string ContactPerson { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public SupplierContactInformation SupplierContactInformation { get; set; }
        public CustomerContactInformation CustomerContactInformation { get; set; }
    }
}