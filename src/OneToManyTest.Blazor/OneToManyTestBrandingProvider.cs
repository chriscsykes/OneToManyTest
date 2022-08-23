using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace OneToManyTest.Blazor;

[Dependency(ReplaceServices = true)]
public class OneToManyTestBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "OneToManyTest";
}
