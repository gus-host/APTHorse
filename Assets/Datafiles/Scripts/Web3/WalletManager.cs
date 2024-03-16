using System;
using System.Collections.Generic;
using Aptos.HdWallet;
using Aptos.Unity.Rest;
using Aptos.Unity.Sample.UI;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public struct JoinedRaceInfo
{
    public string playerAddress;
    public string playerName;
    public int horseId;
    public int horseSpeed;
}

public struct RacePlayer
{
    public JoinedRaceInfo joinedRaceInfo;
    public float acceleration;
    public List<float> hurdles;
}

public class WalletManager : MonoBehaviourPunCallbacks
{
    public static WalletManager Instance { get; private set; }

    //DEV USE ONLY - InstanceID
    public bool devMode = false;
    public TMP_Dropdown instanceId;
    [SerializeField] private GameObject DevPanel;

    [SerializeField] private TextMeshProUGUI AptosTokenBalance;
    [SerializeField] private TextMeshProUGUI AptosAddress;
    [SerializeField] private TextMeshProUGUI AptosUsername;
    [SerializeField] public TextMeshProUGUI _currentRoomName;

    public PvPPhotonLobbyManager serverManager;
    public List<RacePlayer> racePlayer = new();
    public int raceId;

    [HideInInspector] public Wallet Wallet = null;
    [HideInInspector] public float APTBalance;
    [HideInInspector] public string Username = "";
    [HideInInspector] public string Address = "";
    [HideInInspector] public int EquippedHorseId = 1000;
    [HideInInspector] public Dictionary<int, JoinedRaceInfo> joinedRaceInfos = new();

    public bool _canSwitch = false;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        //PlayerPrefs.DeleteAll();
        if (devMode) DevPanel.SetActive(true);
        AptosUILink.Instance.onGetBalance += val =>
        {
            APTBalance = val;
            float bal = AptosUILink.Instance.AptosTokenToFloat(val);
            AptosTokenBalance.text = $"APT Balance : {bal}";
            if (bal < 10) StartCoroutine(AptosUILink.Instance.AirDrop(1000000000));
        };
        RestClient.Instance.SetEndPoint(Constants.RANDOMNET_BASE_URL);
    }

    public bool AuthenticateWithWallet()
    {
        if (Wallet != null) return true;

        string mneomicsKey = PlayerPrefs.GetString(AptosUILink.Instance.mnemonicsKey + (devMode ? instanceId.value : ""), "");
        if (string.IsNullOrEmpty(mneomicsKey))
        {
            if (!AptosUILink.Instance.CreateNewWallet()) return false;
            mneomicsKey = PlayerPrefs.GetString(AptosUILink.Instance.mnemonicsKey + (devMode ? instanceId.value : ""), "");
        }
        else if (!AptosUILink.Instance.RestoreWallet(mneomicsKey)) return false;

        Wallet = new Wallet(mneomicsKey);
        Address = AptosUILink.Instance.GetCurrentWalletAddress();
        AptosAddress.text = $"Address : {Address}";

        return true;
    }

    public void UpdateUsername()
    {
        AptosUsername.text = Username;
        PlayerPrefs.SetString("Username", Username);
        if (!string.IsNullOrEmpty(Username))
        {
            serverManager.gameObject.SetActive(true);
        }
    }
    public override void OnCreatedRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            _currentRoomName.text = "Room name:- " + PhotonNetwork.CurrentRoom.Name + " " + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        _currentRoomName.text = "Room name:- " + PhotonNetwork.CurrentRoom.Name +" "+ PhotonNetwork.CurrentRoom.PlayerCount+"/" + PhotonNetwork.CurrentRoom.MaxPlayers;
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        _currentRoomName.text = "NOT IN ROOM";
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        _currentRoomName.text = "Room name:- " + PhotonNetwork.CurrentRoom.Name + " " + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
        Debug.LogError("Assigning");
        if(PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers && _canSwitch)
        {
            InitSceneSwitchRPC();
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        _currentRoomName.text = "Room name:- " + PhotonNetwork.CurrentRoom.Name + " " + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
    }

    internal void InitSceneSwitchRPC()
    {
        Debug.LogError("InitSceneSwitch");
        photonView.RPC("SwitchToRace", RpcTarget.AllBufferedViaServer);
    }

    public void RPCToggleSwitch()
    {
        Debug.LogError("RPCToggleSwitch");
        photonView.RPC("ToggleSwitch", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    public void ToggleSwitch()
    {
        _canSwitch = true;
    }


    [PunRPC]
    private void SwitchToRace()
    {
        //We will store the variables
        Debug.LogError("Loading scene");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("HorseJockey");
        }
    }
}
