using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");   // or your first scene name
    }

    private void Start()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    //  unneeded. web app.
    // public void QuitGame()
    // {
    //     Application.Quit();
    // }
}
