namespace bageri.api.Entities
{
    public class SupplierIngredient
    {
        public int IngredientId { get; set; }
        public int SupplierId { get; set; }

        public Ingredient Ingredient { get; set; }
        public Supplier Supplier { get; set; }
    }
}