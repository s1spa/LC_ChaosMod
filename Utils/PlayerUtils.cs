using GameNetcodeStuff;
using UnityEngine;

namespace LCChaosMod.Utils
{
    public static class PlayerUtils
    {
        /// <summary>Повертає локального гравця або null.</summary>
        public static PlayerControllerB? GetLocalPlayer()
        {
            return GameNetworkManager.Instance?.localPlayerController;
        }

        /// <summary>Чи знаходиться гравець на кораблі (сейф зона — евенти не діють).</summary>
        public static bool IsOnShip(PlayerControllerB player)
        {
            return player.isInElevator || player.isInHangarShipRoom;
        }

        /// <summary>Чи знаходиться гравець в данжі (під землею).</summary>
        public static bool IsInDungeon(PlayerControllerB player)
        {
            return player.isInsideFactory;
        }
    }
}
