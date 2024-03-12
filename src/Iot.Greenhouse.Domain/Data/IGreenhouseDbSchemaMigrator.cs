using System.Threading.Tasks;

namespace Iot.Greenhouse.Data;

public interface IGreenhouseDbSchemaMigrator
{
    Task MigrateAsync();
}
