using Iot.Greenhouse.Samples;
using Xunit;

namespace Iot.Greenhouse.EntityFrameworkCore.Domains;

[Collection(GreenhouseTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<GreenhouseEntityFrameworkCoreTestModule>
{

}
