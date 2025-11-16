using System.Collections.Generic;
using Project.Scripts.Objects;
using UnityEngine;

public class Inventory : MonoBehaviour
{
   
    private Dictionary<KeyType,int> keys = new Dictionary<KeyType, int>();
    
    public void AddKey(KeyType type)
    {
        if (!keys.ContainsKey(type))
            keys[type] = 0;

        keys[type]++;
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

        if (keys[type] <= 0)
            keys.Remove(type);
    }
    
    public bool RemoveKeyIfHas(KeyType type)
    {
        if (!HasKey(type))
            return false;

        RemoveKey(type);
        return true;
    }
}
