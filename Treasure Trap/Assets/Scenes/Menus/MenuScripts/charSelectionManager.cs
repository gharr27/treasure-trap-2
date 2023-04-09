using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class charSelectionManager : MonoBehaviour
{
    public Image ghostChar;
    public Image cowChar;
    public Image kidChar;
    public Image queenChar;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetString("Character") == "Option1 _ Ghost") {
            ghostChar.enabled = true;
            cowChar.enabled = false;
            kidChar.enabled = false;
            queenChar.enabled = false;
        }
        else if (PlayerPrefs.GetString("Character") == "Option2 _ Cow") {
            ghostChar.enabled = false;
            cowChar.enabled = true;
            kidChar.enabled = false;
            queenChar.enabled = false;
        }
        else if (PlayerPrefs.GetString("Character") == "Option3 _ Kid" ){
            ghostChar.enabled = false;
            cowChar.enabled = false;
            kidChar.enabled = true;
            queenChar.enabled = false;
        }
        else if (PlayerPrefs.GetString("Character") == "Option4 _ Queen" ){
            ghostChar.enabled = false;
            cowChar.enabled = false;
            kidChar.enabled = false;
            queenChar.enabled = true;
        }
    }
}
