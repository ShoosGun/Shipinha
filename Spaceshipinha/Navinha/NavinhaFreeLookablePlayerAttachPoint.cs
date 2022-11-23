using SlateShipyard.PlayerAttaching;

namespace Spaceshipinha.Navinha
{
    public class NavinhaFreeLookablePlayerAttachPoint : FreeLookablePlayerAttachPoint
    {
        public NaveFlightConsole naveFlightConsole;
        public void Awake() 
        {
            GetComponent<InteractZone>().ChangePrompt("Seat On");
            AllowFreeLook = () =>
            {
                return naveFlightConsole.enabled;
            };
        }
    }
}
