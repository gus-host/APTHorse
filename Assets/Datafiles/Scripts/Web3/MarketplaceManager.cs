using System.Collections;
using System.Collections.Generic;
using Aptos.Unity.Rest;
using Aptos.Unity.Rest.Model;
using GraphQlClient.Core;
using Newtonsoft.Json;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;

public class MarketplaceManager : MonoBehaviour
{
    public NFT nftGo;
    public Transform nftSpawn;
    public Sprite[] horseSprite;
    private Dictionary<int, int> ownedHorses = new();
    [SerializeField] private GraphApi getAllTokensGraphQL;

    public delegate void OnDataAquired();
    public event OnDataAquired onDataAquired; 

    public IEnumerator GetMarketplaceDataAsync()
    {
        ResponseInfo responseInfo = new();
        string data = "";

        ViewRequest viewRequest = new()
        {
            Function = "0x3e79e6c4f4d55299f09b3aef9a8ba33a2ba0f53d081336c3811c3e4712a8d48b::aptos_horses::get_all_metadata",
            TypeArguments = new string[] { },
            Arguments = new string[] { }
        };

        Coroutine getUser = StartCoroutine(RestClient.Instance.View((_data, _responseInfo) =>
        {
            if (_data != null) data = _data;
            responseInfo = _responseInfo;

        }, viewRequest));

        yield return getUser;

        if (responseInfo.status == ResponseInfo.Status.Failed) Debug.LogError("Error: Fetching data for marketplace failed!");
        else StartCoroutine(GetEquippedHorseId(data));
    }

    private IEnumerator GetEquippedHorseId(string horses)
    {
        ResponseInfo responseInfo = new();
        string data = "";

        ViewRequest viewRequest = new()
        {
            Function = "0x3e79e6c4f4d55299f09b3aef9a8ba33a2ba0f53d081336c3811c3e4712a8d48b::aptos_horses_user::get_equiped_horse",
            TypeArguments = new string[] { },
            Arguments = new string[] { WalletManager.Instance.Wallet.Account.AccountAddress.ToString() }
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
            AssignData(horses, int.Parse(JsonConvert.DeserializeObject<string[]>(data)[0]));
        }
    }

    private async void AssignData(string data, int equippedHorse)
    {
        WalletManager.Instance.EquippedHorseId = equippedHorse;
        UnityWebRequest request = await getAllTokensGraphQL.Post("query MyQuery {current_token_ownerships_v2(where: {owner_address: {_eq: \"" + WalletManager.Instance.Wallet.Account.AccountAddress.ToString() + "\"}}){current_token_data{current_collection{collection_name}token_uri}}}");

        string ownedData = request.downloadHandler.text;
        JSONNode tokens = JSON.Parse(ownedData)["data"]["current_token_ownerships_v2"];

        ownedHorses = new();
        for (int i = 0; i < tokens.Count; i++)
        {
            if (tokens[i]["current_token_data"]["current_collection"]["collection_name"] == "APTHorse")
            {
                string[] split = tokens[i]["current_token_data"]["token_uri"].Value.Split(",");
                ownedHorses.Add(int.Parse(split[0]), int.Parse(split[1]));
            }
        }

        foreach (Transform t in nftSpawn)
        {
            Destroy(t.gameObject);
        }

        JSONNode node = JSON.Parse(data);
        for (int i = 0; i < node[0].Count; i++)
        {
            Instantiate(nftGo, nftSpawn).SetupNFT
            (
                node[0][i]["name"].Value,
                node[0][i]["description"].Value,
                node[0][i]["price"].AsInt,
                horseSprite[i],
                (ulong)node[0][i]["id"].AsInt,
                ownedHorses.ContainsKey(node[0][i]["id"].AsInt),
                node[0][i]["id"].AsInt == equippedHorse
            );
        }

        WalletManager.Instance.joinedRaceInfos = new JoinedRaceInfo
        {
            playerAddress = WalletManager.Instance.Address,
            playerName = WalletManager.Instance.Username,
            horseId = WalletManager.Instance.EquippedHorseId,
            horseSpeed = WalletManager.Instance.EquippedHorseId == 1000 ? 0 : GetHorseSpeedById(WalletManager.Instance.EquippedHorseId)
        };
        StartCoroutine(WalletManager.Instance.SendEssensData());

    }

    public int GetHorseSpeedById(int id)
    {
        return ownedHorses[id];
    }
}
