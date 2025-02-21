using BookManagement.App.Configurations;
using BookManagement.App.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Install services from assemblies implementing IServiceInstaller
builder.Services
    .InstallServices(
        builder.Configuration,
        typeof(IServiceInstaller).Assembly);

// Configure Serilog for logging
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// Map OpenAPI endpoints in development environment
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use HTTPS redirection
app.UseHttpsRedirection();

// Use Serilog request logging
app.UseSerilogRequestLogging();

// Use authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

// Register the global exception handling middleware in the request processing pipeline
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

// Map controllers to route endpoints
app.MapControllers();

// Run the application
app.Run();