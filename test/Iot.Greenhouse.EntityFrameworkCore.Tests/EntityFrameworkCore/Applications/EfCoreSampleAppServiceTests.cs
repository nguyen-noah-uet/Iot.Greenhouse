using Iot.Greenhouse.Samples;
using Xunit;

namespace Iot.Greenhouse.EntityFrameworkCore.Applications;

[Collection(GreenhouseTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<GreenhouseEntityFrameworkCoreTestModule>
{

}
