using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Story1Menu : MonoBehaviour
{
    public void GoToStoryStrip2()
    {
        SceneManager.LoadScene("StoryStrip2");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
