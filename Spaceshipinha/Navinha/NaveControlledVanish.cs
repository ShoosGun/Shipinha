using UnityEngine;
using SlateShipyard.VanishObjects;

namespace Spaceshipinha.Navinha
{
    public class NaveControlledVanish : ControlledVanishObject
    {
        private OWRigidbody naveBody;
        private NaveFlightConsole naveConsole;
        public void Awake()
        {
            DestroyComponentsOnGrow = false;
            VanishVolumesPatches.OnConditionsForPlayerToWarp += OnConditionsForPlayerToWarp;
        }
        public void OnDestroy()
        {
            VanishVolumesPatches.OnConditionsForPlayerToWarp -= OnConditionsForPlayerToWarp;
        }
        public void Start()
        {
            naveBody = gameObject.GetAttachedOWRigidbody();
            naveConsole = gameObject.GetComponentInChildren<NaveFlightConsole>();
        }
        public override bool OnDestructionVanish(DestructionVolume destructionVolume)
        {
            if (naveConsole.enabled)
            {
                Locator.GetDeathManager().KillPlayer(destructionVolume._deathType);
                return false;
            }
            return true;
        }
        public override bool OnSupernovaDestructionVanish(SupernovaDestructionVolume supernovaDestructionVolume)
        {
            return OnDestructionVanish(supernovaDestructionVolume);
        }
        public override bool OnBlackHoleVanish(BlackHoleVolume blackHoleVolume, RelativeLocationData entryLocation)
        {
            blackHoleVolume._whiteHole.ReceiveWarpedBody(naveBody, entryLocation);
            return false;
        }
        public override bool OnWhiteHoleReceiveWarped(WhiteHoleVolume whiteHoleVolume, RelativeLocationData entryData)
        {
            whiteHoleVolume.SpawnImmediately(naveBody, entryData);
            return false;
        }
        public override void OnWhiteHoleSpawnImmediately(WhiteHoleVolume whiteHoleVolume, RelativeLocationData entryData, out bool playerPassedThroughWarp)
        {
            playerPassedThroughWarp = false;
            if (Time.time > whiteHoleVolume._lastShipWarpTime + Time.deltaTime)
            {
                whiteHoleVolume._lastShipWarpTime = Time.time;
                if (naveConsole.enabled)
                {
                    playerPassedThroughWarp = true;
                    whiteHoleVolume.MakeRoomForBody(naveBody);
                }
            }
        }
        public override bool OnTimeLoopBlackHoleVanish(TimeLoopBlackHoleVolume timeloopBlackHoleVolume)
        {
            if (naveConsole.enabled)
            {
                Locator.GetDeathManager().KillPlayer(DeathType.TimeLoop);
                return false;
            }
            return true;
        }
        private bool OnConditionsForPlayerToWarp()
        {
            if (naveConsole != null) {
                return !naveConsole.enabled; 
            }
            return true;
        }
    }
}
