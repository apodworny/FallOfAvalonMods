using Awaken.TG.Main.Heroes;
using HarmonyLib;
using Awaken.TG.Main.Character;
using Awaken.TG.Main.General.StatTypes;
using Awaken.TG.Main.UI.HUD.AdvancedNotifications;
using Awaken.TG.Main.UI.HUD.AdvancedNotifications.LeftScreen.Proficiency;
using UnityEngine;

namespace ProficiencyHUD;

[HarmonyPatch]
public class CustomHUDPatch
{
    [HarmonyPatch(typeof(VHeroHUD), nameof(VHeroHUD.AfterFullyInitialized))]
    [HarmonyPostfix]
    static void AfterFullyInitializedPostfix(VHeroHUD __instance)
    {
        // Get a reference to the VHeroHUD
        Transform hudRoot = __instance.transform;

        GameObject proficiencyHUDGameObject = new GameObject("ProficiencyHUD");
        ProficiencyHUD proficiencyHUD = proficiencyHUDGameObject.AddComponent<ProficiencyHUD>();

        // Bind the ProficiencyHUD to the VHeroHUD
        proficiencyHUDGameObject.transform.SetParent(hudRoot, false);

        // Bind hero data to the proficiencyHUD
        proficiencyHUD.SetHero(__instance.Target);
    }
}