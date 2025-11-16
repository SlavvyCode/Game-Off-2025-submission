using UnityEngine;
namespace Project.Scripts.Sound
{

    [CreateAssetMenu(menuName = "Audio/SoundData")]
    public class SoundData : ScriptableObject
    {
        public AudioClip clip;
        public float volume = 1f;

        // 0 = 2D stereo;
        // 1 = fully positional
        public float spatialBlend = 1f;

        public float echoAmount = 0f;
    }

}