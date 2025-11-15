using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{


    public void GetInteractedWith()
    {
        Debug.Log("TestInteractable has been interacted with!");
    }
}
