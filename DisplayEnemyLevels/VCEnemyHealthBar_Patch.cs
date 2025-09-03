using HarmonyLib;
using Awaken.TG.Main.Heroes.HUD.Enemies;
using Awaken.TG.Main.Locations;
using Awaken.TG.Main.Fights.NPCs;
using System;

namespace DisplayEnemyLevels;

[HarmonyPatch(typeof(VCEnemyHealthBar), nameof(VCEnemyHealthBar.StartPointing))]
public class VCEnemyHealthBar_Patch
{
    // I should keep this file and make it the level/difficulty score mod
    [HarmonyPostfix]
    private static void StartPointingPostfix(VCEnemyHealthBar __instance, Location location)
    {
        NpcElement npcElement = location.TryGetElement<NpcElement>();
        float meleeDamage = npcElement.Template.meleeDamage;
        float rangedDamage = npcElement.Template.rangedDamage;
        float magicDamage = npcElement.Template.magicDamage;
        int highestDamage = (int)Math.Max(meleeDamage, Math.Max(rangedDamage, magicDamage));

        // Original Difficulty Score formula:
        // DifficultyScore => EffectiveHealth + StrengthLinear * 5;

        // Original effective health formula:
        // float reduction = Damage.GetArmorDamageReduction(armor * armorMultiplier);
        // return MaxHealth / (1 - reduction);

        // Stats currently covered by DifficultyScore:
        // - armor
        // - armorMultiplier
        // - MaxHealth
        // - StrengthLinear

        // Stats I should add to difficultyScore:
        // - MaxStamina (1/3)
        // - HighestDamage
        // - Strength?

        // var healthBar = __instance._currentBars.healthBar;
        // var staminaBar = __instance._currentBars.staminaBar;


        // for current hp, could use any of:
        // withHealthBar.HealthStat.BaseInt
        // npcElement.Health.BaseInt
        // npcElement.HealthStat.BaseInt



        //npcElement.Template.maxHealth
        //npcElement.Template.MaxStamina

        //float powerLevel = health + (highestDamage^1.8) + (stamina*0.5)


        // might have to do something like if there's no stamina and the health is above some amount, add some power

        // ghost squire (lancelot)
        // 600 health
        // 40 damage
        // 999 stamina
        // power level: 600 + (40^1.8) + (999*0.5) = 1864
        // power level: 600 + (40^1.9) + (999*0.5) = 2205 20%: 441

        // wolf
        // 40 health
        // 6 damage
        // 100 stamina
        // power level: 40 + (6^1.8) + (100*0.5) = 115
        // power level: 40 + (6^1.9) + (100*0.5) = 120 20%: 24

        // frostbitten warrior
        // 1500 health
        // 40 damage
        // 80 stamina
        // power level: 1500 + (40^1.8) + (80*0.5) = 2305
        // power level: 1500 + (40^1.9) + (80*0.5) = 2646 20%: 529


        // wyrdstalker
        // 30000 health
        // 80 damage
        // 300 stamina
        // power level: 32814
        // power level: 34279 20%: 6856

        // red death infected with shield (shows 200 in game)
        // 850 health
        // 20 damage (not from template file)
        // 100 stamina
        // power level: 850 + (20^1.9) + (100*0.5) = 1196
        // 632

        //wyrdspirit (150 in game)
        //1 health
        // 28 damage
        // 0 stamina
        // power level: 1 + (28^1.9) + 0 = 563
        // 318 

        // consider changing formula so that the highest damage stat exponentially increases power level?

        // 850 + (20^1.9) + (100*0.5)
        // (850 + (100*0.5)) ^ highestDamage somehow?


        int maxStamina = npcElement.Template.MaxStamina;

        // Account for specifically set, insanely high, max staminas
        if (maxStamina > 99999)
        {
            maxStamina = 0;
        }

        double powerLevel = npcElement.Template.DifficultyScore + Math.Pow(highestDamage, 1.9) + (maxStamina * 0.5);

        // if the enemy has no stamina and the power level is over 1000, add a bonus
        if (maxStamina == 0 && powerLevel > 1000)
        {
            powerLevel += 1000;
        }

        // Compress the values a bit
        powerLevel = Math.Pow(powerLevel, 0.91);

        // bring the power level absolute numbers down
        powerLevel *=  0.4;

        // round it to its nearest multiple of 50
        powerLevel = Math.Round(powerLevel / 50.0) * 50;

        // Make sure power level is at least 1
        if (powerLevel < 1)
        {
            powerLevel = 1;
        }

        __instance.enemyName.text += $" | <color=#FF0000>{(int)powerLevel}</color> | <size=10>Lvl {npcElement.Template.Level} Dmg {highestDamage} Per {npcElement.HasPerception}</size>";
    }
}