using Project.Scripts.Sound;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private SoundEmitter emitterPrefab;

    // NOTE:
    // Use a direct AudioSource on the prefab when the sound is continuous
    // this is for oneshots.
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }

    public void PlaySound(SoundData data, Vector2 position)
    {
        var emitter = Instantiate(emitterPrefab);
        emitter.Play(data, position);
    }
}
