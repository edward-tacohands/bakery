namespace bageri.api.Entities
{
    public class Address
    {
        public int AddressId { get; set; }
        public string Street { get; set; }
        public int StreetNumber { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
    }
}