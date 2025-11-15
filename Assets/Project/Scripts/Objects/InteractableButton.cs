using Project.Scripts.Objects;
using UnityEngine;

public class InteractableButton : MonoBehaviour,IInteractable
{
    
    [SerializeField] bool requiresKey = false;
    bool foundKey = false;
    [SerializeField] private ButtonTarget pairedObject;

    private SpriteRenderer buttonSprite;
    
    public void Use()
    {
        
        if(requiresKey && !foundKey)
        {
            return;
        }
        
        if (pairedObject == null) Debug.LogWarning("No paired object assigned to the button.");

        pairedObject.GetUsedByButton();
        
    }
  
    
    public void UseWithKey(Inventory playerInventory)
    {
        //find the key in the inventory that matches the required key item
        KeyItem key = playerInventory.HasKeyToButton(this);
        bool foundKey = key != null;
        if(!requiresKey)
        {
            Use();
            return;
        }
        
        if (foundKey)
        {
            Use();
        }
        else
        {
            Debug.Log("Incorrect key used on the button.");
        }
    }
    
}
