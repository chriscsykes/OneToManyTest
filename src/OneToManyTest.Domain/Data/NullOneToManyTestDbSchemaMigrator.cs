using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace OneToManyTest.Data;

/* This is used if database provider does't define
 * IOneToManyTestDbSchemaMigrator implementation.
 */
public class NullOneToManyTestDbSchemaMigrator : IOneToManyTestDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
