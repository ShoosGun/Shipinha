using SlateShipyard.NetworkingInterface;
using UnityEngine;

namespace Spaceshipinha.Navinha
{
    public class NavinhaNetworkingInterface : SimpleNetworkingInterface
    {
        //public override bool IsPuppet { get => isPuppet; set => isPuppet = value; }

        public NaveThrusterFlameController naveThrusterFlameController;
        public NaveThrusterController naveThrusterController;

        [SyncableProperty]
        public float translationalInputz
        {
            get => naveThrusterController.ReadTranslationalInput().z;
            set => naveThrusterFlameController.externalTranslationInput = value;
        }
        public override void OnIsPuppetChange(bool isPuppet)
        {
            naveThrusterFlameController.IsPuppet(isPuppet);
            base.OnIsPuppetChange(isPuppet);
        }
        //public void Start()
        //{

        //    if (IsPuppet)
        //    {
        //        for (int i = 0; i < scriptsToDisableWhenPuppet.Length; i++)
        //        {
        //            scriptsToDisableWhenPuppet[i].enabled = false;
        //        }
        //        for (int i = 0; i < gameObjectsToDisableWhenPuppet.Length; i++)
        //        {
        //            gameObjectsToDisableWhenPuppet[i].SetActive(false);
        //        }

        //        Rigidbody r = GetComponent<Rigidbody>();
        //        if (r != null)
        //        {
        //            r.isKinematic = RigidbodyToKinematicWhenPuppet;
        //        }
        //    }
        //}
    }
    /*
    public class NavinhaNetworkingInterface : SimpleNetworkingInterface
    {
        public NaveThrusterFlameController naveThrusterFlameController;
        public NaveThrusterController naveThrusterController;

        [SyncableProperty]
        public float translationalInputz 
        { 
            get => naveThrusterController.ReadTranslationalInput().z; 
            set => naveThrusterFlameController.externalTranslationInput = value; 
        }

        public override void OnIsPuppetChange(bool isPuppet)
        {
            base.OnIsPuppetChange(isPuppet);
            naveThrusterFlameController.IsPuppet(isPuppet);
        }
    }
    */
}
