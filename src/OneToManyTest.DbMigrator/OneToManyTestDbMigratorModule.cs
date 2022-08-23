using OneToManyTest.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace OneToManyTest.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(OneToManyTestEntityFrameworkCoreModule),
    typeof(OneToManyTestApplicationContractsModule)
)]
public class OneToManyTestDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobOptions>(options =>
        {
            options.IsJobExecutionEnabled = false;
        });
    }
}
