using OneToManyTest.Localization;
using Volo.Abp.AspNetCore.Components;

namespace OneToManyTest.Blazor;

public abstract class OneToManyTestComponentBase : AbpComponentBase
{
    protected OneToManyTestComponentBase()
    {
        LocalizationResource = typeof(OneToManyTestResource);
    }
}
