using System;
using System.Collections.Generic;
using System.Text;
using Iot.Greenhouse.Localization;
using Volo.Abp.Application.Services;

namespace Iot.Greenhouse;

/* Inherit your application services from this class.
 */
public abstract class GreenhouseAppService : ApplicationService
{
    protected GreenhouseAppService()
    {
        LocalizationResource = typeof(GreenhouseResource);
    }
}
