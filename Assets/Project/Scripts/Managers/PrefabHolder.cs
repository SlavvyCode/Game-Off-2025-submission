using UnityEngine;

public class PrefabHolder : MonoBehaviour
{
    public static PrefabHolder Instance { get; private set; }


    [SerializeField] private GameObject rockPrefab; 

    public GameObject RockPrefab => rockPrefab;     

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
