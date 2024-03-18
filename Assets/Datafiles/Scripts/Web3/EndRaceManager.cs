using System.Collections;
using System.Collections.Generic;
using Aptos.Accounts;
using Aptos.BCS;
using Aptos.HdWallet.Utils;
using Aptos.Unity.Rest;
using Aptos.Unity.Rest.Model;
using UnityEngine;

public class EndRaceManager : MonoBehaviour
{
    public IEnumerator OnEndRace(ulong raceId, List<BString> winningOrder)
    {
        ResponseInfo responseInfo = new();

        byte[] bytes = "dafe19420f798da33a13a5928202ee55f812b1d4666aad6e0f66dedd6daefead".ByteArrayFromHexString();
        Sequence sequence = new(new ISerializable[] { new U64(raceId), new Sequence(winningOrder.ToArray()) });

        var payload = new EntryFunction
        (
            new(new AccountAddress(bytes), "aptos_horses_game"),
            "on_race_end",
            new(new ISerializableTag[] { }),
            sequence
        );

        Transaction buyTx = new();
        Coroutine buy = StartCoroutine(RestClient.Instance.SubmitTransaction((_transaction, _responseInfo) =>
        {
            buyTx = _transaction;
            responseInfo = _responseInfo;
        },
        WalletManager.Instance.Wallet.Account,
        payload));
        yield return buy;

        if (responseInfo.status != ResponseInfo.Status.Success)
        {
            Debug.LogError("Cannot distribute rewards. " + responseInfo.message);
            yield break;
        }

        Debug.Log("Transaction: " + buyTx.Hash);
        Coroutine waitForTransactionCor = StartCoroutine(RestClient.Instance.WaitForTransaction((_pending, _responseInfo) => 
        { 
            responseInfo = _responseInfo; 
        }, buyTx.Hash));
        yield return waitForTransactionCor;

        Debug.Log(responseInfo.status);

        if (responseInfo.status == ResponseInfo.Status.Success)
        { 
            //Success
        }
        else Debug.Log(responseInfo.message);
    }
}