using OWML.ModHelper;
using UnityEngine;
using Spaceshipinha.Navinha;
using OWML.Common;
using SlateShipyard.ShipSpawner;

namespace Spaceshipinha
{
    public class Spaceshipinha : ModBehaviour
    {
        public static GameObject navinhaPrefab;
        public static IModHelper modHelper;

        public static bool ControllerInputs = false;
        private void Start()
        {
            AssetBundle bundle = ModHelper.Assets.LoadBundle("AssetBundles/navinha");

            navinhaPrefab = bundle.LoadAsset<GameObject>("navinha_bodyv2.prefab");

            modHelper = ModHelper;
            ShipSpawnerManager.AddShip(navinhaPrefab, "Spaceshipinha");
        }
        public override void Configure(IModConfig config)
        {
            NaveThrusterController.ControllerDeadZone = config.GetSettingsValue<float>("controllerDeadZone");
            ControllerInputs = config.GetSettingsValue<bool>("controllerInputs");
        }
    }
}
