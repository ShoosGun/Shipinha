using SlateShipyard.NetworkingInterface;
using UnityEngine;

namespace Spaceshipinha.Navinha
{
    public class NavinhaNetworkingInterface : SimpleNetworkingInterface
    {
        public NaveThrusterFlameController naveThrusterFlameController;
        public NaveThrusterController naveThrusterController;

        [SyncableProperty]
        public float translationalInputz { get => naveThrusterController.ReadTranslationalInput().z; 
            set => naveThrusterFlameController.externalTranslationInput = value; }

        public override void OnIsPuppetChange(bool isPuppet)
        {
            base.OnIsPuppetChange(isPuppet);
            naveThrusterFlameController.IsPuppet(isPuppet);
        }
    }
}
