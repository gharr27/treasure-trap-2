using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Screens' names

CharSelection
GameBoard
GameTitleIntro
LogoIntro
LoserScreen
MainMenu
StoryStrip1
StoryStrip2
Tutorial
WinnerScreen 
*/

public class MenusManager : MonoBehaviour
{
    public void GoToLogoIntro()
    {
        SceneManager.LoadScene("LogoIntro");
    }

    public void GoToGameTitleIntro()
    {
        SceneManager.LoadScene("GameTitleIntro");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToTutorial()
    {
        SceneManager.LoadScene("TutorialRules");
    }

    public void GoToStoryStrip1()
    {
        SceneManager.LoadScene("StoryStrip1");
    }

    public void GoToStoryStrip2()
    {
        SceneManager.LoadScene("StoryStrip2");
    }

    public void GoToCharSelection()
    {
        SceneManager.LoadScene("CharSelection");
    }

    public void GoToGameBoard()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void GoToLoserScreen()
    {
        SceneManager.LoadScene("LoserScreen");
    }

    public void GoToLoserScreenNetwork()
    {
        SceneManager.LoadScene("LoserScreenNetwork");
    }

    public void GoToWinnerScreenAI()
    {
        SceneManager.LoadScene("WinnerScreenAI");
    }

    public void GoToWinnerScreenNetwork()
    {
        SceneManager.LoadScene("WinnerScreenNetwork");
    }

    public void GoToLoadingScreen()
    {
        SceneManager.LoadScene("Loading");
    }

    public void GoToWavesScreen()
    {
        SceneManager.LoadScene("LoadingScreenWaves");
    } 

    public void GoToAIGame()
    {
        SceneManager.LoadScene("AIGame");
    }

    public void DisconnectScreen()
    {
        SceneManager.LoadScene("DisconnectScreen");
    }

    public void ExitGame()
    {
        Debug.Log("Your Application is closed");
        //PlayerPrefs.DeleteAll();
        Application.Quit();
    }

}
