using ECommerceAPI.Application.Validators.Products;
using ECommerceAPI.Infrastructure;
using ECommerceAPI.Infrastructure.Filters;
using ECommerceAPI.Infrastructure.Services.Storage.Azure;
using ECommerceAPI.Infrastructure.Services.Storage.Local;
using ECommerceAPI.Persistance;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistanceServices();
builder.Services.AddInfrastructureServices();

//builder.Services.AddStorage<LocalStorage>();
//Burada IStorage talep ettigimde bunun gelmesini saglýyorum. Baska bir storage verirsem o gelir. 
//Tek satýr kod uzerinden tum bunlarý duzenleyebilirim.
builder.Services.AddStorage<AzureStorage>();



//builder.Services.AddStorage(ECommerceAPI.Infrastructure.Enums.StorageType.Local);
//builder.Services.AddStorage(ECommerceAPI.Infrastructure.Enums.StorageType.Azure);
//builder.Services.AddStorage(ECommerceAPI.Infrastructure.Enums.StorageType.AWS);
//Bu sekilde bir talep etme islemi de yapýyor olabilirim. Burada da bir enum yapýsý kullanmýs oldum.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
            policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddControllers(options=>options.Filters.Add<ValidationFilter>())
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter=true);
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors();
}
app.UseCors();

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();
