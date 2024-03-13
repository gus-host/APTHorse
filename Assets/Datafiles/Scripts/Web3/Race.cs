using System.Collections;
using Aptos.Accounts;
using Aptos.BCS;
using Aptos.HdWallet.Utils;
using Aptos.Unity.Rest;
using Aptos.Unity.Rest.Model;
using Aptos.Unity.Sample.UI;
using SimpleJSON;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Race : MonoBehaviour
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

    void Start()
    {
        joinRaceButton.onClick.AddListener(() => StartCoroutine(JoinRace()));
        leaveRaceButton.onClick.AddListener(() => StartCoroutine(LeaveRace()));
    }

    public void SetupRace(ulong raceId, string raceName, int racePrice, int raceLaps, bool raceStarted, JSONNode players)
    {
        this.raceId = raceId;
        this.racePrice = racePrice;
        this.raceLaps = raceLaps;

        raceNameText.text = raceName;
        racePriceText.text = $"{AptosUILink.Instance.AptosTokenToFloat(racePrice)} APT";
        raceStartedText.text = $"{(raceStarted ? "Ongoing" : "Not Started")}";
        playersJoinedText.text = $"Players: {players.Count}/5";

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].Value == WalletManager.Instance.Wallet.Account.AccountAddress.ToString()) inRace = true;
        }

        CheckInRace();
    }

    private void CheckInRace()
    {
        joinRaceButton.gameObject.SetActive(false);
        leaveRaceButton.gameObject.SetActive(false);

        if (inRace) leaveRaceButton.gameObject.SetActive(true);
        else joinRaceButton.gameObject.SetActive(true);
    }

    public IEnumerator JoinRace()
    {
        if (WalletManager.Instance.APTBalance < racePrice)
        {
            Debug.LogError("APT Balance too low!");
            yield break;
        }

        ResponseInfo responseInfo = new();

        byte[] bytes = "a94a9da70feb4596757bce720b8b612c9ef54783f84316f7cb5523b5eb4e47d7".ByteArrayFromHexString();
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
            yield return StartCoroutine(CanStartRace());
            yield return StartCoroutine(FindObjectOfType<RaceObjectManager>().GetRaceDataAsync());
        }
        else Debug.Log(responseInfo.message);
        yield return new WaitForSeconds(1);
        AptosUILink.Instance.LoadCurrentWalletBalance();
    }

    private IEnumerator CanStartRace()
    {
        ResponseInfo responseInfo = new();
        string data = "";

        ViewRequest viewRequest = new()
        {
            Function = "0xa94a9da70feb4596757bce720b8b612c9ef54783f84316f7cb5523b5eb4e47d7::aptos_horses_game::can_start_race",
            TypeArguments = new string[] { },
            Arguments = new string[] { WalletManager.Instance.Wallet.Account.AccountAddress.ToString(), new U64(raceId).ToString() }
        };

        Coroutine getUser = StartCoroutine(RestClient.Instance.View((_data, _responseInfo) =>
        {
            if (_data != null) data = _data;
            responseInfo = _responseInfo;

        }, viewRequest));

        yield return getUser;

        if (responseInfo.status == ResponseInfo.Status.Failed) Debug.LogError("Error: Fetching data failed!");
        else
        {
            Debug.Log(data);
        }
    }

    public IEnumerator LeaveRace()
    {
        if (WalletManager.Instance.APTBalance < racePrice)
        {
            Debug.LogError("APT Balance too low!");
            yield break;
        }

        ResponseInfo responseInfo = new();

        byte[] bytes = "a94a9da70feb4596757bce720b8b612c9ef54783f84316f7cb5523b5eb4e47d7".ByteArrayFromHexString();
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
        if (responseInfo.status == ResponseInfo.Status.Success) StartCoroutine(FindObjectOfType<RaceObjectManager>().GetRaceDataAsync());
        else Debug.Log(responseInfo.message);
        yield return new WaitForSeconds(1);
        AptosUILink.Instance.LoadCurrentWalletBalance();
    }
}
