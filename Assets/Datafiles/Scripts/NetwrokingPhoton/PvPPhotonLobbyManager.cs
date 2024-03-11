using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PvPPhotonLobbyManager : MonoBehaviourPunCallbacks
{
   public PlayerRoomProfile _profile;
   public List<PlayerRoomProfile> _profileList = new List<PlayerRoomProfile>();

   public RoomItem roomItemPrefab;
   private List<RoomItem> roomItemList = new List<RoomItem>();

   public float timeBetweenUpdates = 1.5f;
   private float nextUpdateTime;

   public GameObject playerProfile;

    private void Start()
    {
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
        // Add a callback for when the client is connected to the Master Server
    }

    public override void OnConnectedToMaster()
    {
        // This callback is triggered when the client is connected to the Master Server
        // Now, you can attempt to join a random room
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    private void PlayGame()
   {
      /*
      SceneManager.instance.LoadScene("PvP");
      */
      PhotonNetwork.JoinRandomOrCreateRoom();
   }

   public override void OnJoinedRoom()
   {
      PhotonNetwork.LoadLevel("World");
      //UpdatePlayerRoomList();
   }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        PlayGame();
    }

    public void JoinRoom(string roomNameText)
   {
      PhotonNetwork.JoinRoom(roomNameText);
   }

   public void LeaveRoom()
   {
      PhotonNetwork.LeaveRoom();
   }
}
