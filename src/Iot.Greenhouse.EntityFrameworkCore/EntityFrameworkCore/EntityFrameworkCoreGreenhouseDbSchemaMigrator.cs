using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Iot.Greenhouse.Data;
using Volo.Abp.DependencyInjection;

namespace Iot.Greenhouse.EntityFrameworkCore;

public class EntityFrameworkCoreGreenhouseDbSchemaMigrator
    : IGreenhouseDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreGreenhouseDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the GreenhouseDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<GreenhouseDbContext>()
            .Database
            .MigrateAsync();
    }
}
