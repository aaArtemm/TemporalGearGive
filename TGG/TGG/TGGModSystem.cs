using System.Collections.Generic;
using TGG.src.CommandHandlers;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using Vintagestory.Server;

namespace TGG
{
    public class TGGModSystem : ModSystem
    {
        public static ICoreServerAPI ServerApi;
        public static ServerPlayerData PlayerData;

        public Dictionary<string, string> CustomPlayerData = new Dictionary<string, string>
        {
            ["isGivedTemporalGear"] = false.ToString()
        };

        public override bool ShouldLoad(EnumAppSide forSide)
        {
            return forSide == EnumAppSide.Server;
        }
        
        public override void StartServerSide(ICoreServerAPI api)
        {
            ServerApi = api;
            PlayerData = new ServerPlayerData();

            ServerApi.Event.PlayerJoin += OnPlayerJoined;
            
            ServerApi.RegisterCommand("givegear", "give temporal gear", "", GiveGear.Execute, Privilege.chat);
            ServerApi.RegisterCommand("rgivegear", "give temporal gear", "", ResetGiveGear.Execute, Privilege.ban);
        }

        private void OnPlayerJoined(IServerPlayer player)
        {
            if (player.ServerData.CustomPlayerData == null)
            {
                player.ServerData.CustomPlayerData.Add("isGivedTemporalGear", false.ToString());
                
                //PlayerData.CustomPlayerData = player.ServerData.CustomPlayerData;
            }
        }
    }
}