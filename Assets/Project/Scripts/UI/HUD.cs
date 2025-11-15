using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [SerializeField] private TMP_Text rockCountText;  // or TMP_Text if using TMPro

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateRockCount(int count)
    {
        rockCountText.text = count.ToString();
    }
}

