using System;
using System.Collections;
using Project.Scripts.Sound;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private SoundEmitter emitterPrefab;
    bool muted;

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

    private void Start()
    {
        muted = true;
        StartCoroutine(UnMuteAfterStart());
    }

    private IEnumerator UnMuteAfterStart()
    {
        yield return new WaitForSeconds(.5f);
        muted = false;
    }

    public void PlaySound(SoundData data, Vector2 position)
    {
        if (muted) return;
        var emitter = Instantiate(emitterPrefab);
        emitter.Play(data, position);
    }
    
    public void PlaySound(SoundData data, Vector2 position, float volumeMult)
    {
        var emitter = Instantiate(emitterPrefab);
        emitter.Play(data, position, volumeMult);
    }
    
    public void PlaySoundGlobal(SoundData data)
    {
        if (data == null || data.clip == null)
        {
            Debug.LogWarning("Attempted to play null sound globally.");
            return;
        }
        
        var go = new GameObject("GlobalSound_" + data.clip.name);
        var src = go.AddComponent<AudioSource>();

        src.clip = data.clip;
        src.volume = data.volume;
        src.spatialBlend = 0f; // force 2D
        src.Play();

        Destroy(go, src.clip.length + 0.1f);
    }

}
