using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [SerializeField] private TMP_Text rockCountText;
    [SerializeField] private TMP_Text messageText;
    private GameObject messageBox;
    [SerializeField] float messageDisplayDuration = 2f;
    private void Awake()
    {
        Instance = this;
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

