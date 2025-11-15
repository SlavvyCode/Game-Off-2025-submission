using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static PlayerInputActions Controls;

    void Awake()
    {
        Controls = new PlayerInputActions();
        Controls.Enable();
    }
}
