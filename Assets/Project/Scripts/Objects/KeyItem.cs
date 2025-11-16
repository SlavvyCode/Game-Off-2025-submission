using System;
using Project.Scripts.Sound;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Scripts.Objects
{
    public enum KeyType
    {
        Red,
        Blue,
        Green,
        Yellow,
        White
    }
    public class KeyItem : MonoBehaviour
    {
        private SaveableObject saveID;
        
        private Collider2D keyCollider;
        [SerializeField]
        public KeyType keyType = KeyType.Red;
        SpriteRenderer spriteRenderer;
        [SerializeField] private SoundData keyPickupSound;

        private void Awake()
        {
            saveID = GetComponent<SaveableObject>();
            LoadState();
        }

        private void LoadState()
        {
            if (PlayerPrefs.GetInt(saveID.UniqueID, 0) == 1)
            {
                // Already picked up in a previous session
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            // make renderer the color of the key type
            SetKeyColor();
            
            keyCollider = GetComponent<Collider2D>();
            if (keyCollider == null)
            {
                Debug.LogError("KeyItem: No Collider2D found on " + gameObject.name);
            }
         
            
        }

        private void SetKeyColor()
        {
            switch (keyType)
            {
                case KeyType.Red:
                    spriteRenderer.color = Color.red;
                    break;
                case KeyType.Blue:
                    spriteRenderer.color = Color.blue;
                    break;
                case KeyType.Green:
                    spriteRenderer.color = Color.green;
                    break;
                case KeyType.Yellow:
                    spriteRenderer.color = Color.yellow;
                    break;
                default:
                    spriteRenderer.color = Color.white;
                    break;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            //if other tag not player
            if (!other.CompareTag("Player"))
                return;
            
            // Save that collected.
            PlayerPrefs.SetInt(saveID.UniqueID, 1);

            Inventory playerInventory = other.GetComponent<Inventory>();
            if (playerInventory != null)
            {
                playerInventory.AddKey(this.keyType);
                Destroy(gameObject);
            }
            
            AudioManager.Instance.PlaySound(keyPickupSound, transform.position);
            
        }

        public static Color GetColorForKeyType(KeyType requiredKeyType)
        {
            switch (requiredKeyType)
            {
                case KeyType.Red:
                    return Color.red;
                case KeyType.Blue:
                    return Color.blue;
                case KeyType.Green:
                    return Color.green;
                case KeyType.Yellow:
                    return Color.yellow;
                case KeyType.White:
                    return Color.white;
                default:
                    return Color.white;
            }
        }
    }
}