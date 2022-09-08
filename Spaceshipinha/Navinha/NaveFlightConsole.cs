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

        private ScreenPrompt promptDeReduzirPotencia;

        private ScreenPrompt promptDeAumentarPotencia;

        public ScreenPrompt valorDaPotencia;
        public NaveThrusterController naveThrusterController;

        private string TurnOnPrompt = "Turn On";
        private string LowerPowerPrompt = "Lower Power";
        private string IncreasePowerPrompt = "Increase Power";
        private string PowerPrompt = "Power: ";

        public event Action OnEnterNaveFlightConsole;
        public event Action OnExitNaveFlightConsole;

        private void Awake()
        {
            enabled = false;
            promptDeAtivar = new ScreenPrompt(InputLibrary.thrustZ, TurnOnPrompt, 1);
            promptDeReduzirPotencia = new ScreenPrompt(InputLibrary.thrustDown, LowerPowerPrompt, 1);
            promptDeAumentarPotencia = new ScreenPrompt(InputLibrary.thrustUp, IncreasePowerPrompt, 1);
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

        private void OnPressInteract()
        {
            if (!enabled)
            {
                playerAudio.PlayOneShotInternal(AudioType.ShipCockpitBuckleUp);
                attachPoint.AttachPlayer();
                enabled = true;
                OnEnterNaveFlightConsole?.Invoke();
                Locator.GetPromptManager().AddScreenPrompt(promptDeAtivar, PromptPosition.LowerLeft, true);
                Locator.GetPromptManager().AddScreenPrompt(promptDeReduzirPotencia, PromptPosition.LowerLeft, true);
                Locator.GetPromptManager().AddScreenPrompt(promptDeAumentarPotencia, PromptPosition.LowerLeft, true);
                Locator.GetPromptManager().AddScreenPrompt(valorDaPotencia, PromptPosition.BottomCenter, true);
            }
        }

        private void Update()
        {
            if (OWInput.IsNewlyPressed(InputLibrary.cancel, InputMode.All))
            {
                attachPoint.DetachPlayer();
                playerAudio.PlayOneShotInternal(AudioType.ShipCockpitUnbuckle);
                interactVolume.ResetInteraction();
                enabled = false;
                OnExitNaveFlightConsole?.Invoke();
                Locator.GetPromptManager().RemoveScreenPrompt(promptDeAtivar);
                Locator.GetPromptManager().RemoveScreenPrompt(promptDeReduzirPotencia);
                Locator.GetPromptManager().RemoveScreenPrompt(promptDeAumentarPotencia);
                Locator.GetPromptManager().RemoveScreenPrompt(valorDaPotencia);
            }
            if (naveThrusterController != null)
            {
                valorDaPotencia.SetText(PowerPrompt + naveThrusterController.Potencia / 10 + '%');
            }
        }
    }
}
