using System.Collections.Generic;
using Project.Scripts.Sound;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/SoundSet")]
public class SoundSet : ScriptableObject
{
    public List<SoundData> variations;

    public SoundData GetRandom()
    {
        if (variations == null || variations.Count == 0)
            return null;

        int index = Random.Range(0, variations.Count);
        return variations[index];
    }
}