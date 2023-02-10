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

    public void ExitGame()
    {
        Debug.Log("Your Application is Closed");
        Application.Quit();
    }
}
