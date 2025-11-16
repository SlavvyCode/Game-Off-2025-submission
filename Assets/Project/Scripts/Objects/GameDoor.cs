using System;
using UnityEngine;

public class GameDoor : LeverTarget
{
    private Collider2D doorCollider;
    private SpriteRenderer doorSpriteRenderer;
    bool isOpen = false;
    private void Awake()
    {
        doorCollider = GetComponent<Collider2D>();
        doorSpriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void ToggleOpenClose()
    {
        if (isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    public void Open()
    {
        //play SFX AND VFX
        //and disappear
     
        isOpen = true;
        
        doorSFX();
        doorVFX();
        
        doorCollider.enabled = false;
        doorSpriteRenderer.enabled = false;
    }
    
    private void doorSFX()
    {
        // throw new NotImplementedException();
    }
    
    private void doorVFX()
    {
        // throw new NotImplementedException();
    }
    
    
    public void Close()
    {
        isOpen = false;
        //reappear
        doorCollider.enabled = true;
        doorSpriteRenderer.enabled = true;
    }

    public override void GetUsedByButton()
    {
        ToggleOpenClose();
    }
}
