using Volo.Abp.Modularity;

namespace OneToManyTest;

[DependsOn(
    typeof(OneToManyTestApplicationModule),
    typeof(OneToManyTestDomainTestModule)
    )]
public class OneToManyTestApplicationTestModule : AbpModule
{

}
