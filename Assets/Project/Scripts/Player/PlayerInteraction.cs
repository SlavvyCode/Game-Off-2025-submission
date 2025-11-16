using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    
    // Rigidbody2D rb;
    Collider2D playerCollider;
    PlayerInputActions inputActions;
    private PlayerInput _playerInput;
    void Start()
    {
        playerCollider = GetComponent<Collider2D>();
        _playerInput = GetComponent<PlayerInput>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerInput.actions["Interact"].WasPressedThisFrame())
        {
            Debug.Log("Interact pressed");
            //check for interactables in range of a multiplier of the player's collider size
            var playerRadius = playerCollider.bounds.extents.magnitude;
            var playerReach = playerRadius * 4f;
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position,playerReach);

            foreach (var hitCollider in hitColliders)
            {
                //check for colliders with iinteractable component
                var interactable = hitCollider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Use(gameObject);
                    break; 
                }
            }
        }
        
        
    }
}
