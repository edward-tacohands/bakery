namespace bageri.api.Entities
{
    public class Address
    {
        public int AddressId { get; set; }
        public int PostalAddressId { get; set; }
        public int AddressTypeId { get; set; }
        public string AddressLine { get; set; }
        public PostalAddress PostalAddress { get; set; }
        public AddressType AddressType { get; set; }
        public IList<SupplierAddress> SupplierAddresses { get; set; }
        public IList<CustomerAddress> CustomerAddresses { get; set; }

    }
}