namespace bageri.api.Entities
{
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string Name { get; set; }

        public IList<SupplierProduct> SupplierProducts { get; set; }
    }
}