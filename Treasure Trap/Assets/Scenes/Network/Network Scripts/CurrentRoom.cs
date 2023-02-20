using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentRoom : MonoBehaviour
{
    private Rooms roomsCanvas;
    public void FirstInitialize(Rooms canvas){

        roomsCanvas= canvas;
   }

   public void Show(){

    gameObject.SetActive(true);
   }

   private void Hide(){
    gameObject.SetActive(false);

   }
}
