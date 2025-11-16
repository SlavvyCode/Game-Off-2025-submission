using System.Collections;
using Project.Scripts.Objects;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [SerializeField] private TMP_Text rockCountText;
    [SerializeField] private TMP_Text messageText;
    private GameObject messageBox;
    [SerializeField] float messageDisplayDuration = 2f;


    [SerializeField] private GameObject HUDKeyContainer;
    [SerializeField] private GameObject HUDKeyImagePrefab;
    private void Awake()
    {
        Instance = this;
    }

    
    public void UpdateKeys(Inventory inventory)
    {
        // Clear container
        foreach (Transform child in HUDKeyContainer.transform)
            Destroy(child.gameObject);

        // Add icons for each key type the player has
        foreach (var pair in inventory.keys)
        {
            KeyType keyType = pair.Key;
            int count = pair.Value;

            if (count <= 0)
                continue;

            GameObject keyIcon = Instantiate(HUDKeyImagePrefab, HUDKeyContainer.transform);

            var keyImage = keyIcon.GetComponent<Image>();
            keyImage.color = KeyItem.GetColorForKeyType(keyType);

            // show count if >1
            // TMP_Text countText = keyIcon.GetComponentInChildren<TMP_Text>(true);
            // if (countText != null)
                // countText.text = count > 1 ? $"x{count}" : "";
        }
    }

    public void UpdateRockCount(int count)
    {
        rockCountText.text = count.ToString();
    }
    
    public void ShowMessage(string message)
    {
        StopAllCoroutines();
        messageText.text = message;
        messageText.gameObject.SetActive(true);
        messageBox = messageText.gameObject.transform.parent.gameObject;
        messageBox.SetActive(true);
        StartCoroutine(HideMessageAfterDelay(messageDisplayDuration));
    }

    private IEnumerator HideMessageAfterDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        messageText.gameObject.SetActive(false);
        messageBox.SetActive(false);
    }
}

