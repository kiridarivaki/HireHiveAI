using Azure.Identity;
using HireHive.Infrastructure.Data;
using HireHive.Infrastructure.Startup;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

if (builder.Environment.IsDevelopment())
{
    var keyVaultURL = configuration.GetSection("AzureKeyVault:KeyVaultURL").Value;
    var keyVaultClientId = configuration.GetSection("AzureKeyVault:ClientId").Value;
    var keyVaultClientSecret = configuration.GetSection("AzureKeyVault:ClientSecret").Value;
    var keyVaultDirectoryID = configuration.GetSection("AzureKeyVault:DirectoryID").Value;

    var credential = new ClientSecretCredential(keyVaultDirectoryID!.ToString(), keyVaultClientId!.ToString(), keyVaultClientSecret!.ToString());
    builder.Configuration.AddAzureKeyVault(
        new Uri(keyVaultURL!),
        credential);
}

builder.Logging.ConfigureLogging(configuration);
services.ConfigureDependencyInjection(configuration);



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

// CORS configuration - add later
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: "angularApp", configurePolicy: policyBuilder =>
//    {
//        policyBuilder.WithOrigins("https://localhost:5000");
//        policyBuilder.AllowAnyHeader();
//        policyBuilder.AllowAnyMethod();
//        policyBuilder.AllowCredentials();
//    });
//});

builder.Services.AddControllers();


var app = builder.Build();

//app.UseCors("angularApp");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        var initializer = scope.ServiceProvider.GetRequiredService<Initialiser>();

        await initializer.InitialiseDatabaseAsync();

        await initializer.SeedDatabaseAsync();
    }
}


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
