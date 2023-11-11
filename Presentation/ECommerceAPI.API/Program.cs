using ECommerceAPI.Application.Validators.Products;
using ECommerceAPI.Infrastructure;
using ECommerceAPI.Infrastructure.Filters;
using ECommerceAPI.Persistance;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistanceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
            policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddControllers(options=>options.Filters.Add<ValidationFilter>())
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
//Fluent Validation yapýsý burada devreye sokulmus oldu.
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter=true);
//Burada da mevcut olan, asp de default gelen validation yapýlanmasýný bastýr, devre dýsý býrak demis oluyoruz.
//Bundan sonra beným kendi yazacagým kendi filter larýmý devreye sok demis oldum.
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors();
}
app.UseCors();

app.UseStaticFiles();//wwwroot un kullanýlabilmesi icin eklenen bir middleware dir.
app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();
