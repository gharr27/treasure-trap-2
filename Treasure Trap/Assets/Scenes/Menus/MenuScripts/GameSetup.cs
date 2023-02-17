using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameSetup : MonoBehaviour
{
    public TextMeshProUGUI username;
    public TextMeshProUGUI difficulty;
    public TextMeshProUGUI whoStarts;
    public Image charRed;
    public Image charGreen;
    public Image charBlue;

    // Start is called before the first frame update
    void Start()
    {
        username.text = PlayerPrefs.GetString("Username");
        difficulty.text = PlayerPrefs.GetString("Difficulty");
        whoStarts.text = PlayerPrefs.GetString("WhoStarts");

        if (PlayerPrefs.GetString("Character") == "Option1 _ Red") {
            charGreen.enabled = false;
            charBlue.enabled = false;
        }
        else if (PlayerPrefs.GetString("Character") == "Option2 _ Green") {
            charRed.enabled = false;
            charBlue.enabled = false;
        }
        else if (PlayerPrefs.GetString("Character") == "Option3 _ Blue" ){
            charRed.enabled = false;
            charGreen.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
