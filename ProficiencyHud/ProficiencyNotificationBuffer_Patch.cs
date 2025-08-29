using HarmonyLib;
using Awaken.TG.Main.UI.HUD.AdvancedNotifications.LeftScreen.Proficiency;

namespace ProficiencyHUD;

[HarmonyPatch(typeof(ProficiencyNotificationBuffer), nameof(ProficiencyNotificationBuffer.MakeNotificationPresenter))]
public class ProficiencyNotificationBuffer_Patch
{
    [HarmonyPrefix]
    static bool MakeNotificationPresenterPrefix()
    {
        // Prevents the default in-game proficiency notification from being created
        return false;
    }
}