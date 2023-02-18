using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class CreateOrJoinRoom : MonoBehaviour
{
    [SerializeField]
    private CreateRoom createRoom;
    [SerializeField]
    private RoomListingMenu roomListingMenu;
    private Rooms roomsCanvas;

    public void FirstInitialize(Rooms canvas){
        roomsCanvas = canvas;
        createRoom.FirstInitialize(canvas);
        roomListingMenu.FirstInitialize(canvas);
   }
}
