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
        SceneManager.LoadScene("Story1");
    }

    public void ExitGame()
    {
        Debug.Log("Your Application is Closed");
        Application.Quit();
    }
}
