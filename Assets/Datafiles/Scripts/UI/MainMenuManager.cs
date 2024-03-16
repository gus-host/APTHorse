using System.Collections;
using Aptos.Unity.Rest;
using Aptos.Unity.Rest.Model;
using Aptos.BCS;
using TMPro;
using UnityEngine;
using Aptos.Accounts;
using Aptos.HdWallet.Utils;
using Newtonsoft.Json;

public class MainMenuManager : MonoBehaviour
{
    public GameObject SignInButton;
    public GameObject EnterUsernamePanel;
    public GameObject UpdateUsernamePanel;
    public GameObject InfoPanel;
    public GameObject MarketplaceButton;
    public GameObject RacesButton;

    public void SignIn()
    {
        if (WalletManager.Instance.AuthenticateWithWallet())
        {
            SignInButton.SetActive(false);
            StartCoroutine(GetUser());
        }
    }

    private IEnumerator GetUser()
    {
        ResponseInfo responseInfo = new();
        string[] data = new string[] { };

        ViewRequest viewRequest = new()
        {
            Function = "0xdafe19420f798da33a13a5928202ee55f812b1d4666aad6e0f66dedd6daefead::aptos_horses_user::get_username",
            TypeArguments = new string[] { },
            Arguments = new string[] { WalletManager.Instance.Wallet.Account.AccountAddress.ToString() }
        };

        Coroutine getUser = StartCoroutine(RestClient.Instance.View((_data, _responseInfo) =>
        {
            if(_data != null) data = JsonConvert.DeserializeObject<string[]>(_data);
            responseInfo = _responseInfo;

        }, viewRequest));

        yield return getUser;

        if (responseInfo.status == ResponseInfo.Status.Failed) EnterUsernamePanel.SetActive(true);
        else
        {
            WalletManager.Instance.Username = data[0];
            WalletManager.Instance.UpdateUsername();
            InfoPanel.SetActive(true);
            MarketplaceButton.SetActive(true);
            //RacesButton.SetActive(true);
            StartCoroutine(FindObjectOfType<MarketplaceManager>().GetMarketplaceDataAsync());
            StartCoroutine(FindObjectOfType<RaceObjectManager>().GetRaceDataAsync());
        }
    }

    public void CreateUser(TMP_InputField username)
    {
        StartCoroutine(CreateUserEnumerator(username.text));
    }

    public void UpdateUsername(TMP_InputField username)
    {
        StartCoroutine(UpdateUsernameEnumerator(username.text));
    }

    private IEnumerator CreateUserEnumerator(string username)
    {
        Debug.LogError("Error: Account doesnt exist, creating a new account!");
        ResponseInfo responseInfo = new();

        byte[] bytes = "dafe19420f798da33a13a5928202ee55f812b1d4666aad6e0f66dedd6daefead".ByteArrayFromHexString();
        Sequence sequence = new(new ISerializable[] { new BString(username) });

        EntryFunction payload = new(
            new(new AccountAddress(bytes), "aptos_horses_user"),
            "create_user",
            new(new ISerializableTag[] { }),
            sequence
        );

        Transaction createUserTx = new();
        Coroutine createUser = StartCoroutine(RestClient.Instance.SubmitTransaction((_transaction, _responseInfo) =>
        {
            createUserTx = _transaction;
            responseInfo = _responseInfo;
        },
        WalletManager.Instance.Wallet.Account,
        payload));
        yield return createUser;

        if (responseInfo.status != ResponseInfo.Status.Success)
        {
            Debug.LogError("Cannot create user. " + responseInfo.message);
            yield break;
        }

        Debug.Log("Transaction: " + createUserTx.Hash);
        Coroutine waitForTransactionCor = StartCoroutine(
            RestClient.Instance.WaitForTransaction((_pending, _responseInfo) =>
            {
                responseInfo = _responseInfo;
            }, createUserTx.Hash)
        );
        yield return waitForTransactionCor;

        if (responseInfo.status != ResponseInfo.Status.Success) Debug.LogWarning("Transaction was not found. Breaking out of example: Error: " + responseInfo.message);
        else
        {
            StartCoroutine(GetUser()); 
            EnterUsernamePanel.SetActive(false);
        }
    }

    private IEnumerator UpdateUsernameEnumerator(string username)
    {
        Debug.Log("Log: Updating Username!");
        ResponseInfo responseInfo = new();

        byte[] bytes = "dafe19420f798da33a13a5928202ee55f812b1d4666aad6e0f66dedd6daefead".ByteArrayFromHexString();
        Sequence sequence = new(new ISerializable[] { new BString(username) });

        EntryFunction payload = new(
            new(new AccountAddress(bytes), "aptos_horses_user"),
            "change_username",
            new(new ISerializableTag[] { }),
            sequence
        );

        Transaction createUserTx = new();
        Coroutine createUser = StartCoroutine(RestClient.Instance.SubmitTransaction((_transaction, _responseInfo) =>
        {
            createUserTx = _transaction;
            responseInfo = _responseInfo;
        },
        WalletManager.Instance.Wallet.Account,
        payload));
        yield return createUser;

        if (responseInfo.status != ResponseInfo.Status.Success)
        {
            Debug.LogError("Cannot update user's username. " + responseInfo.message);
            yield break;
        }

        Debug.Log("Transaction: " + createUserTx.Hash);
        Coroutine waitForTransactionCor = StartCoroutine(
            RestClient.Instance.WaitForTransaction((_pending, _responseInfo) =>
            {
                responseInfo = _responseInfo;
            }, createUserTx.Hash)
        );
        yield return waitForTransactionCor;

        if (responseInfo.status != ResponseInfo.Status.Success) Debug.LogWarning("Transaction was not found. Breaking out of example: Error: " + responseInfo.message);
        else
        {
            StartCoroutine(GetUser());
            UpdateUsernamePanel.SetActive(false);
        }
    }
}