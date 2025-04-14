using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using Vintagestory.Server;

namespace TGG.src.CommandHandlers
{
    public class GiveGear
    {
        public static void Execute(IServerPlayer player, int groupId, CmdArgs args)
        {
            if (player.ServerData.CustomPlayerData.ContainsKey("isGivedTemporalGear") && player.ServerData.CustomPlayerData.ContainsValue(true.ToString()))
            {
                player.SendMessage(groupId,"Вы уже получили темпоральную шестеренку.", EnumChatType.CommandSuccess);
                return;
            }
            else
            {
                string itemCode = "game:gear-temporal";

                var gearItem = TGGModSystem.ServerApi.World?.GetItem(new AssetLocation(itemCode));
                if (gearItem == null)
                {
                    player.SendMessage(groupId, "Не удалось найти темпоральную шестерёнку!", EnumChatType.Notification);
                    return;
                }

                var gearStack = new ItemStack(gearItem, 1);
                if (gearStack == null)
                {
                    player.SendMessage(groupId, "Ошибка при создании ItemStack!", EnumChatType.Notification);
                    return;
                }

                var invMan = player.InventoryManager;
                if (invMan == null)
                {
                    player.SendMessage(groupId, "Ошибка: InventoryManager не найден!", EnumChatType.Notification);
                    return;
                }

                List<IInventory> inventories = new List<IInventory>
                {
                    invMan.GetOwnInventory("hotbar"),
                    invMan.GetOwnInventory("inventory")
                };

                var backpack = invMan.GetOwnInventory("backpack");
                if (backpack != null)
                {
                    inventories.Add(backpack);
                }

                ItemSlot tempSlot = new DummySlot(gearStack);

                foreach (var inv in inventories)
                {
                    if (inv == null) continue;

                    foreach (var slot in inv)
                    {
                        if (slot == null || !slot.Empty) continue;
                        
                        if (!slot.CanHold(tempSlot)) continue;

                        try
                        {
                            slot.Itemstack = gearStack.Clone();
                            slot.MarkDirty();

                            player.ServerData.CustomPlayerData["isGivedTemporalGear"] = true.ToString();
                            player.SendMessage(groupId, "Выдана темпоральная шестерёнка.", EnumChatType.Notification);
                            return;
                        }
                        catch (Exception ex)
                        {
                            player.SendMessage(groupId, $"Ошибка при выдаче предмета: {ex.Message}", EnumChatType.Notification);
                            return;
                        }
                    }
                }

                player.SendMessage(groupId, "Нет свободных слотов!", EnumChatType.Notification);
            }

        }
    }
}