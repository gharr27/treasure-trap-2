using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class PlayerListing : MonoBehaviour
{

    [SerializeField]
    private Text roomText;

    public Player Player { get; private set;}

    public void SetPlayerInfo(Player player){
        Player = player;
        roomText.text = player.NickName;

    }
   
}
