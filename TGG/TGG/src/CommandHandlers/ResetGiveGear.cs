using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.Server;

namespace TGG.src.CommandHandlers
{
    public class ResetGiveGear
    {
        public static void Execute(IServerPlayer player, int groupId, CmdArgs args)
        {
            player.ServerData.CustomPlayerData["isGivedTemporalGear"] = false.ToString();
            player.SendMessage(groupId,"Вы можете получить темпоральную шестеренку еще раз.", EnumChatType.CommandError);
        }
    }
}