using Xunit;

namespace Iot.Greenhouse.EntityFrameworkCore;

[CollectionDefinition(GreenhouseTestConsts.CollectionDefinitionName)]
public class GreenhouseEntityFrameworkCoreCollection : ICollectionFixture<GreenhouseEntityFrameworkCoreFixture>
{

}
