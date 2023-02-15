using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void GoToStoryStrip1()
    {
        SceneManager.LoadScene("StoryStrip1");
    }

    public void ExitGame()
    {
        Debug.Log("Your Application is Closed");
        Application.Quit();
    }

    public void GoToGameBoard()
    {
        SceneManager.LoadScene("GameBoard");
    }

    public void GoToQuickgame()
    {
        SceneManager.LoadScene("Quickgame");
    }
}
