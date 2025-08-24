using BepInEx.Configuration;

public class PluginConfig
{
    public ConfigEntry<float> ProficiencyHUDVerticalPosition { get; private set; }

    public PluginConfig(ConfigFile config)
    {
        config.SaveOnConfigSet = false;
        try
        {
            const string BarsCategory = "ProficiencyHUDPosition";
            const string proficiencyHUDVerticalPositionDescription =
                """
                The vertical alignment of the proficiency HUD.
                Value must be within 0 and 1, where 0 is the bottom and 1 is the top of the screen.
                """;
            ProficiencyHUDVerticalPosition = config.Bind(BarsCategory, nameof(ProficiencyHUDVerticalPosition), 0.4f, proficiencyHUDVerticalPositionDescription);
        }
        finally
        {
            config.Save();
            config.SaveOnConfigSet = true;
        }
    }
}
