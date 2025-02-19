namespace bageri.api.Entities
{
    public class Ingredient
    {
        public int IngredientId { get; set; }
        public string ItemNumber { get; set; }
        public string Name { get; set; }
        public decimal PricePerKg { get; set; }

        public IList<SupplierIngredient> SupplierIngredients { get; set; }
    }
}