using UnityEngine;

namespace Spaceshipinha.Navinha
{
    internal class NaveBody : OWRigidbody
    {
        private bool _isPlayerAtFlightConsole;
        public NaveFlightConsole naveFlightConsole;

        public void Innit() 
        {
            naveFlightConsole.OnEnterNaveFlightConsole += OnEnterFlightConsole;
            naveFlightConsole.OnExitNaveFlightConsole += OnExitFlightConsole;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            naveFlightConsole.OnEnterNaveFlightConsole -= OnEnterFlightConsole;
            naveFlightConsole.OnExitNaveFlightConsole -= OnExitFlightConsole;
        }
        private void OnEnterFlightConsole()
        {
            _isPlayerAtFlightConsole = true;
        }

        private void OnExitFlightConsole()
        {
            _isPlayerAtFlightConsole = false;
        }

		public override void SetPosition(Vector3 worldPosition)
		{
			if (_isPlayerAtFlightConsole)
			{
				base.SetPosition(worldPosition);
				GlobalMessenger.FireEvent("PlayerRepositioned");
				return;
			}
			base.SetPosition(worldPosition);
		}

		public override void SetRotation(Quaternion rotation)
		{
			base.SetRotation(rotation);
		}

		public override void SetVelocity(Vector3 newVelocity)
		{
			base.SetVelocity(newVelocity);
		}
	}
}
