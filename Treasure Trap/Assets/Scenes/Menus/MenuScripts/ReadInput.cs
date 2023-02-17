using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadInput : MonoBehaviour
{
    public InputField display;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Create()
    {
        if (display.text == ""){
            Debug.Log("You");
            PlayerPrefs.SetString("Username", "You");
        }
        else {
            Debug.Log(display.text);
            PlayerPrefs.SetString("Username", display.text);
        } 
    }
}
