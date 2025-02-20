namespace BookManagement.App.Configurations;

/// <summary> 
/// Defines a contract for installing services into the IServiceCollection. 
/// </summary>
public interface IServiceInstaller
{
    /// <summary> 
    /// Installs services into the IServiceCollection. 
    /// </summary> 
    /// <param name="services">The IServiceCollection to add services to.</param> 
    /// <param name="configuration">The IConfiguration for configuration settings.</param>
    void Install(IServiceCollection services, IConfiguration configuration);
}