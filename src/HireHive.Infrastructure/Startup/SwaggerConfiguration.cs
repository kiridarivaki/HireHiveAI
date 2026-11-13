using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json.Nodes;

namespace HireHive.Infrastructure.Startup;

public static class SwaggerConfiguration
{
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            });

            options.SchemaFilter<EnumValueSchemaFilter>();
        });
        services.AddSwaggerGen();
    }

    public static void EnableSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(o => o.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"));
    }
}

public class EnumValueSchemaFilter : ISchemaFilter
{
    public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum && schema is OpenApiSchema openApiSchema)
        {
            var enumValues = Enum.GetValues(context.Type);
            openApiSchema.Enum = [.. enumValues
                .Cast<object>()
                .Select(v => (JsonNode)JsonValue.Create(Convert.ToInt32(v)))];
            openApiSchema.Type = JsonSchemaType.Integer;
            openApiSchema.Format = "int32";
            // Add enum value descriptions to the schema description
            var enumDescriptions = enumValues
                .Cast<object>()
                .Select(v => $"{Convert.ToInt32(v)} = {Enum.GetName(context.Type, v)}");
            openApiSchema.Description = $"Enum values: {string.Join(", ", enumDescriptions)}";
        }
    }
}
