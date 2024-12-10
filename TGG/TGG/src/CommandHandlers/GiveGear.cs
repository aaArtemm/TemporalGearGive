using Vintagestory.API.Common;
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
                player.SendMessage(groupId,"Вы уже получили темпоральную шестеренку.", EnumChatType.CommandError);
                return;
            }
            else
            {
                ItemStack temporalGear =
                    new ItemStack(TGGModSystem.ServerApi.World.GetItem(new AssetLocation("gear-temporal")));
                ItemSlot offhandSlot = player.Entity.LeftHandItemSlot;

                if (offhandSlot?.Empty == true)
                {
                    offhandSlot.Itemstack = temporalGear;
                    offhandSlot.MarkDirty(); //this is needed because otherwise the client does not get the update
                    player.ServerData.CustomPlayerData["isGivedTemporalGear"] = true.ToString();
                    player.SendMessage(groupId,"Темпоральная шестеренка получена!", EnumChatType.CommandError);   
                }
                else
                {
                    player.SendMessage(groupId,"Освободите левую руку.", EnumChatType.CommandError);   
                }
            }
        }
    }
}