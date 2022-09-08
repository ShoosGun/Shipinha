using OWML.ModHelper;
using UnityEngine;
using Spaceshipinha.Navinha;
using OWML.Common;

namespace Spaceshipinha
{
    public class Spaceshipinha : ModBehaviour
    {
        public static GameObject navinhaPrefab;
        public static AudioClip navinhaThrusterAudio;
        public static IModHelper modHelper;
        private void Start()
        {
            AssetBundle bundle = ModHelper.Assets.LoadBundle("AssetBundles/navinha");

            navinhaPrefab = bundle.LoadAsset<GameObject>("navinha_bodyv2.prefab");
            navinhaThrusterAudio = bundle.LoadAsset<AudioClip>("naveThrusterAudio.wav");

            modHelper = ModHelper;
            gameObject.AddComponent<NaveSpawn>();
        }

    }
}
