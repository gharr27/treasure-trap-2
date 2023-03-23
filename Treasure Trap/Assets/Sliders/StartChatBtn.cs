

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartChatBtn : MonoBehaviour
{
    public Animator startBtnAnimator;

    private bool slideIn = false;

    public void startSliding(){

        Debug.Log("Start sliding");
        if(slideIn == false){
            slideIn = true;
            startBtnAnimator.SetBool("SlideIn", slideIn);
        }
        else if(slideIn == true){
            slideIn = false;
            startBtnAnimator.SetBool("SlideIn", slideIn);
        }
    }

}

