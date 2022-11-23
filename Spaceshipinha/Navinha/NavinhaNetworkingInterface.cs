using SlateShipyard.NetworkingInterface;

namespace Spaceshipinha.Navinha
{
    public class NavinhaNetworkingInterface : SimpleNetworkingInterface
    {
        public NaveThrusterFlameController naveThrusterFlameController;
        public NaveThrusterController naveThrusterController;

        [SyncableProperty]
        public float translationalInputz
        {
            get => naveThrusterController.ReadTranslationalInput().z * (naveThrusterController.enabled ? 1f : 0f);
            set => naveThrusterFlameController.externalTranslationInput = value;
        }
        public override void OnIsPuppetChange(bool isPuppet)
        {
            naveThrusterFlameController.IsPuppet(isPuppet);
            base.OnIsPuppetChange(isPuppet);
        }
    }
}
