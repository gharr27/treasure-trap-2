using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class RoomListingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform content;
    [SerializeField]
    private RoomListing roomListing;
    private List<RoomListing> listings = new List<RoomListing>();
    private Rooms rooms;

    public void FirstInitialize(Rooms canvas){
        rooms = canvas;
    }

    public void onJoinRoom(){
        rooms.CurrentRoom.Show();

    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList){
        foreach(RoomInfo info in roomList){

            if(info.RemovedFromList){
                int index = listings.FindIndex(x => x.RoomInfo.Name == info.Name);
                if(index != -1){
                    Destroy(listings[index].gameObject);
                    listings.RemoveAt(index);
                }
            }
            else{
                RoomListing listing = Instantiate(roomListing, content);
                if(listing != null){
                    listing.SetRoomInfo(info);
                    listings.Add(listing);
                }
            }
        }
    }
}
