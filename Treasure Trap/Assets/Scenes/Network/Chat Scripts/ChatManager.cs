using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Chat;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour, IChatClientListener
{
    private ChatClient chatClient;
    public TMP_InputField msgInput;
    public TMP_Text msgArea;
    public TMP_Text userName1;
    public TMP_Text userName2;
    MenusManager menuManager;
    GameObject menuManagerObject;
    public TMP_Text counterText;
    public int counter = 0;
    public GameObject notificationImage;


    void Start()
    {
        Debug.Log("Connecting chat now");
        menuManagerObject = GameObject.FindWithTag("Menu");
        menuManager = menuManagerObject.GetComponent(typeof(MenusManager)) as MenusManager;

        chatClient = new ChatClient(this);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion,
            new Photon.Chat.AuthenticationValues(PhotonNetwork.NickName));
        Application.runInBackground = true;
        
        if(string.IsNullOrEmpty(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat))
        {
            Debug.LogError("No AppID Provided");
            return;
        }
        msgInput.onEndEdit.AddListener((string text) =>
        {
            Debug.Log("Input text: " + msgInput.text);
            if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
            {
                if(!string.IsNullOrWhiteSpace(msgInput.text)){
                SendMsg();
                }
            }
        });

        Photon.Realtime.Player[] photonPlayers = PhotonNetwork.PlayerList;

        if (photonPlayers.Length >= 2)
        {
            userName1.text = photonPlayers[0].NickName;
            userName2.text = photonPlayers[1].NickName;
        }

        // userName1.text = PhotonNetwork.NickName;

    }

    void Update()
    {
        if (chatClient != null)
        {
            chatClient.Service();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Leave();

        }
        if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            //load "come back" scene
            Debug.Log("Player left is leaving");
            // chatClient.PublishMessage(PhotonNetwork.CurrentRoom.Name, $"{PhotonNetwork.NickName} has left the room.");
            Leave();
            menuManager.GoToWinnerScreen();

        }
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
            counter++;
            counterText.text = counter.ToString();
            for (int i = 0; i < senders.Length; i++)
            {
                // if (senders[i] != PhotonNetwork.NickName) // check if sender is not local player
                // {
                //     // display notification on UI text element
                //     notificationText.text = senders[i] + " sent a message: " + messages[i];
                //     // hide notification after 3 seconds
                //     StartCoroutine(HideNotification(5));
                // }

                if (senders[i] != PhotonNetwork.NickName) // check if sender is not local player
                {
                    // display notification image
                    notificationImage.SetActive(true);
                }

                Debug.Log(senders[i] + ": what I get");
                if (string.IsNullOrEmpty(msgArea.text))
                {
                    msgArea.text += messages[i] + " ";
                }
                else
                {
                    msgArea.text += "\r\n" + messages[i] + " ";
                }
            }

    }

    // private IEnumerator HideNotification(float delay)
    // {
    //     yield return new WaitForSeconds(delay);
    //     notificationText.text = "";
    // }

    public void ConnectToServer()
    {

    }

    public void DisconnectFromServer()
    {
        Debug.Log("Leaving...");
        // chatClient.Disconnect(ChatDisconnectCause.None);

        if (chatClient.State == ChatState.ConnectedToFrontEnd || chatClient.State == ChatState.ConnectedToNameServer)
        {
            chatClient.Disconnect(ChatDisconnectCause.None);
        }
    }

    public void SendMsg()
    {
        Debug.Log("Message input before sending: " + msgInput.text);
        // if(string.IsNullOrWhiteSpace(msgInput.text)){
            // Debug.Log("sending: " + msgInput.text);
        chatClient.PublishMessage(PhotonNetwork.CurrentRoom.Name, PhotonNetwork.NickName + ": " + msgInput.text);
         
        msgInput.text = "";
    }

    public void Join()
    {
    
    }

    // public void Leave()
    // {
    //      Debug.Log("Leave room");
    //     chatClient.Unsubscribe(new string[] {PhotonNetwork.CurrentRoom.Name});
    //     chatClient.SetOnlineStatus(ChatUserStatus.Offline);
    // }

    public void Leave()
    {
        Debug.Log("Leave room");
        chatClient.Unsubscribe(new string[] { PhotonNetwork.CurrentRoom.Name });
        chatClient.SetOnlineStatus(ChatUserStatus.Offline);
        DisconnectFromServer(); // disconnect from Photon Chat
        PhotonNetwork.LeaveRoom(); // disconnect from Photon PUN
        PhotonNetwork.Disconnect();
    }


    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log("DebugReturn: " + level + " " + message);

    }
    

    public void OnDisconnected()
    {
        Debug.Log("ON Disconnect room");
        Debug.Log("going to MAIN");
        menuManager.GoToMainMenu();


    }

    public void OnConnected()
    {
        Debug.Log("Moving to chatpanel");

        // MenuManager.Instance.OpenMenu("chatPanel");
        chatClient.Subscribe(new string[] {PhotonNetwork.CurrentRoom.Name});
        chatClient.SetOnlineStatus(ChatUserStatus.Online);
    }

    public void OnChatStateChange(ChatState state)
    {
        
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        // foreach(var channel in channels){
        //     this.chatClient.PublishMessage(channel, "joined");
        // }
    
        
    }

    public void OnUnsubscribed(string[] channels)
    {
        msgArea.text = "";
        //photonManager.Leave();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        
    }

    public void OnUserSubscribed(string channel, string user)
    {
        
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        Debug.Log("ON unsubscribed room");

    }

    public void onExitChat(){
        counter = 0;
        counterText.text = counter.ToString();
        notificationImage.SetActive(false);
    }
}
