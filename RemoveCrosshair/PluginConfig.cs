using BepInEx.Configuration;

public class PluginConfig
{
    public ConfigEntry<float> SneakSymbolXPositionChange { get; private set; }
    public ConfigEntry<float> SneakSymbolYPositionChange { get; private set; }

    public PluginConfig(ConfigFile config)
    {
        config.SaveOnConfigSet = false;
        try
        {
            const string SneakSymbolPositionCategory = "SneakSymbolPosition";
            const string xPositionDescription =
                """
                The x-axis change of the sneaking symbol.
                A more positive number means to position the symbol further right.
                A more negative number means to position the symbol further left.
                """;
            const string yPositionDescription =
                """
                The y-axis change of the sneaking symbol.
                A more positive number means to position the symbol higher.
                A more negative number means to position the symbol lower.
                """;
            SneakSymbolXPositionChange = config.Bind(SneakSymbolPositionCategory, nameof(SneakSymbolXPositionChange), 0f, xPositionDescription);
            SneakSymbolYPositionChange = config.Bind(SneakSymbolPositionCategory, nameof(SneakSymbolYPositionChange), -200f, yPositionDescription);
        }
        finally
        {
            config.Save();
            config.SaveOnConfigSet = true;
        }
    }
}
