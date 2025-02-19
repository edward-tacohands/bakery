namespace bageri.api.Entities
{
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string Name { get; set; }

        public IList<SupplierIngredient> SupplierIngredients { get; set; }
        public IList<SupplierAddress> SupplierAddresses { get; set; }
        public SupplierContactInformation SupplierContactInformation { get; set; }
    }
}