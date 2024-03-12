using Volo.Abp.Modularity;

namespace Iot.Greenhouse;

[DependsOn(
    typeof(GreenhouseDomainModule),
    typeof(GreenhouseTestBaseModule)
)]
public class GreenhouseDomainTestModule : AbpModule
{

}
