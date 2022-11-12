using SlateShipyard.NetworkingInterface;
using UnityEngine;

namespace Spaceshipinha.Navinha
{
    public class NavinhaNetworkingInterface : ObjectNetworkingInterface
    {
        bool isPuppet = false;
        public override bool IsPuppet { get => isPuppet; set => isPuppet = value; }

        public GameObject[] gameObjectsToDisableWhenPuppet;
        public MonoBehaviour[] scriptsToDisableWhenPuppet;
        public bool RigidbodyToKinematicWhenPuppet = true;

        public NaveThrusterFlameController naveThrusterFlameController;
        public NaveThrusterController naveThrusterController;

        [SyncableProperty]
        public float translationalInputz { get => naveThrusterController.ReadTranslationalInput().z; 
            set => naveThrusterFlameController.externalTranslationInput = value; }
        public void Start()
        {
            naveThrusterFlameController.IsPuppet(IsPuppet);

            if (IsPuppet)
            {
                for (int i = 0; i < scriptsToDisableWhenPuppet.Length; i++)
                {
                    scriptsToDisableWhenPuppet[i].enabled = false;
                }
                for (int i =0; i< gameObjectsToDisableWhenPuppet.Length; i++) 
                {
                    gameObjectsToDisableWhenPuppet[i].SetActive(false);
                }

                Rigidbody r = GetComponent<Rigidbody>();
                if (r != null)
                {
                    r.isKinematic = RigidbodyToKinematicWhenPuppet;
                }
            }
        }
    }
}
