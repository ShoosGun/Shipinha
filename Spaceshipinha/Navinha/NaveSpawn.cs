﻿using SlateShipyard.ShipSpawner;

using UnityEngine;

namespace Spaceshipinha.Navinha
{
    internal class NaveSpawn : MonoBehaviour
    {
        private string seatOnPrompt = "Seat On";

        public void Start() 
        {
            ShipSpawnerManager.AddShip(CreateNave, "Spaceshipinha");
        }
        public GameObject CreateNave()
        {
            GameObject naveBody = Instantiate(Spaceshipinha.navinhaPrefab);

            NaveBody naveBodyRigid = naveBody.AddComponent<NaveBody>();
            naveBody.AddComponent<NaveThrusterModel>();
            NaveThrusterController naveThrusterController = naveBody.AddComponent<NaveThrusterController>();
            naveBody.AddComponent<NaveControlledVanish>();
            naveBody.AddComponent<ImpactSensor>();

            AstroObject astroObject = naveBody.AddComponent<AstroObject>();
            astroObject._customName = transform.name;
            astroObject._name = AstroObject.Name.None;
            astroObject._type = AstroObject.Type.None;

            #region Nave_Seat
            //Assento
            GameObject naveSeat = naveBody.transform.GetChild(2).GetChild(0).GetChild(0).gameObject;
            //naveSeat.layer = LayerMask.NameToLayer("BasicEffectVolume");
            naveSeat.AddComponent<InteractZone>().ChangePrompt(seatOnPrompt);

            PlayerAttachPoint attachPoint = naveSeat.AddComponent<PlayerAttachPoint>();
            attachPoint._lockPlayerTurning = true;
            attachPoint._centerCamera = true;
            NaveFlightConsole naveFlightConsole = naveSeat.AddComponent<NaveFlightConsole>();
            naveFlightConsole.naveBody = naveBodyRigid;
            naveFlightConsole.naveThrusterController = naveThrusterController;

            naveThrusterController.naveFlightConsole = naveFlightConsole;
            naveBodyRigid.naveFlightConsole = naveFlightConsole;
            naveThrusterController.Innit();
            naveBodyRigid.Innit();
            #endregion

            #region Nave_Detectors
            //Detector
            GameObject naveDetector = naveBody.transform.GetChild(1).gameObject;
            //naveDetector.layer = LayerMask.NameToLayer("BasicDetector");

            SphereShape shape = naveDetector.AddComponent<SphereShape>();
            shape.CopySettingsFromCollider();
            shape.RecalculateLocalBounds();
            shape.SetCollisionMode(Shape.CollisionMode.Detector);
            ShapeManager.AddShape(shape);

            naveDetector.AddComponent<DynamicForceDetector>();
            naveDetector.AddComponent<SectorDetector>();

            DynamicFluidDetector fluidDetec = naveDetector.AddComponent<DynamicFluidDetector>();
            fluidDetec.SetDragFactor(2f);
            fluidDetec._dontApplyForces = false;
            naveBodyRigid.RegisterAttachedFluidDetector(fluidDetec);

            naveDetector.AddComponent<ShipNoiseMaker>();
            naveDetector.AddComponent<FogWarpDetector>();
            naveDetector.AddComponent<RulesetDetector>();
            #endregion

            #region Nave_ProxyCollider
            //naveBody.transform.GetChild(3).GetChild(0).gameObject.layer = LayerMask.NameToLayer("ProxyPrimitive");
            #endregion

            #region Nave_RF
            GameObject naveRFVolume = naveBody.transform.GetChild(3).gameObject;
            naveRFVolume.SetActive(false);
            //naveRFVolume.layer = LayerMask.NameToLayer("ReferenceFrameVolume"); 
            //naveRFVolume.AddComponent<AutoReferenceFrameVolume>();
            #endregion

            #region Nave_Audio
            //Audio
            GameObject naveAudioSources = naveBody.transform.GetChild(4).gameObject;
            ThrusterAudio thrusterAudio = naveAudioSources.AddComponent<ThrusterAudio>();

            OWAudioSource thrusterAudioSource = naveAudioSources.transform.GetChild(0).gameObject.AddComponent<OWAudioSource>();
            OWAudioSource rotationalAudioSource = naveAudioSources.transform.GetChild(1).gameObject.AddComponent<OWAudioSource>();

            thrusterAudioSource.playOnAwake = false;
            thrusterAudioSource.clip = Spaceshipinha.navinhaThrusterAudio;
            thrusterAudioSource._track = OWAudioMixer.TrackName.Ship;

            thrusterAudio._translationalSource = thrusterAudioSource;
            thrusterAudio._rotationalSource = rotationalAudioSource;
            thrusterAudio._rotationClip = AudioType.MovementWoodCreakFootstep;
            thrusterAudio._underwaterRotationClip = AudioType.MovementShallowWaterFootstep;

            #endregion

            return naveBody;
        }
    }
}