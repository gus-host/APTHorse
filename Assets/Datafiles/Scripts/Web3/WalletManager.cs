using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Aptos.HdWallet;
using Aptos.Unity.Rest;
using Aptos.Unity.Sample.UI;
using GraphQlClient.Core;
using Photon.Pun;
using SimpleJSON;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public GameObject _restartNote;
    
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
    public float []playerOneHurd = new float[3];
    public float []playerTwoHurd = new float[3];
    public float []playerThreeHurd = new float[3];
    public float []playerFourHurd = new float[3];
    public float []playerFiveHurd = new float[3];
    public int[] horsesMaxSpeed = new int[5];
    public string[] address = new string[5];

    [HideInInspector] public Wallet Wallet = null;
    [HideInInspector] public float APTBalance;
    public string Username = "";
    public string Address = "";
    public int EquippedHorseId = 1000;
    public int horseSpeed = 0;
    public JoinedRaceInfo joinedRaceInfos = new();

    public bool _canSwitch = false;
    public bool _createdServerInstance = false;
    public bool _playerInfoAdded = false;
    public bool _inRace = false;
    public bool _blockchainRoomFull = false;
    public bool _completedRace = false;
    public bool _headingBack = false;

    [SerializeField] private GraphApi getResultsGraphql;

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
        PlayerPrefs.DeleteAll();
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

        if(_completedRace && _headingBack)
        {
            int index = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            if(index == 0)
            {
                MainMenuManager.instance.SignInButton.SetActive(false);
                _restartNote.SetActive(true);
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

        RaceObjectManager _raceObject = FindObjectOfType<RaceObjectManager>();
        if (_raceObject != null)
        {
            _serverInstance.GetComponent<ServerInstance>().RPCFetchRaceDataAsync(true);
        }
        else
        {
            Debug.LogError("GetRaceDataAsyncNotFound");
        }

        StartCoroutine(SwitchScene());
    }

    private IEnumerator SwitchScene()
    {
        //Switch scene
        if(PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            yield return new WaitUntil(()=> _blockchainRoomFull);
            _serverInstance.GetComponent<ServerInstance>().RPCInitSceneSwitch();
        }
        yield return null;
    }

    public void SpawnServerInstance(object state = null)
    {
        _serverInstance = PhotonNetwork.Instantiate("ServerInstance", new Vector3(0, 0, 0), Quaternion.identity);
        if(_serverInstance != null)
        {
            _createdServerInstance = true;
        }
        Race[] _races = FindObjectsOfType<Race>();
        foreach (var race in _races)
        {
            race._createdServerInstance = _createdServerInstance;
        }
    }

    public IEnumerator SendEssensData()
    {

        yield return new WaitUntil(()=>_createdServerInstance && _inRace);

        horseSpeed = FindObjectOfType<MarketplaceManager>().GetHorseSpeedById(WalletManager.Instance.EquippedHorseId);
        
        Debug.LogError($"ServerInstance stats {_createdServerInstance}");
        if (_serverInstance != null)
        {
            _serverInstance.GetComponent<ServerInstance>().RPCSendEssesData(spawnAt, joinedRaceInfos.playerAddress, joinedRaceInfos.horseSpeed);
        }
        else
        {
            Debug.LogError("ServerInstance is null");
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        _currentRoomName.text = "Room name:- " + PhotonNetwork.CurrentRoom.Name + " " + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;

        RaceObjectManager _raceObject = FindObjectOfType<RaceObjectManager>();
        if (_raceObject != null)
        {
            _serverInstance.GetComponent<ServerInstance>().RPCFetchRaceDataAsync(true);
        }
        else
        {
            Debug.LogError("GetRaceDataAsyncNotFound");
        }
        Race[] _races = FindObjectsOfType<Race>();
        foreach (var race in _races)
        {
            StartCoroutine(race.LeaveRace());
        }
    }

    

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Race[] _races = FindObjectsOfType<Race>();
        foreach (var race in _races)
        {
           StartCoroutine(race.LeaveRace());
        }
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

    internal void RPCBlockchainRoomFull()
    {
        if(_serverInstance != null)
        {
            _serverInstance.GetComponent<ServerInstance>().RPCMakeBlockchainRoomFull(true);
        }
        else
        {
            Debug.LogError("ServerInstance is null");
        }
    }

    private void OnApplicationQuit()
    {
        /*Race[] _races = FindObjectsOfType<Race>();
        foreach (var race in _races)
        {
            StartCoroutine(race.LeaveRace());
        }*/
    }

    /*async OnDestroy()
    {
        Race[] _races = FindObjectsOfType<Race>();
        foreach (var race in _races)
        {
            StartCoroutine(race.LeaveRace());
        }
    }*/
}
