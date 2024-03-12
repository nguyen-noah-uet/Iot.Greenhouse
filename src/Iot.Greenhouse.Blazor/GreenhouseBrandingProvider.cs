using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Iot.Greenhouse.Blazor;

[Dependency(ReplaceServices = true)]
public class GreenhouseBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "Greenhouse";
}
