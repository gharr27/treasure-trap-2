using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("Connect to Server");
        PhotonNetwork.NickName = MasterManager.GameSettings.NickName;
        PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster(){
        // PhotonNetwork.JoinLobby();
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        
    }

    public override void OnDisconnected(DisconnectCause cause){
        Debug.Log("Disconnected from Master because " + cause.ToString());
    }

    public override void OnJoinedLobby(){
        Debug.Log("Joined lobby");
        // SceneManager.LoadScene("Lobby");
    }

}
