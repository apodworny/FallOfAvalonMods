using BepInEx.Configuration;

public class PluginConfig
{
    public ConfigEntry<float> BarsXPositionChange { get; private set; }
    public ConfigEntry<float> BarsYPositionChange { get; private set; }

    public PluginConfig(ConfigFile config)
    {
        config.SaveOnConfigSet = false;
        try
        {
            const string BarsCategory = "BarsPosition";
            const string barsXPositionDescription =
                """
                The x-axis change of the Bars.
                A more positive number means to position the symbol further right.
                A more negative number means to position the symbol further left.
                To approximately center the bars at default hud scale at 1920x1080, set to 600.
                To approximately center the bars at 80% hud scale 1920x1080, set to 750.
                If you have a different resolution or hud scale, you may need to change this number to center the bars.
                """;
            const string barsYPositionDescription =
                """
                The y-axis change of the bars.
                A more positive number means to position the symbol higher.
                A more negative number means to position the symbol lower.
                """;
            BarsXPositionChange = config.Bind(BarsCategory, nameof(BarsXPositionChange), 600f, barsXPositionDescription);
            BarsYPositionChange = config.Bind(BarsCategory, nameof(BarsYPositionChange), 0f, barsYPositionDescription);
        }
        finally
        {
            config.Save();
            config.SaveOnConfigSet = true;
        }
    }
}
