using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class Rooms : MonoBehaviour
{
    
    [SerializeField]
    private CreateOrJoinRoom createOrJoinRoom;
    public CreateOrJoinRoom CreateOrJoinRoom {get { return createOrJoinRoom; }}
    
     [SerializeField]
    private CurrentRoom currentRoom;
    public CurrentRoom CurrentRoom {get { return currentRoom; }}


    private void Awake(){
        FirstInitialize();
    }

    private void FirstInitialize(){
        CreateOrJoinRoom.FirstInitialize(this);
        currentRoom.FirstInitialize(this);
    }
}
