using UnityEngine;

namespace Spaceshipinha.Navinha
{
    public class NavinhaOWAudioSource : OWAudioSource
    {
        public AudioClip Clip;

        public void OnEnable() 
        {
            clip = Clip;
        }
    }
}
