using Awaken.TG.Main.Heroes;
using HarmonyLib;

namespace RepositionHud;

[HarmonyPatch]
public class HudPatch
{
    // Adjust the position of the HUD bars
    [HarmonyPatch(typeof(VHeroHUD), nameof(VHeroHUD.AfterFullyInitialized))]
    [HarmonyPostfix]
    public static void AfterFullyInitializedPostfix(VHeroHUD __instance)
    {
        // Change the x and y positions of the HUD bars
        __instance.heroBars.transform.Translate(Plugin.PluginConfig.BarsXPositionChange.Value, Plugin.PluginConfig.BarsYPositionChange.Value, 0);
    }
}