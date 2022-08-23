using OneToManyTest.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace OneToManyTest;

[DependsOn(
    typeof(OneToManyTestEntityFrameworkCoreTestModule)
    )]
public class OneToManyTestDomainTestModule : AbpModule
{

}
