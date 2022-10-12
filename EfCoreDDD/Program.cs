using EfCoreDDD.Application;
using EfCoreDDD.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddScoped<MyDbContext, MyDbContext>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IDbDataInitializer, DbDataInitializer>();
    })
    .Build();

using IServiceScope initScope = host.Services.CreateScope();
{
    await initScope.ServiceProvider.GetRequiredService<IDbDataInitializer>().AddData();
}

using IServiceScope activateScope = host.Services.CreateScope();
{
    await activateScope.ServiceProvider.GetRequiredService<IUserService>().ActivateAccounts();
}

await host.RunAsync();
Console.ReadLine();