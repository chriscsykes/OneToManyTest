using OneToManyTest.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace OneToManyTest.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class OneToManyTestController : AbpControllerBase
{
    protected OneToManyTestController()
    {
        LocalizationResource = typeof(OneToManyTestResource);
    }
}
