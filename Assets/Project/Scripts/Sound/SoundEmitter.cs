namespace Project.Scripts.Sound
{
    using UnityEngine;

    public class SoundEmitter : MonoBehaviour
    {
        AudioSource src;

        private void Awake()
        {
            src = GetComponent<AudioSource>();
        }

        public void Play(SoundData data, Vector2 position)
        {
            transform.position = position;

            src.clip = data.clip;
            src.volume = data.volume;
            src.spatialBlend = data.spatialBlend;   
            src.dopplerLevel = 0f;                  

            if (data.echoAmount > 0)
            {
                var echo = src.gameObject.AddComponent<AudioEchoFilter>();
                echo.delay = 120f;
                echo.decayRatio = data.echoAmount; 
                echo.wetMix = data.echoAmount;
                echo.dryMix = 1f;
            }

            src.Play();
            Destroy(gameObject, src.clip.length + 0.1f);
        }
    }

}