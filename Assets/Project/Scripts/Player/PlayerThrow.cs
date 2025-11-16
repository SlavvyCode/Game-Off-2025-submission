using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Scripts.Player
{
    public class PlayerThrow : MonoBehaviour
    {
        private Collider2D playerCollider;
        private PlayerInput _playerInput;
        bool isAiming = false;
        [SerializeField] private int minThrowStrength = 10;
        [SerializeField] int throwStrength = 0;
        [SerializeField] int maxThrowStrength = 100;
        [SerializeField] public int rockCount = 0; 
        [SerializeField] private float throwConstant = 0.5f;
        
            
        void Start()
        {
            playerCollider = GetComponent<Collider2D>();
            _playerInput = GetComponent<PlayerInput>();   


            HUDManager.Instance.UpdateRockCount(rockCount);
        }
        
        void Update()
        {

            if (rockCount <= 0)
            {
                return;
            }
            
            isAiming = _playerInput.actions["Aim"].IsInProgress();
            

            if (isAiming)
            {
                ThrowLogic();
            }

        }

        public void AddRockToInventory()
        {
            rockCount++;
            HUDManager.Instance.UpdateRockCount(rockCount);
            Debug.Log($"Rock added to inventory. Total rocks: {rockCount}");
        }
        
        public void RemoveRockFromInventory()
        {
            if (rockCount > 0)
            {
                rockCount--;
                HUDManager.Instance.UpdateRockCount(rockCount);

                Debug.Log($"Rock removed from inventory. Total rocks: {rockCount}");
            }
            else
            {
                Debug.Log("No rocks to remove from inventory.");
            }
        }
        
        
        private void ThrowLogic()
        {
            if (_playerInput.actions["Throw"].WasPressedThisFrame())
            {
                Debug.Log("Throw action triggered");
                
                // throw in Direction, depending on how far the mouse is from the player, that indicates throw strength
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                Vector3 throwDirection = (mousePosition - transform.position).normalized;
                float distanceToMouse = Vector3.Distance(transform.position, mousePosition);
                throwStrength = Mathf.Clamp((int)distanceToMouse * 10, minThrowStrength, maxThrowStrength);
                
                
                if (rockCount > 0)
                {
                    RemoveRockFromInventory();
                    Debug.Log($"Throwing rock with strength {throwStrength} in direction {throwDirection}");
                    //todo instantiate rock prefab and throw it
                    GameObject rockObject = Instantiate(PrefabHolder.Instance.RockPrefab, transform.position + throwDirection * 1f, Quaternion.identity);
                    Rock rock = rockObject.GetComponent<Rock>();
                    rock.GetThrown(playerCollider, throwDirection * throwStrength * throwConstant);
                }
                else
                {
                    Debug.Log("No rocks left to throw!");
                }
                
            }
        }
    }
}