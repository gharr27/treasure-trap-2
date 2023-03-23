using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text text;

	public RoomInfo info;

	public void SetUp(RoomInfo _info)
	{
		Debug.Log("Input room info");
		info = _info;
		text.text = _info.Name;
	}

	public void OnClick()
	{
		// RoomOptions roomOptions = new RoomOptions();
		// if(ConnectServer.Instance.MaxPlayers > 2){
		// 	Debug.Log("Only 2 players per game");
		// }
		// else{
			ConnectServer.Instance.JoinRoom(info);
		// }
		
	}
}
