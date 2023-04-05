using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;
using UnityEngine.UI;


public class ConnectServer : MonoBehaviourPunCallbacks
{
    public static ConnectServer Instance;
    public bool playerEnteredRoom = false;
    public bool playerLeftRoom = false;
    public const byte UpdateRoomEventCode = 1;
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_InputField userNameInput;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject PlayerListItemPrefab;
    // [SerializeField] GameObject startGameButton;
    // [SerializeField] GameObject CreateRoom;

    RoomOptions roomOptions = new RoomOptions();
    public Button startGameButton;
    public Button CreateRoomBtn;
    public Button CreateRoomBtn2;
    public Button JoinRoomBtn;


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    }

    void Update()
    {
        if(string.IsNullOrEmpty(userNameInput.text))
        {
            CreateRoomBtn.interactable = false;
            JoinRoomBtn.interactable = false;
        }
        else{
            CreateRoomBtn.interactable = true;
            JoinRoomBtn.interactable = true;
        }

		if(string.IsNullOrEmpty(roomNameInputField.text))
        {
			CreateRoomBtn2.interactable = false;
        }
        else{
			CreateRoomBtn2.interactable = true;
        }

        if(playerEnteredRoom && playerLeftRoom == false){
            // Debug.Log("someone entered");
            UpdatePlayerList();
            startGameButton.interactable = (PhotonNetwork.CurrentRoom.PlayerCount > 1);

            // startGameButton.interactable = true;
        }
        else{
            startGameButton.interactable = false;
        }

        // Debug.Log("refreshing page");
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
        CreateRoomBtn.interactable = false;
        
        
    }

    // public void ifInputEmpty(){
    //  Debug.Log("input is empty");
    //  if(string.IsNullOrEmpty(userNameInput.text))
    //  {
    //      return;
    //  }
    //  else{
    //      CreateRoomBtn.interactable = true;
    //  }
    // }
    public void CreateRoom()
    {
        // if(string.IsNullOrEmpty(roomNameInputField.text))
        // {
        //     return;
        // }
		// CreateRoomBtn2.interactable = false;

        Debug.Log("Created room");
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
        startGameButton.interactable = false;
        playerLeftRoom = false;
        Debug.Log("set button to false");
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
            Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>()
                .SetUp(photonPlayers[i]);
        }

        //  foreach (Photon.Realtime.Player player in PhotonNetwork.CurrentRoom.Players.Values)
        // {
        //  Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(player);
        // }

        startGameButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
        // startGameButton.interactable = (PhotonNetwork.CurrentRoom.PlayerCount > 1);

    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        startGameButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);

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
        
        MenuManager.Instance.OpenMenu("error");
    }


    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log("ON Player LEFT room");

        if (PhotonNetwork.IsMasterClient)
        {
            startGameButton.interactable = false;
        }
        else
        {
            startGameButton.interactable = true;
        }
    }
    public void LeaveRoom()
    {
        DisconnectUser();
        Debug.Log("Leave room");
        playerLeftRoom = true;
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
        Debug.Log("ON LEFT room");
        MenuManager.Instance.OpenMenu("title");
    }

    public void DisconnectUser()
    {
        Debug.Log("Disconnect");

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
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
    public void UpdatePlayerList()
    {
        // Debug.Log("UPDATING current room");
        Photon.Realtime.Player[] photonPlayers = PhotonNetwork.PlayerList;

        foreach (Transform child in playerListContent.transform)
        {
            Destroy(child.gameObject);
        }

        //  foreach (Photon.Realtime.Player player in PhotonNetwork.CurrentRoom.Players.Values)
        // {
        //  Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(player);
        // }

        for (int i = 0; i < photonPlayers.Length; i++)
        {
            // Debug.Log("Instantiating");
            Debug.Log(photonPlayers[i]);
            Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>()
                .SetUp(photonPlayers[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        playerEnteredRoom = true;
        // userNameInput.text = "";
        // userNameInput.text = newPlayer.NickName; 
        // PhotonNetwork.NickName = userNameInput.text + " " + Random.Range(0, 1000).ToString("0000");
        // userNameInput.text = PhotonNetwork.NickName;
        // Debug.Log("Player entered room" + userNameInput.text + "empty");
        Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);

            

        
    }
}

