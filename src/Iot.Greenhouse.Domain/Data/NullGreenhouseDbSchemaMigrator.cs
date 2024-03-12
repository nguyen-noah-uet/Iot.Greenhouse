using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Iot.Greenhouse.Data;

/* This is used if database provider does't define
 * IGreenhouseDbSchemaMigrator implementation.
 */
public class NullGreenhouseDbSchemaMigrator : IGreenhouseDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
