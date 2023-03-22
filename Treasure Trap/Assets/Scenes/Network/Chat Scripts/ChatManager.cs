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

    void Start()
    {
          Debug.Log("Connecting chat now");
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
            if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
            {
                SendMsg();
            }
        });
    }

    void Update()
    {
        if (chatClient != null)
        {
            chatClient.Service();
        }
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < senders.Length; i++)
        {
            Debug.Log(senders[i]);
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

     public void ConnectToServer()
    {
      
    }

    public void DisconnectFromServer()
    {
        Debug.Log("Leaving");
        chatClient.Disconnect(ChatDisconnectCause.None);
    }

    public void SendMsg()
    {
        chatClient.PublishMessage(PhotonNetwork.CurrentRoom.Name, PhotonNetwork.NickName + ": " + msgInput.text);
        msgInput.text = "";
    }

    public void Join()
    {
    
    }

    public void Leave()
    {
        chatClient.Unsubscribe(new string[] {PhotonNetwork.CurrentRoom.Name});
        chatClient.SetOnlineStatus(ChatUserStatus.Offline);
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log("DebugReturn: " + level + " " + message);

    }
    

    public void OnDisconnected()
    {
        
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
        
    }
}
