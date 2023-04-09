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
	[SerializeField] TMP_Text errorText2;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject PlayerListItemPrefab;
	private List<GameObject> roomListItems = new List<GameObject>();
    public ScrollRect scrollRect;
    public GameObject findRoomMenu;
    public GameObject playerJoined;

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
            UpdatePlayerList();
            startGameButton.interactable = (PhotonNetwork.CurrentRoom.PlayerCount > 1);
        }
        else{
            startGameButton.interactable = false;
        }

        if(findRoomMenu != null && findRoomMenu.activeSelf && roomListItems.Count == 0 ){
            Debug.Log("No one has created room");
                playerJoined.SetActive(true);
        }
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

    public void CreateRoom()
    {
        Debug.Log("Created room");
        Debug.Log("Input room info");
        //only 2 players can connect
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(roomNameInputField.text, roomOptions);
		roomNameInputField.text = "";
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        startGameButton.interactable = false;
        playerLeftRoom = false;
        Debug.Log("set button to false");
        Debug.Log("User name input: " + userNameInput.text);
        PhotonNetwork.NickName = userNameInput.text + " " + Random.Range(0, 10).ToString("00");
        
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

        startGameButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
		userNameInput.text = "";
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        startGameButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);

    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {

        errorText.text = "Room Creation Failed: a game with the specified ID already exists";
        MenuManager.Instance.OpenMenu("error");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // Get the error message
        errorText2.text = "Joining Room Failed: room is already full";;
        MenuManager.Instance.OpenMenu("error2");
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
    public void LeaveRoom2()
    {
        DisconnectUser();
        Debug.Log("Leave room");
        playerLeftRoom = true;
        // PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
        
    }

	public void LeaveRoom()
    {
       	// DisconnectUser();
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
		foreach (RoomInfo room in roomList)
		{
			if (room.RemovedFromList)
			{
				// Remove the room from the room list
				Debug.Log("removing from list");
				GameObject roomItem = roomListItems.Find(item => item.GetComponent<RoomListItem>().info.Name == room.Name);
				if (roomItem != null)
				{
					roomListItems.Remove(roomItem);
					Debug.Log("destroying room");
					Destroy(roomItem);
				}
			}
			else
			{
				// Update or add the room to the room list
				GameObject roomItem = roomListItems.Find(item => item.GetComponent<RoomListItem>().info.Name == room.Name);
				if (roomItem != null)
				{
					Debug.Log("update room");
					// Update the existing room item
                    playerJoined.SetActive(false);
					roomItem.GetComponent<RoomListItem>().SetUp(room);
				}
				else
				{
					// Instantiate a new room item prefab
					roomItem = Instantiate(roomListItemPrefab, roomListContent);
					Debug.Log("adding room");
                    playerJoined.SetActive(false);
					roomListItems.Add(roomItem);
					roomItem.GetComponent<RoomListItem>().SetUp(room);
				}
			}
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
        Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    public void CleanInput(){

        //roomNameInputField
        roomNameInputField.text = "";

        //userNameInput
        userNameInput.text = "";

    }
}

