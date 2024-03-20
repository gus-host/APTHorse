using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Aptos.Accounts;
using Aptos.BCS;
using Aptos.HdWallet;
using Aptos.HdWallet.Utils;
using Aptos.Unity.Rest;
using Aptos.Unity.Rest.Model;
using Aptos.Unity.Sample.UI;
using Photon.Pun;
using Photon.Realtime;
using SimpleJSON;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Race : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI raceNameText;
    public TextMeshProUGUI racePriceText;
    public TextMeshProUGUI raceStartedText;
    public TextMeshProUGUI playersJoinedText;
    public Button joinRaceButton;
    public Button leaveRaceButton;
    private ulong raceId;
    private int racePrice;
    private int raceLaps;
    private bool inRace = false;
    private bool playerJoined = false;
    public bool _addedLastInfo = false;
    public bool _createdServerInstance = false;

    private int playersInRace;
    private SpinnerManager spinnerManager;

    void Start()
    {
        joinRaceButton.onClick.AddListener(() => StartCoroutine(JoinRace()));
        leaveRaceButton.onClick.AddListener(() => StartCoroutine(LeaveRace()));
        if (inRace && !PhotonNetwork.InRoom && PhotonNetwork.IsConnected)
        {
            JoinArena();
        }
    }

    public void SetupRace(ulong raceId, string raceName, int racePrice, int raceLaps, bool raceStarted, JSONNode players)
    {
        spinnerManager = FindObjectOfType<SpinnerManager>();
        this.raceId = raceId;
        this.racePrice = racePrice;
        this.raceLaps = raceLaps;

        raceNameText.text = raceName;
        racePriceText.text = $"{AptosUILink.Instance.AptosTokenToFloat(racePrice)} APT";
        raceStartedText.text = $"{(raceStarted ? "Ongoing" : "Not Started")}";
        playersJoinedText.text = $"Players: {players.Count}/5";
        playersInRace = players.Count;

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].Value == WalletManager.Instance.Wallet.Account.AccountAddress.ToString())
            {
                WalletManager.Instance.spawnAt = i;
                Debug.LogError($"i spawn at {i}");
                inRace = true;
                WalletManager.Instance._inRace= inRace;
            } 
        }

        CheckInRace();
    }

    private void CheckInRace()
    {
        leaveRaceButton.gameObject.SetActive(inRace);
        joinRaceButton.gameObject.SetActive(!inRace);

        spinnerManager.HideMessage();
        playerJoined = true;
    }

    public IEnumerator JoinRace()
    {
        if (WalletManager.Instance.APTBalance < racePrice)
        {
            Debug.LogError("APT Balance too low!");
            yield break;
        }

        if (WalletManager.Instance.EquippedHorseId == 1000)
        {
            Debug.LogError("No horse equipped! Please equip a horse.");
            yield break;
        }

        spinnerManager.ShowMessage("Joining Race...");
        ResponseInfo responseInfo = new();

        byte[] bytes = "dafe19420f798da33a13a5928202ee55f812b1d4666aad6e0f66dedd6daefead".ByteArrayFromHexString();
        Sequence sequence = new(new ISerializable[] { new U64(raceId) });

        var payload = new EntryFunction
        (
            new(new AccountAddress(bytes), "aptos_horses_game"),
            "join_race",
            new(new ISerializableTag[] { }),
            sequence
        );

        Transaction joinTx = new();
        Coroutine join = StartCoroutine(RestClient.Instance.SubmitTransaction((_transaction, _responseInfo) =>
        {
            joinTx = _transaction;
            responseInfo = _responseInfo;
        },
        WalletManager.Instance.Wallet.Account,
        payload));
        yield return join;

        if (responseInfo.status != ResponseInfo.Status.Success)
        {
            Debug.LogError("Cannot join. " + responseInfo.message);
            yield break;
        }

        Debug.Log("Transaction: " + joinTx.Hash);
        Coroutine waitForTransactionCor = StartCoroutine(
            RestClient.Instance.WaitForTransaction((_pending, _responseInfo) =>
            {
                responseInfo = _responseInfo;
            }, joinTx.Hash)
        );
        yield return waitForTransactionCor;

        Debug.Log(responseInfo.status);

        if (responseInfo.status == ResponseInfo.Status.Success)
        {
            JoinArena();
            yield return StartCoroutine(CanStartRace());
            yield return StartCoroutine(FindObjectOfType<RaceObjectManager>().GetRaceDataAsync());
            WalletManager.Instance._playerInfoAdded = true;                  
        }
        else
        {
            Debug.Log(responseInfo.message);
            spinnerManager.HideMessage();
        } 
        yield return new WaitForSeconds(1);
        AptosUILink.Instance.LoadCurrentWalletBalance();
    }

    private IEnumerator CanStartRace()
    {
        ResponseInfo responseInfo = new();
        string data = "";

        ViewRequest viewRequest = new()
        {
            Function = "0xdafe19420f798da33a13a5928202ee55f812b1d4666aad6e0f66dedd6daefead::aptos_horses_game::can_start_race",
            TypeArguments = new string[] { },
            Arguments = new string[] { WalletManager.Instance.Wallet.Account.AccountAddress.ToString(), new U64(raceId).ToString() }
        };

        Coroutine getUser = StartCoroutine(RestClient.Instance.View((_data, _responseInfo) =>
        {
            if (_data != null) data = _data;
            responseInfo = _responseInfo;

        }, viewRequest));

        yield return getUser;

        if (responseInfo.status != ResponseInfo.Status.Success) Debug.LogError("Error: Fetching data failed!");
        else
        {
            JSONNode node = JSONNode.Parse(data);
            Debug.LogError($"JSON {data}");
            bool canStart = node[0].AsBool;
            if (canStart)
            {
                int raceId = node[1];
                JSONNode randomAcceleration = node[2];
                JSONNode randomHurdles = node[3];

                List<RacePlayer> players = new();
                for (int i = 0; i < randomAcceleration.Count; i++)
                {
                    List<float> playerHurdles = new();
                    for (int j = 0; j < randomHurdles[i].Count; j++)
                    {
                        playerHurdles.Add(int.Parse(randomHurdles[i][j].Value) / 100);
                    }

                    RacePlayer player = new()
                    {
                        acceleration = int.Parse(randomAcceleration[i].Value) / 100,
                        hurdles = playerHurdles
                    };
                    players.Add(player);
                }
                Debug.LogError("Waiting for 5 sec");
                yield return new WaitUntil(() => PhotonNetwork.InRoom);
                if (PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
                {
                    
                    Debug.LogError("Multiplayer logic 1");

                    //Send acceleration and hurdles data over network
                    WalletManager.Instance._serverInstance.GetComponent<ServerInstance>().RPCRaceData(data);
                    Debug.LogError("Multiplayer logic 2");

                    //Wait until player check ins
                    yield return new WaitUntil(() => playerJoined);

                    //store local acceleration
                    WalletManager.Instance._acceleration = players[WalletManager.Instance.spawnAt].acceleration;

                    //WalletManager.Instance.RPCBlockchainRoomFull();
                    StartCoroutine(WalletManager.Instance._serverInstance.GetComponent<ServerInstance>().RPCInitSceneSwitch());
                }
                else
                {
                    Debug.LogError("Not In room or Room not full");
                    WalletManager.Instance._blockchainRoomFull = true;
                }
            }
        }
    }

    private void JoinArena()
    {
        Debug.LogError($"JoinArena");
        Debug.LogError($"Race Id {raceId} && {PhotonNetwork.Server}");
        WalletManager.Instance.raceId = (int)this.raceId;
        RoomOptions roomOptions = new RoomOptions() { MaxPlayers = 5, IsOpen = true, IsVisible = true };
        PhotonNetwork.JoinOrCreateRoom(raceId.ToString(), roomOptions, null);
    }

    public IEnumerator LeaveRace()
    {
        if (WalletManager.Instance.APTBalance < racePrice)
        {
            Debug.LogError("APT Balance too low!");
            yield break;
        }

        spinnerManager.ShowMessage("Leaving Race...");
        ResponseInfo responseInfo = new();

        byte[] bytes = "dafe19420f798da33a13a5928202ee55f812b1d4666aad6e0f66dedd6daefead".ByteArrayFromHexString();
        Sequence sequence = new(new ISerializable[] { new U64(raceId) });

        var payload = new EntryFunction
        (
            new(new AccountAddress(bytes), "aptos_horses_game"),
            "leave_race",
            new(new ISerializableTag[] { }),
            sequence
        );

        Transaction leaveTx = new();
        Coroutine leave = StartCoroutine(RestClient.Instance.SubmitTransaction((_transaction, _responseInfo) =>
        {
            leaveTx = _transaction;
            responseInfo = _responseInfo;
        },
        WalletManager.Instance.Wallet.Account,
        payload));
        yield return leave;

        if (responseInfo.status != ResponseInfo.Status.Success)
        {
            Debug.LogError("Cannot leave. " + responseInfo.message);
            yield break;
        }

        Debug.Log("Transaction: " + leaveTx.Hash);
        Coroutine waitForTransactionCor = StartCoroutine(
            RestClient.Instance.WaitForTransaction((_pending, _responseInfo) =>
            {
                responseInfo = _responseInfo;
            }, leaveTx.Hash)
        );
        yield return waitForTransactionCor;

        Debug.Log(responseInfo.status);
        if (responseInfo.status == ResponseInfo.Status.Success)
        {
            WalletManager.Instance.joinedRaceInfos = new();
            StartCoroutine(FindObjectOfType<RaceObjectManager>().GetRaceDataAsync());
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
            }
        }
        else
        {
            Debug.Log(responseInfo.message);
            spinnerManager.HideMessage();
        } 
        yield return new WaitForSeconds(1);
        AptosUILink.Instance.LoadCurrentWalletBalance();
    }
}