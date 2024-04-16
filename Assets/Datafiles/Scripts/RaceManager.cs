//Race Ids denotes Room feature like 0 for 1 Lap, 1 for 3 Lap, 2 for 5 lap
using Aptos.Accounts;
using Aptos.BCS;
using GraphQlClient.Core;
using Photon.Pun;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[Serializable]
public class CoinActivity
{
    public float amount;
    public int transaction_version;
    public int event_index;
}

[Serializable]
public class CoinActivitiesResponseData
{
    public CoinActivity[] coin_activities;
}

[Serializable]
public class CoinActivitiesResponse
{
    public CoinActivitiesResponseData data;
}

public class RaceManager : MonoBehaviourPunCallbacks
{
    public static RaceManager instance;

    public float DummyToken = 10f;

    public TMP_Text _availableToken;
    public TMP_InputField tMP_InputField_betAmount;
    public TMP_Dropdown tMP_Dropdown_Horsecolor;
    public TMP_InputField tMP_InputField_totallap;
    public Button _Bet;
    public Button _start;
    public Button _leaveRace = null;

    public CanvasGroup _toast;
    public TMP_Text _toastText;
    public GameObject BetPanel;
    public GameObject ResultPanel;
    public Transform _content;
    public PlayerItem playerItem;

   /* public List<float> acceleration = new List<float>();
    public List<int> speed = new List<int>();
    public List<string>  address= new List<string>();*/

    public int jockeyId;
    public int totalLap;

    public bool addedAllJockeys = false;
    public bool _completedRace = false;
    
    public HorseController[] jockeys = new HorseController[5];
    public int jockeysIndex = 0;
    public List<HorseController> _horseRanks = new List<HorseController>();
    public List<PlayerItem> _playerItemInstances = new List<PlayerItem>();
    static Action UpdateAtt;

    //On which playe has bet

    public Stack<HorseController> horses = new Stack<HorseController>();

    [SerializeField] public GraphApi getResultsGraphql;


    private void Awake()
    {
        instance = this;

       jockeys = new HorseController[5]; 
    }

    private void Start()
    {
        #region Go through each jockey and assign the properties
        /* foreach (var jockey in jockeys)
         {
             if (jockey.playerProperties.color == HorseColor.Green)
             {

             }else if (jockey.playerProperties.color == HorseColor.White)
             {

             }
             else if (jockey.playerProperties.color == HorseColor.Blue)
             {

             }
             else if (jockey.playerProperties.color == HorseColor.Brown)
             {

             }
         }*/
        #endregion

        UpdateAtt += UpdateAttributes;
        UpdateAtt?.Invoke();
        _Bet.onClick.AddListener(() =>
        {
            Bet();
        }
        );
        StartCoroutine(AssignValues());

        _leaveRace.gameObject.SetActive( false );
    }

    public void FadeToastIn()
    {
        LeanTween.alphaCanvas(_toast, 1, 1f);
    }

    public void FadeToastOut()
    {
        LeanTween.alphaCanvas(_toast, 0, 3f);
    }
    public void AddHorse(HorseController _horse)
    {
        jockeys[jockeysIndex++] = _horse;
    }

    private IEnumerator AssignValues()
    {
        Debug.LogError("Assigning values to each horse");
        yield return new WaitUntil(() => addedAllJockeys);
        if (PhotonNetwork.IsMasterClient)
        {
            if (WalletManager.Instance.raceId == 0)
            {
                totalLap = 1;
            }
            else if (WalletManager.Instance.raceId == 1)
            {
                totalLap = 3;
            }
            else if (WalletManager.Instance.raceId == 2)
            {
                totalLap = 5;
            }
            for (int i = 0; i < jockeys.Length; i++)
            {
                float[] hurd = new float[3];
                if (i == 0)
                {
                    hurd = WalletManager.Instance.playerOneHurd;
                }
                else if (i == 1)
                {
                    hurd = WalletManager.Instance.playerTwoHurd;
                }
                else if (i == 2)
                {
                    hurd = WalletManager.Instance.playerThreeHurd;
                }
                else if (i == 3)
                {
                    hurd = WalletManager.Instance.playerFourHurd;
                }
                else if (i == 4)
                {
                    hurd = WalletManager.Instance.playerFiveHurd;
                }
                if (jockeys[i] != null)
                {
                    float hurd1 = 0;
                    float hurd2 = 0;
                    float hurd3 = 0;
                    if (totalLap == 1)
                    {
                        hurd1 = hurd[0];
                    }
                    else if (totalLap == 3)
                    {
                        hurd1 = hurd[0];
                        hurd2 = hurd[1];
                        hurd3 = hurd[2];
                    }

                    Debug.LogError($"Assigning values to horse {jockeys[i].name} hurd1{hurd1} hurd2{hurd2} hurd3 {hurd3}");

                    int maxSpeed = Mathf.CeilToInt(WalletManager.Instance.horsesMaxSpeed[i] / 1.5f);
                    float acceleration = WalletManager.Instance.acceleration[i];
                    string address = WalletManager.Instance.address[i];
                    jockeys[i].RPCAssign(i, maxSpeed,
                    0,
                    acceleration,
                    address,
                    totalLap,
                    totalLap,
                    true,
                    hurd1,
                    hurd2,
                    hurd3);
                }
                else
                {
                    Debug.LogError($"Horse is null");
                }
            }
        }

        //CallDisableBetPanelRPC();

        StartCoroutine(ShowCountdown());
        yield return new WaitForSeconds(5f);
        StartRace();
    }

