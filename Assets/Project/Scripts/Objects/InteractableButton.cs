using UnityEngine;

public class InteractableButton : MonoBehaviour,IInteractable
{

    [SerializeField] private ButtonTarget pairedObject;

    private SpriteRenderer buttonSprite;
    
    public void GetInteractedWith()
    {
        if (pairedObject == null) Debug.LogWarning("No paired object assigned to the button.");

        pairedObject.GetUsedByButton();
        
    }
}
