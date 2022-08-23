using OneToManyTest.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace OneToManyTest.Permissions;

public class OneToManyTestPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(OneToManyTestPermissions.GroupName);

        myGroup.AddPermission(OneToManyTestPermissions.Dashboard.Host, L("Permission:Dashboard"), MultiTenancySides.Host);
        myGroup.AddPermission(OneToManyTestPermissions.Dashboard.Tenant, L("Permission:Dashboard"), MultiTenancySides.Tenant);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(OneToManyTestPermissions.MyPermission1, L("Permission:MyPermission1"));

        var customerPermission = myGroup.AddPermission(OneToManyTestPermissions.Customers.Default, L("Permission:Customers"));
        customerPermission.AddChild(OneToManyTestPermissions.Customers.Create, L("Permission:Create"));
        customerPermission.AddChild(OneToManyTestPermissions.Customers.Edit, L("Permission:Edit"));
        customerPermission.AddChild(OneToManyTestPermissions.Customers.Delete, L("Permission:Delete"));

        var orderPermission = myGroup.AddPermission(OneToManyTestPermissions.Orders.Default, L("Permission:Orders"));
        orderPermission.AddChild(OneToManyTestPermissions.Orders.Create, L("Permission:Create"));
        orderPermission.AddChild(OneToManyTestPermissions.Orders.Edit, L("Permission:Edit"));
        orderPermission.AddChild(OneToManyTestPermissions.Orders.Delete, L("Permission:Delete"));

        var hobbyPermission = myGroup.AddPermission(OneToManyTestPermissions.Hobbies.Default, L("Permission:Hobbies"));
        hobbyPermission.AddChild(OneToManyTestPermissions.Hobbies.Create, L("Permission:Create"));
        hobbyPermission.AddChild(OneToManyTestPermissions.Hobbies.Edit, L("Permission:Edit"));
        hobbyPermission.AddChild(OneToManyTestPermissions.Hobbies.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<OneToManyTestResource>(name);
    }
}