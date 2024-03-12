using Iot.Greenhouse.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Iot.Greenhouse.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class GreenhouseController : AbpControllerBase
{
    protected GreenhouseController()
    {
        LocalizationResource = typeof(GreenhouseResource);
    }
}
