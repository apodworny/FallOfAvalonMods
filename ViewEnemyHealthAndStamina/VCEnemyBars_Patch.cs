using HarmonyLib;
using Awaken.TG.Main.Heroes.HUD.Enemies;
using Awaken.TG.Main.Locations;
using Awaken.TG.Main.Fights.NPCs;
using UnityEngine;
using UnityEngine.UI;
using Awaken.TG.Main.Character;

namespace ViewEnemyHealthAndStamina;

[HarmonyPatch(typeof(VCEnemyBars), nameof(VCEnemyBars.UpdateHP))]
public class VCEnemeyBars_Patch
{
    [HarmonyPostfix]
    private static void UpdateHPPostfix(VCEnemyBars __instance, Location location)
    {
        UpdateHPBar(__instance, location);
        UpdateStaminaBar(__instance, location);
    }

    private static void UpdateHPBar(VCEnemyBars __instance, Location location)
    {
        NpcElement npcElement = location.TryGetElement<NpcElement>();
        Transform existing = __instance.healthBar.transform.Find("EnemyHealthLabel");
        Text labelText = null;
        EnemyHealthLabelInfo info = null;

        // Create label if it doesn't exist
        if (existing == null)
        {
            GameObject enemyHealthTextGameObject = new GameObject("EnemyHealthLabel");
            enemyHealthTextGameObject.transform.SetParent(__instance.healthBar.transform, false);

            labelText = enemyHealthTextGameObject.AddComponent<Text>();
            labelText.color = Color.white;
            labelText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            labelText.alignment = TextAnchor.MiddleCenter;

            info = enemyHealthTextGameObject.AddComponent<EnemyHealthLabelInfo>();
        }
        else
        {
            labelText = existing.GetComponent<Text>();
            info = existing.GetComponent<EnemyHealthLabelInfo>();
        }

        // Always update the npcElement reference
        if (info != null)
        {
            info.npcElement = npcElement;
            if (npcElement != null)
                info.lastKnownMaxHealth = npcElement.MaxHealth.BaseInt;
        }

        // Update label text
        if (location.TryGetElement(out IWithHealthBar element) && !VCEnemyBars.HasHealthBarBlocker(location, element))
        {
            if (labelText != null && info != null && info.npcElement != null)
                labelText.text = $"{info.npcElement.Health.BaseInt}/{info.npcElement.MaxHealth.BaseInt}";
        }
        else
        {
            if (labelText != null && info != null)
            {
                int maxHealth = info.npcElement != null ? info.npcElement.MaxHealth.BaseInt : info.lastKnownMaxHealth;
                labelText.text = $"0/{maxHealth}";
            }

        }
    }

    private static void UpdateStaminaBar(VCEnemyBars __instance, Location location)
    {
        NpcElement npcElement = location.TryGetElement<NpcElement>();
        Transform existing = __instance.staminaBar.transform.Find("EnemyStaminaLabel");
        Text labelText = null;
        EnemyStaminaLabelInfo info = null;

        // Create label if it doesn't exist
        if (existing == null)
        {
            GameObject enemyStaminaTextGameObject = new GameObject("EnemyStaminaLabel");
            enemyStaminaTextGameObject.transform.SetParent(__instance.staminaBar.transform, false);

            labelText = enemyStaminaTextGameObject.AddComponent<Text>();
            labelText.color = Color.white;
            labelText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            labelText.alignment = TextAnchor.MiddleCenter;

            info = enemyStaminaTextGameObject.AddComponent<EnemyStaminaLabelInfo>();
        }
        else
        {
            labelText = existing.GetComponent<Text>();
            info = existing.GetComponent<EnemyStaminaLabelInfo>();
        }

        // Always update the npcElement reference
        if (info != null)
        {
            info.npcElement = npcElement;
            if (npcElement != null)
                info.lastKnownMaxStamina = npcElement.CharacterStats.MaxStamina.BaseInt;
        }

        int maxStamina = info.npcElement != null ? info.npcElement.CharacterStats.MaxStamina.BaseInt : info.lastKnownMaxStamina;

        // If the enemy is staggered, they are out of stamina
        if (info.npcElement.Staggered)
        {
            labelText.text = $"0/{maxStamina}";
        }
        else
        {
            labelText.text = $"{info.npcElement.CharacterStats.Stamina.BaseInt}/{info.npcElement.CharacterStats.MaxStamina.BaseInt}";
        }
    }
}