    private IEnumerator ShowCountdown()
    {
        _toastText.text = "5";
        LeanTween.alphaCanvas(_toast, 1, 1.5f).setOnComplete(() => { LeanTween.alphaCanvas(_toast, 0, 1.5f); });
        yield return new WaitForSecondsRealtime(1f);
        _toastText.text = "4";
        LeanTween.alphaCanvas(_toast, 1, 1.5f).setOnComplete(() => { LeanTween.alphaCanvas(_toast, 0, 1.5f); });
        yield return new WaitForSecondsRealtime(1f);
        _toastText.text = "3";
        LeanTween.alphaCanvas(_toast, 1, 1.5f).setOnComplete(() => { LeanTween.alphaCanvas(_toast, 0, 1.5f); });
        yield return new WaitForSecondsRealtime(1f);
        _toastText.text = "2";
        LeanTween.alphaCanvas(_toast, 1, 1.5f).setOnComplete(() => { LeanTween.alphaCanvas(_toast, 0, 1.5f); });
        yield return new WaitForSecondsRealtime(1f);
        _toastText.text = "1";
        LeanTween.alphaCanvas(_toast, 1, 1.5f).setOnComplete(() => { LeanTween.alphaCanvas(_toast, 0, 1.5f); });
    }

    private void StartRace()
    {
        foreach (var horse in jockeys)
        {
            horse.StartRace();
        }
    }

    //Everyone will generate random values but the ones host generated will be asigned to all at line 96
    private void GenerateRandomValues()
    {
        foreach (var jockey in jockeys)
        {
            jockey._maxSpeed = UnityEngine.Random.Range(4, 6);
            jockey._minSpeed = UnityEngine.Random.Range(1, 2);
            jockey._acceleration = UnityEngine.Random.Range(0.1f, 1.1f);
            jockey.totalLap = totalLap;
            jockey.lapLeft = totalLap;
            if(totalLap == 1)
            {
                jockey._lastLap = true;
            }
        }
    }

    [PunRPC]
    private void SyncJockeyValues(int jockeyIndex, float maxSpeed, float minSpeed, float acceleration, int totalLap, int lapLeft, bool lastLap)
    {
        jockeys[jockeyIndex]._maxSpeed = maxSpeed;
        jockeys[jockeyIndex]._minSpeed = minSpeed;
        jockeys[jockeyIndex]._acceleration = acceleration;
        jockeys[jockeyIndex].totalLap = totalLap;
        jockeys[jockeyIndex].lapLeft = lapLeft;
        if (totalLap == 1)
        {
            jockeys[jockeyIndex]._lastLap = lastLap;
        }
        //jockeys[jockeyIndex].StartRace();
    }

    [PunRPC]
    private void DisableBetPanel()
    {
        BetPanel.gameObject.SetActive(false);
    }

    // Call this method when you want to disable the BetPanel across the network
    private void CallDisableBetPanelRPC()
    {
        photonView.RPC("DisableBetPanel", RpcTarget.All);
    }
    private void Bet()
    {
        int.TryParse(tMP_InputField_totallap.text, out int totalLapVal);
        if (int.TryParse(tMP_InputField_betAmount.text, out int betAmount))
        {
            if(betAmount < DummyToken)
            {
                _Bet.interactable = false;
                _start.interactable = true;
                DummyToken = DummyToken - betAmount;
                jockeyId = tMP_Dropdown_Horsecolor.value;
                totalLap = totalLapVal;
                UpdateAtt?.Invoke();
            }
            else
            {
                _toastText.text = "Not enough token";
                LeanTween.alphaCanvas(_toast, 1, 1f).setOnComplete(() => { LeanTween.alphaCanvas(_toast, 0, 3f); });
            }
        }
        else
        {
            _toastText.text = "Not valid input";
            LeanTween.alphaCanvas(_toast, 1, 1f).setOnComplete(() => { LeanTween.alphaCanvas(_toast, 0, 3f); });
        }
    }

