using bageri.api;
using bageri.api.Data;
using bageri.api.Interfaces;
using bageri.api.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options => {
    options.UseSqlite(builder.Configuration.GetConnectionString("DevConnection"));
});

builder.Services.AddScoped<IProductRepository, ProductRepositories>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IProductPreparationRepository, ProductPreparationRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IContactInformationRepository, ContactInformationRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if(app.Environment.IsDevelopment()){
    app.UseSwagger();
    app.UseSwaggerUI();
}

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.LoadIngredients(context);
    await Seed.LoadSuppliers(context);
    await Seed.LoadSupplierIngredients(context);
    await Seed.LoadAddressTypes(context);
    await Seed.LoadPostalAddresses(context);
    await Seed.LoadAddresses(context);
    await Seed.LoadContactInformations(context);
    await Seed.LoadSupplierContactInformations(context);
    await Seed.LoadSupplierAddresses(context);
    await Seed.LoadCustomers(context);
    await Seed.LoadCustomerContactInformations(context);
    await Seed.LoadCustomerAddresses(context);
    await Seed.LoadOrders(context);
    await Seed.LoadProducts(context);
    await Seed.LoadOrderProducts(context);
    await Seed.LoadCustomerOrders(context);
    await Seed.LoadProductPreparations(context);

}catch(Exception ex){
    Console.WriteLine("{0}", ex.Message);
}

app.MapControllers();

app.Run();
