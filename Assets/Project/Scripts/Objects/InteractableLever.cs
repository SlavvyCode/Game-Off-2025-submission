using Project.Scripts.Objects;
using UnityEngine;

public class InteractableLever : MonoBehaviour, IInteractable
{
    [Header("Key Settings")]
    [SerializeField] private bool requiresKey = false;
    [SerializeField] public KeyType requiredKeyType;

    [Header("Lever Visuals")]
    [SerializeField] private Sprite leverOffSprite;
    [SerializeField] private Sprite leverOnSprite;

    [Header("Linked Target")]
    [SerializeField] private LeverTarget pairedObject;

    private SpriteRenderer sr;
    private bool isOn = false;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        UpdateLeverSprite();
        UpdateLeverSpriteColor();
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

        if (pairedObject != null)
            pairedObject.GetUsedByButton();
        else
            Debug.LogWarning("Lever has no paired object!");
    }

    private void UpdateLeverSprite()
    {
        sr.sprite = isOn ? leverOnSprite : leverOffSprite;
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