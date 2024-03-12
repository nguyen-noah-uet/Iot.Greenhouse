using Volo.Abp.Modularity;

namespace Iot.Greenhouse;

[DependsOn(
    typeof(GreenhouseApplicationModule),
    typeof(GreenhouseDomainTestModule)
)]
public class GreenhouseApplicationTestModule : AbpModule
{

}
