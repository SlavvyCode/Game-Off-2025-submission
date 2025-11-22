using System;
using Project.Scripts.Objects;
using Project.Scripts.Sound;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableLever : MonoBehaviour, IInteractable
{
    
    
    private SaveableObject saveID;
    
    [Header("Key Settings")]
    [SerializeField] private bool requiresKey = false;
    [SerializeField] public KeyType requiredKeyType;

    [Header("Lever Visuals")]
    [SerializeField] private Sprite leverOffSprite;
    [SerializeField] private Sprite leverOnSprite;

    [Header("Linked Target")]
    [SerializeField] private LeverTarget pairedObject;
    
    [SerializeField] private SoundData leverSound;

    private SpriteRenderer sr;
    private bool isOn = false;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        saveID = GetComponent<SaveableObject>();
        UpdateLeverSprite();
    }

    private void Start()
    {
        
        LoadState();
        UpdateLeverSprite();  
    }

    private void UpdateLeverSpriteColor()
    {
        // make it match the key color if it requires a key
        if (requiresKey)
        {
            Color keyColor = KeyItem.GetColorForKeyType(requiredKeyType);
            sr.color = keyColor;
        }
    }

    public void GetInteractedWith(Inventory playerInventory = null)
    {
        if (!CanUse(playerInventory))
        {
            Debug.Log("Lever is locked. You need the correct key.");
            return;
        }
        
        ToggleLever();
    }

    private bool CanUse(Inventory inv)
    {
        if (!requiresKey)
            return true;

        if (inv == null)
            return false;

        if (!inv.HasKey(requiredKeyType))
            return false;

        return true;
    }

    private void ToggleLever()
    {
        isOn = !isOn;
        UpdateLeverSprite();
        SaveState();
        AudioManager.Instance.PlaySound(leverSound, transform.position);

        if (pairedObject != null)
            pairedObject.GetUsedByButton();
        else
            Debug.LogWarning("Lever has no paired object!");
    }
  
    private void SaveState()
    {
        PlayerPrefs.SetInt(saveID.UniqueID, isOn ? 1 : 0);
    }

    private void LoadState()
    {
        if (PlayerPrefs.HasKey(saveID.UniqueID))
            isOn = PlayerPrefs.GetInt(saveID.UniqueID) == 1;
        

        if (isOn && pairedObject != null)
            pairedObject.GetUsedByButton();
    }
    private void UpdateLeverSprite()
    {
        sr.sprite = isOn ? leverOnSprite : leverOffSprite;
        UpdateLeverSpriteColor();
    }

    public void Use(GameObject player)
    {
        Inventory playerInventory = player.GetComponent<Inventory>();
        
        if(CanUse(playerInventory))
        {
            ToggleLever();
            HUDManager.Instance.ShowMessage("Lever used.");
        }
        else
        {
            Debug.Log("Cannot use lever, key required.");
        }
        
    }
}