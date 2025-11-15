using System.Collections.Generic;
using Project.Scripts.Objects;
using UnityEngine;

public class Inventory : MonoBehaviour
{
   
    private List<KeyItem> keys = new List<KeyItem>();
    
    public void AddKey(KeyItem key)
    {
        keys.Add(key);
    }
    
    public bool HasKey(KeyItem key)
    {
        return keys.Contains(key);
    }
    
    public KeyItem HasKeyToButton(InteractableButton button)
    {
        var desiredKey = keys.Find(k => k.pairedButton == button);
        if (desiredKey == null)
        {
            return null;
        }
        return desiredKey;
        
    }
    
    public void RemoveKey(KeyItem key)
    {
        keys.Remove(key);
    }
    
    public bool RemoveKeyIfHas(KeyItem key)
    {
        if (HasKey(key))
        {
            RemoveKey(key);
            return true;
        }
        return false;
    }
}
