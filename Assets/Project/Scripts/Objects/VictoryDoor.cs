using System;
using General_and_Helpers;
using Project.Scripts.UI;
using UnityEngine;

public class VictoryDoor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.CompareTag("Player"))
        {
            UIManager.instance.ShowVictoryScreen();
            
        }
        
    }
}
