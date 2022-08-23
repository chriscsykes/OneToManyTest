using OneToManyTest.Localization;
using Volo.Abp.Application.Services;

namespace OneToManyTest;

/* Inherit your application services from this class.
 */
public abstract class OneToManyTestAppService : ApplicationService
{
    protected OneToManyTestAppService()
    {
        LocalizationResource = typeof(OneToManyTestResource);
    }
}
