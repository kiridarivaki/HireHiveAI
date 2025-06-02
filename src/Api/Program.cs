using HireHive.Infrastructure.Startup;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;


configuration.ConfigureKeyVault(configuration);
builder.Logging.ConfigureLogging(configuration);
services.ConfigureDependencyInjection(configuration);
services.ConfigureIdentity(configuration);
services.ConfigureSwagger();
services.AddAuthorization();
services.ConfigureAuthentication(configuration);
services.ConfigureHangfire(configuration);
services.ConfigureCors();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.EnableSwagger();
    app.EnableHangfire();
    app.EnableCors();
    app.UseDeveloperExceptionPage();
}

app.Services.Seed(configuration);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
