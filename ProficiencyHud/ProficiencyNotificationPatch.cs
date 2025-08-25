using HarmonyLib;
using Awaken.TG.Main.UI.HUD.AdvancedNotifications.LeftScreen.Proficiency;

namespace ProficiencyHUD;

[HarmonyPatch(typeof(ProficiencyNotificationBuffer), nameof(ProficiencyNotificationBuffer.MakeNotificationPresenter))]
public class ProficiencyNotificationPatch
{
    [HarmonyPrefix]
    static bool MakeNotificationPresenterPrefix(ProficiencyNotificationBuffer __instance)
    {
        return false;
    }
}