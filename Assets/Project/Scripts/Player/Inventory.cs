using System;
using System.Collections.Generic;
using Project.Scripts.Objects;
using UnityEngine;

public class Inventory : MonoBehaviour
{
   
    public Dictionary<KeyType,int> keys = new Dictionary<KeyType, int>();


    private void Start()
    {
        LoadKeys();
        HUDManager.Instance.UpdateKeys(this);
    }

    public void AddKey(KeyType type)
    {
        if (!keys.ContainsKey(type))
            keys[type] = 0;

        keys[type]++;
        PlayerPrefs.SetInt("key_" + type.ToString(), keys[type]);
        HUDManager.Instance.UpdateKeys(this);

    }
    private void LoadKeys()
    {
        foreach (KeyType type in Enum.GetValues(typeof(KeyType)))
        {
            int count = PlayerPrefs.GetInt("key_" + type.ToString(), 0);
            if (count > 0)
                keys[type] = count;
        }
    }

    
    public bool HasKey(KeyType type)
    {
        return keys.ContainsKey(type) && keys[type] > 0;
    }

    public bool HasKeyToLever(InteractableLever lever)
    {
        return HasKey(lever.requiredKeyType);
    }
    

    public void RemoveKey(KeyType type)
    {
        if (!HasKey(type))
            return;

        keys[type]--;
        if (keys[type] < 0)
            keys[type] = 0;
        PlayerPrefs.SetInt("key_" + type.ToString(), keys[type]);

        if (keys[type] <= 0)
            keys.Remove(type);
        HUDManager.Instance.UpdateKeys(this);

    }
    
    public bool RemoveKeyIfHas(KeyType type)
    {
        if (!HasKey(type))
            return false;

        RemoveKey(type);
        return true;
    }
}
