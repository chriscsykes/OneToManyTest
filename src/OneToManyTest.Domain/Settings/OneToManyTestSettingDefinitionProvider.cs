using Volo.Abp.Settings;

namespace OneToManyTest.Settings;

public class OneToManyTestSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(OneToManyTestSettings.MySetting1));
    }
}
