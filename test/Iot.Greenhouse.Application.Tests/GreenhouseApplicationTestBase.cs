using Volo.Abp.Modularity;

namespace Iot.Greenhouse;

public abstract class GreenhouseApplicationTestBase<TStartupModule> : GreenhouseTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
