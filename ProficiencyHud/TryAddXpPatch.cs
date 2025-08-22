using Awaken.TG.Main.Heroes;
using HarmonyLib;
using Awaken.TG.Main.Heroes.Stats.Observers;
using Awaken.TG.Main.General.StatTypes;
using Awaken.TG.Main.Character;
using UnityEngine;

namespace ProficiencyHUD;

[HarmonyPatch]
public class TryAddXpPatch
{
    [HarmonyPatch(typeof(ProficiencyStats), nameof(ProficiencyStats.TryAddXP))]
    [HarmonyPostfix]
    static void TryAddXPPostfix(ProficiencyEventListener __instance, ProfStatType targetStatToRaiseXPOf, float amountOfXPToAdd)
    {
        Hero hero = __instance.ParentModel;

        ProficiencyHUD proficiencyHUD = Object.FindAnyObjectByType<ProficiencyHUD>();

        if (proficiencyHUD != null)
        {
            float progressToNextLevel = hero.ProficiencyStats.GetProgressToNextLevel(targetStatToRaiseXPOf);

            // XP was gained, so update the proficiencyHUD
            proficiencyHUD.UpdateProgress(targetStatToRaiseXPOf, progressToNextLevel);
        }
    }
}