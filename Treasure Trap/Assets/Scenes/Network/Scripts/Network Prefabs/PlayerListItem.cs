using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
	//reference to text 
	[SerializeField] TMP_Text text;
	// [SerializeField] string playerName = "Player ";
	Photon.Realtime.Player player;

	public void SetUp(Photon.Realtime.Player _player)
	{
		player = _player;
		Debug.Log("received name" + _player.NickName);
		text.text = _player.NickName;
	}



	//player that left the room
	public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
	{
		if(player == otherPlayer)
		{
			Destroy(gameObject);
		}
	}

	public override void OnLeftRoom()
	{
		Destroy(gameObject);
	}
}