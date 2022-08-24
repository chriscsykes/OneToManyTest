using Volo.Abp.Account;
using Volo.Abp.AuditLogging;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Gdpr;
using Volo.Abp.Identity;
using Volo.Abp.LanguageManagement;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TextTemplateManagement;
using Volo.Saas.Host;
using Volo.FileManagement;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.Azure;

namespace OneToManyTest;

[DependsOn(
    typeof(OneToManyTestDomainModule),
    typeof(OneToManyTestApplicationContractsModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(SaasHostApplicationModule),
    typeof(AbpAuditLoggingApplicationModule),
    typeof(AbpOpenIddictProApplicationModule),
    typeof(AbpAccountPublicApplicationModule),
    typeof(AbpAccountAdminApplicationModule),
    typeof(LanguageManagementApplicationModule),
    typeof(AbpGdprApplicationModule),
    typeof(TextTemplateManagementApplicationModule)
    )]
[DependsOn(typeof(FileManagementApplicationModule))]
    [DependsOn(typeof(AbpBlobStoringAzureModule))]
    public class OneToManyTestApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<OneToManyTestApplicationModule>();
        });

        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.ConfigureDefault(container =>
            {
                //TODO...
                container.UseAzure(azure =>
                {
                    azure.ConnectionString = "DefaultEndpointsProtocol=https;" +
                    "AccountName=rayzor;" +
                    "AccountKey=plL7UDQX1XwqdK/yEpcnxEHNmAcsEtbkgINmE9ovpbBz1lUpFhBIRKg+ALi7mfFRko2hHMWK4F9P+AStSm5zGw==;" +
                    "EndpointSuffix=core.windows.net";

                    azure.ContainerName = "azureblob";

                    azure.CreateContainerIfNotExists = true;
                });

            });
        });
    }
}
