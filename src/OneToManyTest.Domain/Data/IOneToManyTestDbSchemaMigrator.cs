using System.Threading.Tasks;

namespace OneToManyTest.Data;

public interface IOneToManyTestDbSchemaMigrator
{
    Task MigrateAsync();
}
