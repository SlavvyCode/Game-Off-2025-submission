using UnityEngine;
using UnityEngine.InputSystem;

public class Scream : MonoBehaviour
{
    public GameObject SoundWave;
    private PlayerInput _playerInput;

    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
    }
    // Update is called once per frame
    void Update()
    {
        if (_playerInput.actions["Jump"].WasPressedThisFrame())
        {
            MakeScream();
        }
    }

    void MakeScream()
    {
        Instantiate(SoundWave, transform.position, Quaternion.identity);
    }
}
