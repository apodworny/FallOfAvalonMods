using HarmonyLib;
using Awaken.TG.Main.Heroes.HUD.Enemies;
using Awaken.TG.Main.Locations;
using Awaken.TG.Main.Fights.NPCs;

namespace DisplayEnemyLevels;

[HarmonyPatch(typeof(VCEnemyHealthBar), nameof(VCEnemyHealthBar.StartPointing))]
public class EnemyHealthBarPatch
{
    [HarmonyPostfix]
    static void StartPointingPostfix(VCEnemyHealthBar __instance, Location location)
    {
        NpcElement npcElement = location.TryGetElement<NpcElement>();
        __instance.enemyName.text += $" | <size=12>Lvl {npcElement.Template.Level}</size> | <size=12><color=#FF0000>{npcElement.Template.DifficultyScore}</color></size>";
    }
}