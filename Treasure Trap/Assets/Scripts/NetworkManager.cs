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

    public void ReceiveMoveString(string tileName, string tilePos) {
        photon.RPC("RPC_SendMoveString", RpcTarget.All, tileName, tilePos);
    }


    [PunRPC]
    void RPC_SendMoveString(string tileName, string tilePos) {
        game.ReceiveMoveString(tileName, tilePos);
    }

}
