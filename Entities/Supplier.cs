namespace bageri.api.Entities
{
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string Name { get; set; }

        public IList<SupplierProduct> SupplierProducts { get; set; }
        public ContactInformation ContactInformation { get; set; }
        public SupplierAddress SupplierAddress { get; set; }
        
    }
}