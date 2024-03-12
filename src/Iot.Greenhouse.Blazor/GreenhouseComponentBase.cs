using Iot.Greenhouse.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Iot.Greenhouse.Blazor;

public abstract class GreenhouseComponentBase : AbpComponentBase
{
    protected GreenhouseComponentBase()
    {
        LocalizationResource = typeof(GreenhouseResource);
    }
}
