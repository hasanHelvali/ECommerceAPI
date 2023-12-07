using ECommerceAPI.API.Extensions;
using ECommerceAPI.Application;
using ECommerceAPI.Application.Validators.Products;
using ECommerceAPI.Infrastructure;
using ECommerceAPI.Infrastructure.Filters;
using ECommerceAPI.Infrastructure.Services.Storage.Azure;
using ECommerceAPI.Infrastructure.Services.Storage.Local;
using ECommerceAPI.Persistance;
using ECommerceAPI.SignalR;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Security.Claims;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistanceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();
builder.Services.AddStorage<LocalStorage>();
//builder.Services.AddStorage<AzureStorage>();
//builder.Services.AddStorage(ECommerceAPI.Infrastructure.Enums.StorageType.Local);
//builder.Services.AddStorage(ECommerceAPI.Infrastructure.Enums.StorageType.Azure);
//builder.Services.AddStorage(ECommerceAPI.Infrastructure.Enums.StorageType.AWS);
builder.Services.AddEndpointsApiExplorer();



builder.Services.AddSignalRServices();

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
            policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials()));

builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>())
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

#region SeriLog Configuration
SqlColumn sqlColumn = new SqlColumn();
sqlColumn.ColumnName = "UserName";
sqlColumn.DataType = System.Data.SqlDbType.NVarChar;
sqlColumn.PropertyName = "UserName";
sqlColumn.DataLength = 50;
sqlColumn.AllowNull = true;
ColumnOptions columnOpt = new ColumnOptions();
columnOpt.Store.Remove(StandardColumn.Properties);
columnOpt.Store.Add(StandardColumn.LogEvent);
columnOpt.AdditionalColumns = new Collection<SqlColumn> { sqlColumn };

Logger logger = new LoggerConfiguration()
    .WriteTo.Console()//Console a loglama yapar.
    .WriteTo.File("logs/log.txt")//Bir dosyaya loglama yap demis oluyorum. log/logs.txt dosyasýný olustur ve ilgili loglarý buraya yaz.
    .WriteTo.MSSqlServer(//MSSql veritabanýna loglama yap.
        builder.Configuration.GetConnectionString("SqlServerConnections"),//loglama yapacagý veri tabanýnýn connection string i bu sekildedir.
        sinkOptions: new MSSqlServerSinkOptions
        {
            AutoCreateSqlTable = true,
            TableName = "logs"
        },
        appConfiguration: null,
        columnOptions: columnOpt)
     .Enrich.FromLogContext()
    .Enrich.With<CustomUserNameColumn>()
    .MinimumLevel.Information()
    .CreateLogger();
builder.Host.UseSerilog(logger);
/*Bu service i cagýrdýgýmýz zaman build in gelen log mekanizmasý ezilmis oldu. Artýk buradaki yapýyý kullanacaz.
Burada tanýmlanan serilog yapýsý yukarýda logger olarak veridigm butun konfigurasyonel yapýlandýrmalarý logger
nesnesi ile benimsemis olur.*/

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});
//Yapýlan Request lerin log mekanizmasý ile yakalanmasý gerekiyor. Burada ilgili yakalanacak request lerin konfigürasyonunu yaptýk.
#endregion

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin", options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
            LifetimeValidator = (notbefore, expires, securityToken, valdiationParameters) => expires != null ? expires > DateTime.UtcNow : false,
            NameClaimType=ClaimTypes.Name
        };
    });
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseCors();
}

app.ConfigureExceptionHandler<Program>(app.Services.GetRequiredService<ILogger<Program>>());
app.UseStaticFiles();


app.UseSerilogRequestLogging();//NBu middleware kendisinden onceki middleware leri loglatmaz, sonrasýný loglatýr.
//Haliyle loglanmasýný istedigimiz middleware lerin ustune koyuyoruz.
app.UseHttpLogging();//Artýk yapýlan request lerde log mekanizmasý ile yakalanabilmektedir.
app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    var userName = context.User?.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name : null;
    //LogContext.PushProperty("user_name",userName);elastic
    await next();
});

app.MapControllers();
app.MapHubs();
app.Run();
public class CustomUserNameColumn : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var (username, value) = logEvent.Properties.FirstOrDefault(x => x.Key == "UserName");
        if (value != null)
        {
            var getValue = propertyFactory.CreateProperty(username, value);
            logEvent.AddPropertyIfAbsent(getValue);
        }
    }
}