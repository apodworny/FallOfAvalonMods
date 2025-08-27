using HarmonyLib;
using Awaken.TG.Main.Heroes.Items;
using Awaken.TG.Main.Heroes;
using Awaken.TG.MVC.UI.Events;
using Awaken.TG.Main.Utility;
using Awaken.TG.Main.Character;
using System.Linq;
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
            if (action == KeyBindings.HeroItems.NextQuickSlot)
            {
                System.Collections.Generic.IEnumerable<Item> items = __instance.Target.HeroItems.Items;

                Item quickSlot2Item = items.FirstOrDefault(item => item.EquippedInSlotOfType?.EnumName == "QuickSlot2");
                Item foodQuickSlotItem = items.FirstOrDefault(item => item.EquippedInSlotOfType?.EnumName == "FoodQuickSlot");

                __instance.Target.HeroItems.TryGetSelectedQuickSlotItem(out Item selectedItem);

                // Accounts for the various states of the quick slot by making sure something always happens
                if (selectedItem?._itemName == quickSlot2Item?._itemName)
                {
                    __instance.UseQuickSlotItem(foodQuickSlotItem);
                }
                else if (selectedItem?._itemName == foodQuickSlotItem?._itemName)
                {
                    __instance.UseQuickSlotItem(quickSlot2Item);
                }
                else
                {
                    __instance.Target.HeroItems.SelectNextQuickSlot();
                }

                return false;
            }
        }
        
        // Run original method if we didn't return yet
        return true;
    }
}