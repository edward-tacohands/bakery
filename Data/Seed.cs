using System.Text.Json;
using bageri.api.Entities;
namespace bageri.api.Data;

    public static class Seed
    {
        private static readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static async Task LoadIngredients(DataContext context){
            if(context.Ingredients.Any()) return;

            var json = File.ReadAllText("Data/json/ingredients.json");
            var ingredients = JsonSerializer.Deserialize<List<Ingredient>>(json, _options);

            if(ingredients != null && ingredients.Count > 0){
                await context.Ingredients.AddRangeAsync(ingredients);
                await context.SaveChangesAsync();
            }
        }

        public static async Task LoadSuppliers(DataContext context){
            if(context.Suppliers.Any()) return;

            var json = File.ReadAllText("Data/json/suppliers.json");
            var suppliers = JsonSerializer.Deserialize<List<Supplier>>(json, _options);
            if (suppliers != null && suppliers.Count > 0){
                await context.Suppliers.AddRangeAsync(suppliers);
                await context.SaveChangesAsync();
            }
        }

        public static async Task LoadCustomers(DataContext context){
            if(context.Customers.Any()) return;

            var json = File.ReadAllText("Data/json/customers.json");
            var customers = JsonSerializer.Deserialize<List<Customer>>(json, _options);
            if (customers != null && customers.Count > 0){
                await context.Customers.AddRangeAsync(customers);
                await context.SaveChangesAsync();
            }
        }
        public static async Task LoadContactInformations(DataContext context){
            if(context.ContactInformations.Any()) return;

            var json = File.ReadAllText("Data/json/contactinformation.json");
            var contactInformations = JsonSerializer.Deserialize<List<ContactInformation>>(json, _options);
            if (contactInformations != null && contactInformations.Count > 0){
                await context.ContactInformations.AddRangeAsync(contactInformations);
                await context.SaveChangesAsync();
            }
        }        
        public static async Task LoadAddresses(DataContext context){
            if(context.Addresses.Any()) return;

            var json = File.ReadAllText("Data/json/addresses.json");
            var addresses = JsonSerializer.Deserialize<List<Address>>(json, _options);
            if (addresses != null && addresses.Count > 0){
                await context.Addresses.AddRangeAsync(addresses);
                await context.SaveChangesAsync();
            }
        }

        public static async Task LoadAddressTypes(DataContext context){
            if(context.AddressTypes.Any()) return;

            var json = File.ReadAllText("Data/json/addresstypes.json");
            var at = JsonSerializer.Deserialize<List<AddressType>>(json, _options);
            if (at != null && at.Count > 0){
                await context.AddressTypes.AddRangeAsync(at);
                await context.SaveChangesAsync();
            }
        }

        public static async Task LoadPostalAddresses(DataContext context){
            if(context.PostalAddresses.Any()) return;
            var json = File.ReadAllText("Data/json/postaladdresses.json");
            var pa = JsonSerializer.Deserialize<List<PostalAddress>>(json, _options);

            if(pa != null && pa.Count > 0){
                await context.PostalAddresses.AddRangeAsync(pa);
                await context.SaveChangesAsync();
            }
        }

        public static async Task LoadProducts(DataContext context){
            if(context.Products.Any()) return;
            var json = File.ReadAllText("Data/json/products.json");
            var p = JsonSerializer.Deserialize<List<Product>>(json, _options);

            if(p != null && p.Count > 0){
                await context.Products.AddRangeAsync(p);
                await context.SaveChangesAsync();
            }
        }
        public static async Task LoadProductPreparations(DataContext context){
            if(context.ProductPreparations.Any()) return;
            var json = File.ReadAllText("Data/json/productpreparations.json");
            var pp = JsonSerializer.Deserialize<List<ProductPreparation>>(json, _options);

            if(pp != null && pp.Count > 0){
                await context.ProductPreparations.AddRangeAsync(pp);
                await context.SaveChangesAsync();
            }
        }
        public static async Task LoadOrders(DataContext context){
            if(context.Orders.Any()) return;
            var json = File.ReadAllText("Data/json/orders.json");
            var o = JsonSerializer.Deserialize<List<Order>>(json, _options);

            if(o != null && o.Count > 0){
                await context.Orders.AddRangeAsync(o);
                await context.SaveChangesAsync();
            }
        }        

        /********************************************************************/

        public static async Task LoadSupplierIngredients(DataContext context){
            if(context.SupplierIngredients.Any()) return;

            var json = File.ReadAllText("Data/json/supplieringredients.json");
            var supplierIngredients = JsonSerializer.Deserialize<List<SupplierIngredient>>(json, _options);
            if (supplierIngredients != null && supplierIngredients.Count > 0){
                await context.SupplierIngredients.AddRangeAsync(supplierIngredients);
                await context.SaveChangesAsync();
            }
        }

        public static async Task LoadSupplierAddresses(DataContext context){
            if(context.SupplierAddresses.Any()) return;

            var json = File.ReadAllText("Data/json/supplieraddresses.json");
            var supplierAddresses = JsonSerializer.Deserialize<List<SupplierAddress>>(json, _options);
            if (supplierAddresses != null && supplierAddresses.Count > 0){
                await context.SupplierAddresses.AddRangeAsync(supplierAddresses);
                await context.SaveChangesAsync();
            }
        }

        public static async Task LoadCustomerContactInformations(DataContext context){
            if(context.CustomerContactInformations.Any()) return;

            var json = File.ReadAllText("Data/json/customercontactinformation.json");
            var customerContactInformations = JsonSerializer.Deserialize<List<CustomerContactInformation>>(json, _options);
            if (customerContactInformations != null && customerContactInformations.Count > 0){
                await context.CustomerContactInformations.AddRangeAsync(customerContactInformations);
                await context.SaveChangesAsync();
            }
        }

        public static async Task LoadSupplierContactInformations(DataContext context){
            if(context.SupplierContactInformations.Any()) return;

            var json = File.ReadAllText("Data/json/suppliercontactinformation.json");
            var supplierContactInformations = JsonSerializer.Deserialize<List<SupplierContactInformation>>(json, _options);
            if (supplierContactInformations != null && supplierContactInformations.Count > 0){
                await context.SupplierContactInformations.AddRangeAsync(supplierContactInformations);
                await context.SaveChangesAsync();
            }
        }

        public static async Task LoadCustomerAddresses(DataContext context){
            if(context.CustomerAddresses.Any()) return;

            var json = File.ReadAllText("Data/json/customeraddresses.json");
            var customerAddresses = JsonSerializer.Deserialize<List<CustomerAddress>>(json, _options);
            if (customerAddresses != null && customerAddresses.Count > 0){
                await context.CustomerAddresses.AddRangeAsync(customerAddresses);
                await context.SaveChangesAsync();
            }
        }

        public static async Task LoadOrderProducts(DataContext context){
            if(context.OrderProducts.Any()) return;

            var json = File.ReadAllText("Data/json/orderproducts.json");
            var op = JsonSerializer.Deserialize<List<OrderProduct>>(json, _options);
            if (op != null && op.Count > 0){
                await context.OrderProducts.AddRangeAsync(op);
                await context.SaveChangesAsync();
            }
        }

        public static async Task LoadCustomerOrders(DataContext context){
            if(context.CustomerOrders.Any()) return;

            var json = File.ReadAllText("Data/json/customerorders.json");
            var co = JsonSerializer.Deserialize<List<CustomerOrder>>(json, _options);
            if (co != null && co.Count > 0){
                await context.CustomerOrders.AddRangeAsync(co);
                await context.SaveChangesAsync();
            }
        }

    }