    public void UpdateAttributes()
    {
        _availableToken.text = "Token balance " + DummyToken.ToString();

    }

    private void OnDestroy()
    {
        UpdateAtt -= UpdateAttributes;
    }

    
    public void RPCPrintStack()
    {
        photonView.RPC("PrintStack", RpcTarget.AllViaServer);
    }

    [PunRPC]
    public void PrintStack()
    {
/*        foreach (var horse in horses)
        {
            Debug.LogError(horse.playerProperties.color);
        }*/
        if (horses.Count == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            foreach(var horse in jockeys)
            {
                horse.RPCEnableResultPanel();
            }

            while (horses.Count > 0 && horses.Count <= PhotonNetwork.CurrentRoom.PlayerCount)
            {
                _horseRanks.Add(horses.Pop());
            }
            _horseRanks.Reverse();
            int rank = 1;
            List<BString> winningOrder = new();
            foreach (var horse in _horseRanks)
            {
                if (rank > PhotonNetwork.CurrentRoom.PlayerCount) break;
                winningOrder.Add(new BString(horse.playerProperties.address.Replace("0x", "")));
                GameObject playerInfo = Instantiate(playerItem.gameObject, _content);
                _playerItemInstances.Add(playerInfo.GetComponent<PlayerItem>());
                playerInfo.GetComponent<PlayerItem>()._playerName.text = rank.ToString() + ". " + horse.GetName();
                playerInfo.GetComponent<PlayerItem>()._aptReward.text = "__";
                rank++;
            }
            if (PhotonNetwork.IsMasterClient) StartCoroutine(FindObjectOfType<EndRaceManager>().OnEndRace((ulong)WalletManager.Instance.raceId, winningOrder));
            StartCoroutine(WaitAndGetResults());
        }
    }



    private IEnumerator WaitAndGetResults()
    {
        _toastText.text = "Please sit tight and wait while we distribute your Aptos rewards.";
        LeanTween.alphaCanvas(_toast, 1, 6f).setOnComplete(() => { LeanTween.alphaCanvas(_toast, 0, 1f); });
        yield return new WaitUntil(()=>_completedRace);
        /*     if (PhotonNetwork.IsMasterClient) GetResults();*/
        /*if (PhotonNetwork.IsMasterClient) */
        foreach (var jockey in jockeys)
        {
            jockey.RPCGetResults();
        }
        foreach (var jockey in jockeys)
        {
            jockey.HeadBackInit();
        }
    }


    private async void GetResults()
    {
        UnityWebRequest request =
        await getResultsGraphql.Post("query MyQuery {\r\n  coin_activities(\r\n    where: {owner_address: {_eq: \"0x3e79e6c4f4d55299f09b3aef9a8ba33a2ba0f53d081336c3811c3e4712a8d48b\"}, _and: {entry_function_id_str: {_eq: \"0x3e79e6c4f4d55299f09b3aef9a8ba33a2ba0f53d081336c3811c3e4712a8d48b::aptos_horses_game::on_race_end\"}}}\r\n    limit: 5\r\n    order_by: {block_height: desc, amount: desc}\r\n  ) {\r\n    amount\r\n    transaction_version\r\n    event_index\r\n  }\r\n}");

        if (request.result == UnityWebRequest.Result.Success)
        {
            string data = request.downloadHandler.text;
            Debug.LogError($"Data for rewards {data}");
            if (!string.IsNullOrEmpty(data))
            {
                CoinActivitiesResponse response = JsonUtility.FromJson<CoinActivitiesResponse>(data);
                var tempIndex = 0;
                if (response != null && response.data != null && response.data.coin_activities != null)
                {
                    foreach (CoinActivity activity in response.data.coin_activities)
                    {
                        Debug.Log("Amount: " + activity.amount / 100000000);
                        _playerItemInstances[tempIndex++]._aptReward.text = "+"+(activity.amount/100000000).ToString();
                    }
                }
                else
                {
                    Debug.LogError("Failed to deserialize JSON or JSON data is empty");
                }
                
            }
        }
        else
        {
            Debug.LogError("Request failed: " + request.error);
        }
    }

    public void SetToastText(string v)
    {
        _toastText.text = v;
    }
}
