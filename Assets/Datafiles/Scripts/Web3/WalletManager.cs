using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Aptos.HdWallet;
using Aptos.Unity.Rest;
using Aptos.Unity.Sample.UI;
using Photon.Pun;
using TMPro;
using UnityEngine;

[Serializable]
public class JoinedRaceInfo
{
    public string playerAddress;
    public string playerName;
    public int horseId;
    public int horseSpeed;
}

[Serializable]
public class RacePlayer : ISerializationCallbackReceiver
{
    public JoinedRaceInfo joinedRaceInfo;
    public float acceleration;
    public List<float> hurdles;

    public void OnAfterDeserialize()
    {
        throw new NotImplementedException();
    }

    public void OnBeforeSerialize()
    {
        throw new NotImplementedException();
    }
}

public class WalletManager : MonoBehaviourPunCallbacks
{
    public static WalletManager Instance { get; private set; }
    public GameObject _serverInstance;
    //DEV USE ONLY - InstanceID
    public bool devMode = false;
    public bool generatedSpawnPoint = false;
    public TMP_Dropdown instanceId;
    [SerializeField] private GameObject DevPanel;

    [SerializeField] private TextMeshProUGUI AptosTokenBalance;
    [SerializeField] private TextMeshProUGUI AptosAddress;
    [SerializeField] private TextMeshProUGUI AptosUsername;
    [SerializeField] public TextMeshProUGUI _currentRoomName;

    public PvPPhotonLobbyManager serverManager;
    public List<RacePlayer> racePlayer = new();
    public int raceId;
    public int spawnAt;
    public int horseID;
    public int _horseSpeed;
    public float _acceleration;

    public List<float> acceleration = new List<float>();
    public int[] horsesMaxSpeed = new int[5];
    public string[] address = new string[5];

    [HideInInspector] public Wallet Wallet = null;
    [HideInInspector] public float APTBalance;
    public string Username = "";
    public string Address = "";
    public int EquippedHorseId = 1000;
    public JoinedRaceInfo joinedRaceInfos = new();

    public bool _canSwitch = false;
    public bool _createdServerInstance = false;
    public bool _playerInfoAdded = false;

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
        PlayerPrefs.DeleteKey("PlayerId");
        PlayerPrefs.DeleteKey("PlayerId1");
        PlayerPrefs.DeleteKey("PlayerId2");
        PlayerPrefs.DeleteKey("PlayerId3");
        PlayerPrefs.DeleteKey("PlayerId4");

        if (devMode) DevPanel.SetActive(true);
        AptosUILink.Instance.onGetBalance += val =>
        {
            APTBalance = val;
            float bal = AptosUILink.Instance.AptosTokenToFloat(val);
            AptosTokenBalance.text = $"APT Balance : {bal}";
            if (bal < 10) StartCoroutine(AptosUILink.Instance.AirDrop(1000000000));
        };
        RestClient.Instance.SetEndPoint(Constants.RANDOMNET_BASE_URL);
        ThreadPool.QueueUserWorkItem(new WaitCallback(SpawnServerInstance));
    }

    private void Update()
    {
        if (PhotonNetwork.InRoom && _serverInstance == null)
        {
            Debug.LogError("Spawning server instance");
            SpawnServerInstance();
        }

        if (_serverInstance != null)
        {
            Race[] _races = FindObjectsOfType<Race>();
            foreach (var race in _races)
            {
                race._createdServerInstance = _createdServerInstance;
            }
        }
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
        AptosAddress.text = $"{Address}";

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

        if (_serverInstance == null)
        {
            Debug.LogError("Spawning server instance");
            SpawnServerInstance();
        }

        Debug.LogError("Assigning");
    }

    public void SpawnServerInstance(object state = null)
    {
        _serverInstance = PhotonNetwork.Instantiate("ServerInstance", new Vector3(0, 0, 0), Quaternion.identity);
        StartCoroutine(SendEssensData());
        _createdServerInstance = true;
        Race[] _races = FindObjectsOfType<Race>();
        foreach (var race in _races)
        {
            race._createdServerInstance = _createdServerInstance;
        }
    }

    private IEnumerator SendEssensData()
    {
        yield return new WaitUntil(()=>_createdServerInstance && _playerInfoAdded);
        _serverInstance.GetComponent<ServerInstance>().RPCSendEssesData(spawnAt, joinedRaceInfos.playerAddress, joinedRaceInfos.horseSpeed);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        _currentRoomName.text = "Room name:- " + PhotonNetwork.CurrentRoom.Name + " " + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
    }

    internal void SaveHorseId(int val)
    {
        PlayerPrefs.SetInt("HorseId", val);
    }

    public void AddAddress(int index, string val)
    {
        for(int ind = 0; ind < address.Length; ind++)
        {
            if(ind == index)
            {
                address[ind] = val;
            }
        }
    }

    public void AddSpeed(int index, int val)
    {
        for (int ind = 0; ind < horsesMaxSpeed.Length; ind++)
        {
            if (ind == index)
            {
                horsesMaxSpeed[ind] = val;
            }
        }
    }
}
