using Localization.Resources.AbpUi;
using OneToManyTest.Localization;
using Volo.Abp.Account;
using Volo.Abp.AuditLogging;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.LanguageManagement;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Saas.Host;
using Volo.Abp.Localization;
using Volo.Abp.SettingManagement;
using Volo.Abp.TextTemplateManagement;
using Volo.Abp.Gdpr;
using Volo.Abp.OpenIddict;
using Volo.FileManagement;

namespace OneToManyTest;

 [DependsOn(
    typeof(OneToManyTestApplicationContractsModule),
    typeof(AbpIdentityHttpApiModule),
    typeof(AbpPermissionManagementHttpApiModule),
    typeof(AbpFeatureManagementHttpApiModule),
    typeof(AbpSettingManagementHttpApiModule),
    typeof(AbpAuditLoggingHttpApiModule),
    typeof(AbpOpenIddictProHttpApiModule),
    typeof(AbpAccountAdminHttpApiModule),
    typeof(LanguageManagementHttpApiModule),
    typeof(SaasHostHttpApiModule),
    typeof(AbpGdprHttpApiModule),
    typeof(AbpAccountPublicHttpApiModule),
    typeof(TextTemplateManagementHttpApiModule)
    )]
[DependsOn(typeof(FileManagementHttpApiModule))]
    public class OneToManyTestHttpApiModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureLocalization();
    }

    private void ConfigureLocalization()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<OneToManyTestResource>()
                .AddBaseTypes(
                    typeof(AbpUiResource)
                );
        });
    }
}
