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

	RoomInfo info;

	public void SetUp(RoomInfo _info)
	{
		Debug.Log(_info + " Input room info");
		info = _info;
		text.text = _info.Name;
	}

	public void OnClick()
	{
	
		ConnectServer.Instance.JoinRoom(info);
		
	}
}
