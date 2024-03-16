using Photon.Pun;
using UnityEngine;

public class PvPPhotonLobbyManager : MonoBehaviourPunCallbacks
{
    public GameObject Footer;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        Connect();
    }

    private void Connect()
    {
        string userName = PlayerPrefs.GetString("Username");
        if (!string.IsNullOrEmpty(userName))
        {
            PhotonNetwork.NickName = userName;
            Debug.LogError($"username {userName}");
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            Debug.LogError($"Empty {userName}");
        }
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Footer.SetActive( true );

    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.LogError($"Created Room {PhotonNetwork.CurrentRoom.Name}");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
    }
}
