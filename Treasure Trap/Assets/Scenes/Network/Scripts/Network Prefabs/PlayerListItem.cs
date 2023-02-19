using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
	[SerializeField] TMP_Text text;
	[SerializeField] string playerName;
	Player player;
    

	public void SetUp(Player _player)
	{
		player = _player;
		text.text = playerName;
	}

	public void OnPlayerLeftRoom(Player otherPlayer)
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