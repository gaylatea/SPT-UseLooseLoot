using System;
using System.Reflection;

using Aki.Reflection.Patching;

using EFT.InventoryLogic;
using EFT;

namespace Gaylatea
{
    namespace UseLooseLoot
    {
        /// <summary>
        /// Changes loose food and med items so that they can be used without
        /// needing to take them into your inventory first.
        /// </summary>
        class MakeFoodMedsUsablePatch : ModulePatch
        {
            protected override MethodBase GetTargetMethod()
            {
                return typeof(GClass1766).GetMethod("smethod_4", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            }

            [PatchPostfix]
            public static void PatchPostfix(ref GClass2645 __result, Item rootItem, GamePlayerOwner owner)
            {
                if(!(rootItem is MedsClass) && !(rootItem is FoodClass)) {
                    return;
                }

                var @class = new FoodMedUser();
                @class.owner = owner;
                @class.item = rootItem;
                __result.Actions.Add(new GClass2644{
                    Name = "Use".Localized(null),
                    Action = new Action(@class.UseAll),
                });
            }
        }

        class FoodMedUser {
            public GamePlayerOwner owner;
            public Item item;

            public void UseAll() {
                this.owner.Player.HealthController.ApplyItem(this.item, EBodyPart.Common);
            }
        }
    }
}