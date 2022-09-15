// See https://aka.ms/new-console-template for more information

using CosmosTablePoc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((_, configApp) =>
    {
        configApp.SetBasePath(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).FullName);
        configApp.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        configApp.AddUserSecrets<Program>();
    })
    .ConfigureServices((hostContext, services) => {
        services.AddLogging(configure => configure.AddConsole());

        services.AddTransient<PersonWorker>();
    })
    .Build();

await DoWork(host.Services);
await host.RunAsync();

static async Task DoWork(IServiceProvider serviceProvider)
{
    await using (var serviceScope = serviceProvider.CreateAsyncScope())
    {
        var services = serviceScope.ServiceProvider;

        try
        {
            var workerStub = services.GetRequiredService<PersonWorker>();
            await workerStub.RunAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}
