using BookManagement.Domain.Primitives.Id;
using BookManagement.Presentation;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BookManagement.App.Configurations;

/// <summary>
/// Installs presentation services and configuration.
/// </summary>
public class PresentationServiceInstaller : IServiceInstaller
{
    /// <summary>
    /// Configures the presentation services.
    /// </summary>
    /// <param name="services">The collection of services to configure.</param>
    /// <param name="configuration">The application configuration.</param>
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        // Add controllers and application part
        services
            .AddControllers()
            .AddApplicationPart(AssemblyReference.Assembly);

        // Add OpenAPI (Swagger) support
        services.AddOpenApi();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SchemaFilter<StronglyTypedIdSchemaFilter>();
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "BookManagement API",
                Description = "An ASP.NET Core Web API for managing books and users",
                TermsOfService = new Uri("https://example.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "Example Contact",
                    Url = new Uri("https://example.com/contact")
                },
                License = new OpenApiLicense
                {
                    Name = "Example License",
                    Url = new Uri("https://example.com/license")
                }
            });

            // Add JWT Authentication
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter 'Bearer' [space] and then your token in the text input below.\n\nExample: \"Bearer abcdefghijklmnopqrstuvwxyz\""
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }
}

public sealed class StronglyTypedIdSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var baseType = context.Type.BaseType;
        if (baseType == null) return;

        if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(StronglyTypedId<>))
        {
            schema.Type = "string";
            schema.Format = "int"; // yoki "int"/"string" depending on type
            schema.Properties?.Clear();
        }
    }
}
