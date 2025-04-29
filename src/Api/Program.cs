using HireHive.Infrastructure.Startup;

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

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.EnableSwagger();
    app.UseDeveloperExceptionPage();
}


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
