using GameNetcodeStuff;
using UnityEngine;

namespace LCChaosMod.Utils
{
    public static class PlayerUtils
    {
        // Повертає локального гравця або null.
        public static PlayerControllerB? GetLocalPlayer()
        {
            return GameNetworkManager.Instance?.localPlayerController;
        }

        // * Чи знаходиться гравець на кораблі (сейф зона — евенти не діють).
        public static bool IsOnShip(PlayerControllerB player)
        {
            return player.isInElevator || player.isInHangarShipRoom;
        }

        // Чи знаходиться гравець в данжі (під землею).
        public static bool IsInDungeon(PlayerControllerB player)
        {
            return player.isInsideFactory;
        }

        // * Чи позиція знаходиться занадто близько до корабля (сейф зона спавну).
        public static bool IsNearShip(Vector3 pos, float radius = 14f)
        {
            var ship = StartOfRound.Instance?.elevatorTransform;
            if (ship == null) return false;
            return Vector3.Distance(pos, ship.position) < radius;
        }
    }
}
