using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PvPConnectToServer : MonoBehaviourPunCallbacks
{
    public TMP_InputField _playerName;
    public Button _continue;
    public TMP_Text _continuetext;
    public GameObject _profilePanel;
    public GameObject _lobbyManager;
    
    private void Start()
    {
        //_continue.onClick.AddListener(Connect);
        PhotonNetwork.Disconnect();
        Connect();
    }

    private void Connect()
    {
        string userName = PlayerPrefs.GetString("Username");
        if (!string.IsNullOrEmpty(userName))
        {
            PhotonNetwork.NickName = userName;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            Debug.LogError($"Empty {userName}");
        }
    }

    public override void OnConnectedToMaster()
    {
        _lobbyManager.SetActive(true);
    }
}
