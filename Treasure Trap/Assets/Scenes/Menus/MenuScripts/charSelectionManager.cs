using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class charSelectionManager : MonoBehaviour
{

    public Image charRed;
    public Image charGreen;
    public Image charBlue;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetString("Character") == "Option1 _ Red") {
            charRed.enabled = true;
            charGreen.enabled = false;
            charBlue.enabled = false;
        }
        else if (PlayerPrefs.GetString("Character") == "Option2 _ Green") {
            charGreen.enabled = true;
            charRed.enabled = false;
            charBlue.enabled = false;
        }
        else if (PlayerPrefs.GetString("Character") == "Option3 _ Blue" ){
            charBlue.enabled = true;
            charRed.enabled = false;
            charGreen.enabled = false;
        }
    }
}
