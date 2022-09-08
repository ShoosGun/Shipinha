﻿using UnityEngine;

namespace Spaceshipinha.Navinha
{
    internal class NaveThrusterController : ThrusterController
    {
        public int Potencia { get; private set; } = 0; //Em "por mil"
        public NaveFlightConsole naveFlightConsole;

        public override void Awake()
        {
            base.Awake();
            enabled = false;
        }
        public void Innit() 
        {
            naveFlightConsole.OnEnterNaveFlightConsole += OnEnterNaveFlightConsole;
            naveFlightConsole.OnExitNaveFlightConsole += OnExitNaveFlightConsole;
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            naveFlightConsole.OnEnterNaveFlightConsole -= OnEnterNaveFlightConsole;
            naveFlightConsole.OnExitNaveFlightConsole -= OnExitNaveFlightConsole;
        }
        public override void Update()
        {
            Potencia += (int)((OWInput.GetValue(InputLibrary.thrustUp, InputMode.All) - OWInput.GetValue(InputLibrary.thrustDown, InputMode.All)) * 10);
            Potencia = Mathf.Clamp(Potencia, 0, 1000);


            if (OWInput.IsNewlyPressed(InputLibrary.rollMode, InputMode.All))
            {
                _isRollMode = !_isRollMode;
                _isRollPressed = true;
            }
            if (_isRollPressed && OWInput.IsNewlyReleased(InputLibrary.rollMode, InputMode.All))
            {
                _isRollPressed = false;
                _isRollMode = !_isRollMode;
            }
        }
        public bool IsThrusterOn()
        {
            return OWInput.GetValue(InputLibrary.thrustZ, InputMode.All) > 0.1f;
        }
        public override Vector3 ReadTranslationalInput()
        {
            if (IsThrusterOn())
                return new Vector3(0f, 0f, Potencia / 1000f);

            return Vector3.zero;
        }

        public override Vector3 ReadRotationalInput()
        {
            //Rodar na horizontal eh 8x menos efetivo que rolar (1.25f de aceleracao)
            float pitchInput = -OWInput.GetValue(InputLibrary.pitch, InputMode.All);
            float roolAndYawInput = -OWInput.GetValue(InputLibrary.yaw, InputMode.All);
            if (IsRollMode())
            {
                return new Vector3(pitchInput, 0f, roolAndYawInput);
            }
            else if (_isRollMode) 
            {
                return new Vector3(pitchInput, 0f, roolAndYawInput);
            }
            else 
            {
                return new Vector3(pitchInput, -roolAndYawInput, 0f); 
            }
        }

        private void OnEnterNaveFlightConsole()
        {
            enabled = true;
            OWInput.ChangeInputMode(InputMode.ShipCockpit);
        }

        private void OnExitNaveFlightConsole()
        {
            OWInput.RestorePreviousInputs();
            enabled = false;
        }
    }
}