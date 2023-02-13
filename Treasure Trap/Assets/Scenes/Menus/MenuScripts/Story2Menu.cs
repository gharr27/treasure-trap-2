using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Story2Menu : MonoBehaviour
{
    public void GoToQuickgame()
    {
        SceneManager.LoadScene("Quickgame");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToStoryStrip1()
    {
        SceneManager.LoadScene("StoryStrip1");
    }
}
