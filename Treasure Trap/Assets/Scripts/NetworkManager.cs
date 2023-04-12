using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkManager : MonoBehaviour
{
    public GameObject gameManager;
    GameManager game;
    PhotonView photon;

    private void Start() {
        game = gameManager.GetComponent(typeof(GameManager)) as GameManager;
        photon = GetComponent(typeof(PhotonView)) as PhotonView;
    }

    public void ReceiveMoveString(string tileName, string tileColor, string tilePosX, string tilePosY, string tilePosZ, string moveType) {
        Debug.Log("Network received: " + tileName + " " + tileColor + " " + tilePosX + " " + tilePosY + " " + tilePosZ + " " + moveType);
        photon.RPC("RPC_SendMoveString", RpcTarget.All, tileName, tileColor, tilePosX, tilePosY, tilePosZ, moveType);
    }


    [PunRPC]
    void RPC_SendMoveString(string tileName, string tileColor, string tilePosX, string tilePosY, string tilePosZ, string moveType) {
        game.ReceiveMoveString(tileName, tileColor, tilePosX, tilePosY, tilePosZ, moveType);
    }

}
