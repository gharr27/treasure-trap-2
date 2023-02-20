using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class RoomListing : MonoBehaviour
{

    [SerializeField]
    private Text roomText;

    public RoomInfo RoomInfo {get; private set;}


    public void SetRoomInfo(RoomInfo roomInfo){

        RoomInfo = roomInfo;
        roomText.text = roomInfo.MaxPlayers + ", " + roomInfo.Name;
    }

    public void OnClick_Button(){

        PhotonNetwork.JoinRoom(RoomInfo.Name);
    }
   
}
