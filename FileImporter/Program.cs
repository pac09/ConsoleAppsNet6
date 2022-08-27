using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FileImporter.Workers;
using FileImporter.Data.Context;

public class Program 
{
    public static void Main(string[] args) 
    {
        var host = CreateDefaultBuilder().Build();

        using IServiceScope serviceScope = host.Services.CreateScope();
        IServiceProvider provider = serviceScope.ServiceProvider;

        var workerInstance = provider.GetRequiredService<Importer>();
        workerInstance.Import();

        host.Run();
    }

    private static IHostBuilder CreateDefaultBuilder() 
    {
        return Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(app =>
            {
                app.AddJsonFile("appsettings.json");
            })
            .ConfigureServices(services => 
            {
                services.AddScoped<Importer>();
                services.AddDbContext<ContosoContext>();
            });
    }
}

