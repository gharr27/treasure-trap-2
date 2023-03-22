using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;


public class ConnectServer : MonoBehaviourPunCallbacks
{
    public static ConnectServer Instance;

	[SerializeField] TMP_InputField roomNameInputField;
	[SerializeField] TMP_InputField userNameInput;
	[SerializeField] TMP_Text errorText;
	[SerializeField] TMP_Text roomNameText;
	[SerializeField] Transform roomListContent;
	[SerializeField] GameObject roomListItemPrefab;
	[SerializeField] Transform playerListContent;
	[SerializeField] GameObject PlayerListItemPrefab;
	[SerializeField] GameObject startGameButton;
	RoomOptions roomOptions = new RoomOptions();


	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		Debug.Log("Connecting to Master");
		PhotonNetwork.ConnectUsingSettings();
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log("Connected to Master");
		PhotonNetwork.JoinLobby();
		PhotonNetwork.AutomaticallySyncScene = true;
	}

	public override void OnJoinedLobby()
	{
		MenuManager.Instance.OpenMenu("title");
		Debug.Log("Joined Lobby");
	}

	public void CreateRoom()
	{

        Debug.Log("Created room");
		if(string.IsNullOrEmpty(roomNameInputField.text))
		{
			return;
		}
		Debug.Log("Input room info");
        //only 2 players can connect
		roomOptions.MaxPlayers = 2;
		PhotonNetwork.CreateRoom(roomNameInputField.text, roomOptions);
		MenuManager.Instance.OpenMenu("loading");
		roomNameInputField.text = "";
	}

	public override void OnJoinedRoom()
	{
		MenuManager.Instance.OpenMenu("room");
		roomNameText.text = PhotonNetwork.CurrentRoom.Name;
		
		Debug.Log("User name input: " + userNameInput.text);
		PhotonNetwork.NickName = userNameInput.text + " " + Random.Range(0, 1000).ToString("0000");
		
		Photon.Realtime.Player[] photonPlayers = PhotonNetwork.PlayerList;

		foreach (Transform child in playerListContent)
		{
			Destroy(child.gameObject);
		}

		for (int i = 0; i < photonPlayers.Length; i++)
		{
			Debug.Log("Instantiating");
			Debug.Log(photonPlayers[i]);
			Instantiate(PlayerListItemPrefab, playerListContent)
				.GetComponent<PlayerListItem>()
				.SetUp(photonPlayers[i]);
		}
		
		startGameButton.SetActive(PhotonNetwork.IsMasterClient);
	}

	public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
	{
		startGameButton.SetActive(PhotonNetwork.IsMasterClient);
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		errorText.text = "Room Creation Failed: " + message;
		Debug.LogError("Room Creation Failed: " + message);
		MenuManager.Instance.OpenMenu("error");
	}

	public override void OnJoinRoomFailed(short returnCode, string message)
{
    // Get the error message
    string errorMessage = message;
    
    // Display the error message
    // Debug.LogError(errorMessage);
	MenuManager.Instance.OpenMenu("error");
}


	public void StartGame()
	{
		PhotonNetwork.LoadLevel(1);
	}

	public void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom();
		MenuManager.Instance.OpenMenu("loading");
	}

	public void JoinRoom(RoomInfo info)
	{
		PhotonNetwork.JoinRoom(info.Name);
		MenuManager.Instance.OpenMenu("loading");
	}

	public override void OnLeftRoom()
	{
		MenuManager.Instance.OpenMenu("title");
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		//clear list every time we update
		foreach(Transform trans in roomListContent)
		{
			Destroy(trans.gameObject);
		}

		for(int i = 0; i < roomList.Count; i++)
		{
			Debug.Log("Room updated");
			if(roomList[i].RemovedFromList)
				continue;
			Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
		}
	}

	public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
	{
		  // userNameInput.text = newPlayer.NickName; // 
		Debug.Log("Player entered roomm");
		Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
		
	}
}
