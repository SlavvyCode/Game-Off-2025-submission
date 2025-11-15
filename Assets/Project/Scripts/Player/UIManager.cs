using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

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

