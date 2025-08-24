using BepInEx.Configuration;

public class PluginConfig
{
    public ConfigEntry<float> ProficiencyHUDVerticalPosition { get; private set; }
    public ConfigEntry<bool> ExpGainAnimationEnabled { get; private set; }

    public PluginConfig(ConfigFile config)
    {
        config.SaveOnConfigSet = false;
        try
        {
            const string PositionCategory = "ProficiencyHUDPosition";
            const string proficiencyHUDVerticalPositionDescription =
                """
                The vertical alignment of the proficiency HUD.
                Value must be within 0 and 1, where 0 is the bottom and 1 is the top of the screen.
                """;
            ProficiencyHUDVerticalPosition = config.Bind(PositionCategory, nameof(ProficiencyHUDVerticalPosition), 0.4f, proficiencyHUDVerticalPositionDescription);

            const string AnimationCategory = "ProficiencyHUDAnimation";
            const string expGainAnimationEnabledDescription =
                """
                Enable or disable experience gain animation.
                True means the animation (the colour change of the exp bar) is enabled, false disables it.
                """;
            ExpGainAnimationEnabled = config.Bind(AnimationCategory, nameof(ExpGainAnimationEnabled), true, expGainAnimationEnabledDescription);
        }
        finally
        {
            config.Save();
            config.SaveOnConfigSet = true;
        }
    }
}
