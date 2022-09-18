using Spaceshipinha;
using System;
using UnityEngine;

namespace Spaceshipinha.Navinha
{
    internal class NaveFlightConsole : MonoBehaviour
    {
        public OWRigidbody naveBody;

        private SingleInteractionVolume interactVolume;

        private PlayerAttachPoint attachPoint;

        //private OWRigidbody attachedBody;

        private PlayerAudioController playerAudio;

        private ScreenPrompt promptDeAtivar;
        private ScreenPrompt promptDeFreelook;
        private ScreenPrompt promptDeReduzirPotencia;
        private ScreenPrompt promptDeAumentarPotencia;


        private ScreenPrompt promptDeAtivarKB;
        private ScreenPrompt promptDeReduzirPotenciaKB;
        private ScreenPrompt promptDeAumentarPotenciaKB;

        private ScreenPrompt promptDeAtivarControle;
        private ScreenPrompt promptDeMudarPotenciaControle;

        public ScreenPrompt valorDaPotencia;
        public NaveThrusterController naveThrusterController;

        private string FreeLookPrompt = "Free Look";
        private string TurnOnPrompt = "Turn On";
        private string LowerPowerPrompt = "Lower Power";
        private string IncreasePowerPrompt = "Increase Power";
        private string ChangePowerPrompt = "Change Power";
        private string PowerPrompt = "Power: ";

        public event Action OnEnterNaveFlightConsole;
        public event Action OnExitNaveFlightConsole;

        private void Awake()
        {
            enabled = false;

            promptDeFreelook = new ScreenPrompt(InputLibrary.freeLook, FreeLookPrompt, 1);

            promptDeAtivarKB = new ScreenPrompt(InputLibrary.thrustZ, TurnOnPrompt, 1);
            promptDeReduzirPotenciaKB = new ScreenPrompt(InputLibrary.thrustDown, LowerPowerPrompt, 1);
            promptDeAumentarPotenciaKB = new ScreenPrompt(InputLibrary.thrustUp, IncreasePowerPrompt, 1);

            promptDeAtivarControle = new ScreenPrompt(InputLibrary.thrustUp, TurnOnPrompt, 1);
            promptDeMudarPotenciaControle = new ScreenPrompt(InputLibrary.thrustZ, ChangePowerPrompt, 1);

            valorDaPotencia = new ScreenPrompt(PowerPrompt, 1);

            attachPoint = this.GetRequiredComponent<PlayerAttachPoint>();
            interactVolume = this.GetRequiredComponent<SingleInteractionVolume> ();
            playerAudio = Locator.GetPlayerAudioController();

            interactVolume.OnPressInteract += OnPressInteract;
        }

        private void OnDestroy()
        {
            interactVolume.OnPressInteract -= OnPressInteract;
        }

        private void ChoosePromptType() 
        {
            promptDeAtivar = Spaceshipinha.ControllerInputs ? promptDeAtivarControle : promptDeAtivarKB;
            promptDeReduzirPotencia = Spaceshipinha.ControllerInputs ? promptDeMudarPotenciaControle : promptDeReduzirPotenciaKB;
            promptDeAumentarPotencia = Spaceshipinha.ControllerInputs ? null : promptDeAumentarPotenciaKB;
        }
        private void OnPressInteract()
        {
            if (!enabled)
            {
                playerAudio.PlayOneShotInternal(AudioType.ShipCockpitBuckleUp);
                attachPoint.AttachPlayer();
                interactVolume.DisableInteraction();
                enabled = true;
                OnEnterNaveFlightConsole?.Invoke();

                ChoosePromptType();
                PromptManager manager = Locator.GetPromptManager();

                manager.AddScreenPrompt(promptDeAtivar, PromptPosition.LowerLeft, true);
                manager.AddScreenPrompt(promptDeReduzirPotencia, PromptPosition.LowerLeft, true);
                if (promptDeAumentarPotencia != null){
                    manager.AddScreenPrompt(promptDeAumentarPotencia, PromptPosition.LowerLeft, true);
                }
                manager.AddScreenPrompt(promptDeFreelook, PromptPosition.UpperRight, true);
                manager.AddScreenPrompt(valorDaPotencia, PromptPosition.BottomCenter, true);
            }
        }

        private void Update()
        {
            if (OWInput.IsNewlyPressed(InputLibrary.cancel, InputMode.All))
            {
                attachPoint.DetachPlayer();
                playerAudio.PlayOneShotInternal(AudioType.ShipCockpitUnbuckle);
                interactVolume.EnableInteraction();
                interactVolume.ResetInteraction();
                enabled = false;
                OnExitNaveFlightConsole?.Invoke();

                PromptManager manager = Locator.GetPromptManager();

                manager.RemoveScreenPrompt(promptDeAtivar);
                manager.RemoveScreenPrompt(promptDeReduzirPotencia);
                if (promptDeAumentarPotencia != null)
                {
                    manager.RemoveScreenPrompt(promptDeAumentarPotencia);
                }
                manager.RemoveScreenPrompt(promptDeFreelook);
                manager.RemoveScreenPrompt(valorDaPotencia);
            }
            if (naveThrusterController != null)
            {
                valorDaPotencia.SetText(PowerPrompt + naveThrusterController.Potencia / 10 + '%');
            }
        }
    }
}
