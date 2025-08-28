using HarmonyLib;
using Awaken.TG.Main.Heroes.Items;
using Awaken.TG.Main.Heroes;
using Awaken.TG.MVC.UI.Events;
using Awaken.TG.Main.Utility;
using System.Linq;
using System.Collections.Generic;
namespace HotkeyConsumables;

[HarmonyPatch(typeof(VHeroKeys), nameof(VHeroKeys.Handle))]
public class EnemyHealthBarPatch
{
    [HarmonyPrefix]
    static bool HandlePrefix(VHeroKeys __instance, UIEvent evt)
    {
        if (!__instance.Target.IsAlive) return true;

        if (evt is UIKeyDownAction uiDownAction)
        {
            string action = uiDownAction.Name;

            List<string> handledActions =
            [
                KeyBindings.HeroItems.UseQuickSlot,
                KeyBindings.HeroItems.NextQuickSlot,
                KeyBindings.UI.CharacterSheets.Journal
            ];

            // Makes sure that the food quickslot is selected
            __instance.Target.HeroItems.SelectQuickSlot(EquipmentSlotType.FoodQuickSlot);

            if (handledActions.Contains(action))
            {
                IEnumerable<Item> items = __instance.Target.HeroItems.Items;

                Item foodQuickSlotItem = items.FirstOrDefault(item => item.EquippedInSlotOfType?.EnumName == "FoodQuickSlot");
                Item quickSlot2Item = items.FirstOrDefault(item => item.EquippedInSlotOfType?.EnumName == "QuickSlot2");
                Item quickSlot3Item = items.FirstOrDefault(item => item.EquippedInSlotOfType?.EnumName == "QuickSlot3");

                if (action == handledActions[0])
                {
                    __instance.UseQuickSlotItem(foodQuickSlotItem);
                }
                else if (action == handledActions[1])
                {
                    __instance.UseQuickSlotItem(quickSlot2Item);
                }
                else if (action == handledActions[2])
                {
                    __instance.UseQuickSlotItem(quickSlot3Item);
                }

                return false;
            }
            else
            {
                return true;
            }
        }
        
        // Run original method if we didn't return yet
        return true;
    }
}