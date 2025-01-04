using System.Text.Json;
using bageri.api.Entities;
namespace bageri.api.Data;

    public static class Seed
    {
        public static async Task LoadProducts(DataContext context){
            var options = new JsonSerializerOptions{
                PropertyNameCaseInsensitive = true
            };

            if(context.Products.Any()) return;

            var json = File.ReadAllText("Data/json/products.json");
            var products = JsonSerializer.Deserialize<List<Product>>(json, options);

            if(products != null && products.Count > 0){
                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
            }
        }

        public static async Task LoadSuppliers(DataContext context){
            var options = new JsonSerializerOptions{
                PropertyNameCaseInsensitive = true
            };

            if(context.Suppliers.Any()) return;

            var json = File.ReadAllText("Data/json/suppliers.json");
            var suppliers = JsonSerializer.Deserialize<List<Supplier>>(json, options);
            if (suppliers != null && suppliers.Count > 0){
                await context.Suppliers.AddRangeAsync(suppliers);
                await context.SaveChangesAsync();
            }
        }

        public static async Task LoadSupplierProducts(DataContext context){
            var options = new JsonSerializerOptions{
                PropertyNameCaseInsensitive = true
            };

            if(context.SupplierProducts.Any()) return;

            var json = File.ReadAllText("Data/json/supplierproducts.json");
            var supplierProducts = JsonSerializer.Deserialize<List<SupplierProduct>>(json, options);
            if (supplierProducts != null && supplierProducts.Count > 0){
                await context.SupplierProducts.AddRangeAsync(supplierProducts);
                await context.SaveChangesAsync();
            }
        }

        public static async Task LoadAddresses(DataContext context){
            var options = new JsonSerializerOptions{
                PropertyNameCaseInsensitive = true
            };

            if(context.Addresses.Any()) return;

            var json = File.ReadAllText("Data/json/addresses.json");
            var addresses = JsonSerializer.Deserialize<List<Address>>(json, options);
            if (addresses != null && addresses.Count > 0){
                await context.Addresses.AddRangeAsync(addresses);
                await context.SaveChangesAsync();
            }
        }

        public static async Task LoadContactInformations(DataContext context){
            var options = new JsonSerializerOptions{
                PropertyNameCaseInsensitive = true
            };

            if(context.ContactInformations.Any()) return;

            var json = File.ReadAllText("Data/json/contactinformation.json");
            var contactInformations = JsonSerializer.Deserialize<List<ContactInformation>>(json, options);
            if (contactInformations != null && contactInformations.Count > 0){
                await context.ContactInformations.AddRangeAsync(contactInformations);
                await context.SaveChangesAsync();
            }
        }
    }
