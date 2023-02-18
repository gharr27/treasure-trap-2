using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class CreateRoom : MonoBehaviourPunCallbacks
{
 
    [SerializeField]
    private Text roomName;
    private Rooms roomsCanvas;
    public void FirstInitialize(Rooms canvas){

        roomsCanvas = canvas;
   }

    public void OnClick_CreateRoom(){


        if (!PhotonNetwork.IsConnected)
            return;

        // if(string.IsNullOrEmpty(roomName.text)){
        //  Debug.Log("No Room Name");
        //  return;
        // }
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom(roomName.text, options, TypedLobby.Default);
    }

    public override void OnCreatedRoom(){
        roomsCanvas.CurrentRoom.Show();
        
    }

    public override void OnCreateRoomFailed(short returnCode, string message){
      //show error
   }
}
