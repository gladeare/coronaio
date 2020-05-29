using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void gameStart()
    {

        SceneManager.LoadScene("IOMainScene");
    }

    public void gameQuit()
    {
        Application.Quit();
        Debug.Log("Game Quit!");
    }

    public void loadTutorial()
    {
        SceneManager.LoadScene("IOTutorial");
        Debug.Log("Tutorial Loaded");
    }

    public void loadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
