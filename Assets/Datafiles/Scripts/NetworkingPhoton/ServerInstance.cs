using Photon.Pun;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerInstance : MonoBehaviourPunCallbacks
{
    
    public static ServerInstance Instance;
    public int spawnPoint;
    private SpinnerManager spinnner;
    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        spinnner = FindObjectOfType<SpinnerManager>();
    }
    public IEnumerator RPCInitSceneSwitch()
    {
        if(!PhotonNetwork.InRoom)
        {
            Debug.LogError("Not in room");
            yield return null; 
        }
        spinnner.ShowMessage("Loading race");
        Debug.LogError("InitSceneSwitch");
       
        photonView.RPC("SwitchToRace", RpcTarget.All);
        yield return null; 
    }

    [PunRPC]
    private void SwitchToRace()
    {
        //We will store the variables
        if(PhotonNetwork.IsMasterClient)
        {
            Debug.LogError($"Is master client loaded scene {PhotonNetwork.IsMasterClient}");
            PhotonNetwork.LoadLevel("HorseJockey");
        }
    }

    public void RPCRaceData(string data)
    {
        if (!PhotonNetwork.InRoom)
        {
            Debug.LogError("Not in room");
            return ;
        }
        Debug.LogError("RPCToggleSwitch");
        photonView.RPC("ToggleSwitch", RpcTarget.AllBufferedViaServer, data);
    }

    [PunRPC]
    public void ToggleSwitch(string data)
    {
        JSONNode node = JSONNode.Parse(data);
        Debug.LogError($"JSON {data}");
        int raceId = node[1];
        JSONNode randomAcceleration = node[2];
        JSONNode randomHurdles = node[3];

        List<RacePlayer> players = new();
        for (int i = 0; i < randomAcceleration.Count; i++)
        {
            List<float> playerHurdles = new();
            for (int j = 0; j < randomHurdles[i].Count; j++)
            {
                playerHurdles.Add((float)int.Parse(randomHurdles[i][j].Value) / 100);
            }
            if (i == 0)
            {
                WalletManager.Instance.playerOneHurd = playerHurdles.ToArray();
            }else if (i==1)
            {
                WalletManager.Instance.playerTwoHurd = playerHurdles.ToArray();
            }else if (i == 2)
            {
                WalletManager.Instance.playerThreeHurd = playerHurdles.ToArray();
            }
            else if (i == 3)
            {
                WalletManager.Instance.playerFourHurd = playerHurdles.ToArray();
            }
            else if (i == 4)
            {
                WalletManager.Instance.playerFiveHurd = playerHurdles.ToArray();
            }
            RacePlayer player = new()
            {
                acceleration = (float)int.Parse(randomAcceleration[i].Value) / 100,
                hurdles = playerHurdles
            };
            players.Add(player);
        }
        for (int i = 0; i< players.Count; i++)
        {

            Debug.LogError($"acceleration {players[i].acceleration}");
            WalletManager.Instance.acceleration.Add(players[i].acceleration);
        }
    }

    internal void RPCSendEssesData(int spawnAt, string playerAddress, int horseSpeed)
    {
        Debug.LogError($" RPCSendEssesData spawnAt {spawnAt} address {playerAddress} horseSpeed {horseSpeed}");
        photonView.RPC("SendEssesData", RpcTarget.AllBufferedViaServer, spawnAt, playerAddress, horseSpeed);
    }

    [PunRPC]
    public void SendEssesData(int spawnAt, string playerAddress, int horseSpeed)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            Debug.LogError($" SendEssesData SpawnAt {spawnAt} Address {playerAddress} HorseSpeed {horseSpeed}");
            WalletManager.Instance.AddAddress(spawnAt, playerAddress);
            WalletManager.Instance.AddSpeed(spawnAt, horseSpeed);
            if(spawnAt == 4)
            {
                Race[] _race = FindObjectsOfType<Race>();
                foreach(Race race in _race)
                {
                    race._addedLastInfo = true;
                }
            }
        }
    }


    public void RPCFetchRaceDataAsync(bool val)
    {
        photonView.RPC("FetchRaceDataAsync", RpcTarget.All, val);
    }

    [PunRPC]
    private void FetchRaceDataAsync(bool val)
    {
        if (val)
        {
            StartCoroutine(FindObjectOfType<RaceObjectManager>().GetRaceDataAsync());
        }
    }
}
