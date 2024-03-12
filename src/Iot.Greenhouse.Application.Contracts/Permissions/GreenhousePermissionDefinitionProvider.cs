using Iot.Greenhouse.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Iot.Greenhouse.Permissions;

public class GreenhousePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(GreenhousePermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(GreenhousePermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<GreenhouseResource>(name);
    }
}
