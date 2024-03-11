using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.UI;

public class UIManagerMainMenu : MonoBehaviour
{
    [Header("Addressable")]
    [SerializeField] private AssetLabelReference addressableWeekOne; 
    [SerializeField] private AssetLabelReference addressableWeekTwo; 
    [SerializeField] private AssetLabelReference addressableWeekThree; 
    [SerializeField] private AssetLabelReference addressableWeekFour;
    [SerializeField] private AssetLabelReference addressableWeekFive;
    [SerializeField] private AsyncOperationHandle<SceneInstance> handle; 

    [Header("Button")]
    [SerializeField]private Button _settingBtn;
    [SerializeField]private Button _disableSettingBtn;
    [SerializeField]private TMP_Dropdown _selectPlatform;
    [SerializeField]private TMP_InputField week_name;
    [SerializeField]private Button _sceneLoad;



    [Header("Panel")] 
    [SerializeField]private GameObject _settingPanel;
    [SerializeField]private GameObject _loading;
    [SerializeField]private GameObject _weekMapSelect;
    [SerializeField]private MapIndexing []_mapIndexing;
    
    public int defaultPlatform = 0;

    private int modeSelected = -1;
    [Header("Mode")]
    public Button _weeklyChallenge;
    public Button _onChainQuest;
    public Button _PlayervsPlayer;
    public TMP_Text _weeklyChallengetxt;
    public TMP_Text _onChainQuesttxt;
    public TMP_Text _PlayervsPlayertxt;
    public TextMeshProUGUI lockedText;
    public GameObject _modeSelectionPanel;
    public int _weeklyChallengeScene;
    public int _onChainQuestScene;
    public int _PlayervsPlayerScene;
    
    private void Start()
    {
        PlayerPrefs.SetInt("Platform", defaultPlatform);
        _settingBtn.onClick.AddListener(() => { ToggleSettingPanel(true);});
        _disableSettingBtn.onClick.AddListener(() => { ToggleSettingPanel(false);});
        
        //SetListerner
        
        _weeklyChallenge.onClick.AddListener(SelectModeWeeklyChallenge);
        _onChainQuest.onClick.AddListener(SelectModeOnChainQuest);
        _PlayervsPlayer.onClick.AddListener(SelectModePlayervsPlayer);
        _sceneLoad.onClick.AddListener(LoadScene);
    }

    public async void LoadScene()
    {
/*        if (week_name.text == "Week-1 OT")
        {
            handle = Addressables.LoadSceneAsync(addressableWeekOne, UnityEngine.SceneManagement.LoadSceneMode.Single);
            await handle.Task;
        }
        else if (week_name.text == "Week-2")
        {
            handle = Addressables.LoadSceneAsync(addressableWeekTwo, UnityEngine.SceneManagement.LoadSceneMode.Single);
            await handle.Task;
        }
        else if (week_name.text == "Week-3")
        {
            handle = Addressables.LoadSceneAsync(addressableWeekThree, UnityEngine.SceneManagement.LoadSceneMode.Single);
            await handle.Task;
        }
        else if (week_name.text == "Week-4")
        {
            handle = Addressables.LoadSceneAsync(addressableWeekFour, UnityEngine.SceneManagement.LoadSceneMode.Single);
            await handle.Task;
        }
        else if (week_name.text == "Week-5")
        {
            handle = Addressables.LoadSceneAsync(addressableWeekFive, UnityEngine.SceneManagement.LoadSceneMode.Single);
            await handle.Task;
        }*/
        SceneManager.instance.LoadScene(week_name.text);
    }

    private void ToggleSettingPanel(bool val)
    {
        _settingPanel.SetActive(val);
    }

    public void OnDropdownValueChanged()
    {
        Debug.LogError($"val {_selectPlatform.value}");
        defaultPlatform = _selectPlatform.value;
        PlayerPrefs.SetInt("Platform", defaultPlatform);
    }

    public void DisablePanel(GameObject _obj)
    {
        _obj.SetActive(false);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void SelectModePlayervsPlayer()
    {
        string tempstring = _PlayervsPlayertxt.text;
        modeSelected = _PlayervsPlayerScene;
        _PlayervsPlayertxt.text = "Selected";
        _modeSelectionPanel.SetActive(false);
        _PlayervsPlayertxt.text = tempstring;
        SceneManager.instance.LoadScene("Lobby");
    }
    
    public void SelectModeWeeklyChallenge()
    {
        string tempstring = _weeklyChallengetxt.text;
        modeSelected = _weeklyChallengeScene;
        _weeklyChallengetxt.text = "Selected";
        _modeSelectionPanel.SetActive(false);
        _weeklyChallengetxt.text = tempstring;
        _weekMapSelect.gameObject.SetActive(true);
    }
    
    public void SelectModeOnChainQuest()
    {
        string tempstring = _onChainQuesttxt.text;
        modeSelected = _onChainQuestScene;
        _onChainQuesttxt.text = "Selected";
        _modeSelectionPanel.SetActive(false);
        _onChainQuesttxt.text = tempstring;
    }
    
    public async void CheckCharacter()
    {
        _modeSelectionPanel.SetActive(true);
/*        var tx = new Transaction()
        {
            FeePayer = SolanaManager.Instance.sessionWallet.Account.PublicKey,
            Instructions = new List<TransactionInstruction>(),
            RecentBlockHash = await Web3.BlockHash()
        };

        PublicKey.TryFindProgramAddress(new[]{
            System.Text.Encoding.UTF8.GetBytes("PLAYER_CHARACTER"),
            Web3.Account.PublicKey,
            new PublicKey("EiM7i2o3LwP4YRYz4YxZSnVf6tLdvsn3YoiZnmjeZdpi")
            }, SolanaManager.Instance.programId, out PublicKey playerCharacterPDA, out var _);

        SetCurrentPlayerCharacterAccounts playerAccounts = new()
        {
            Player = SolanaManager.Instance.playerPDA,
            Signer = SolanaManager.Instance.sessionWallet.Account.PublicKey,
            SessionToken = SolanaManager.Instance.sessionWallet.SessionTokenPDA,
            SystemProgram = SystemProgram.ProgramIdKey,
            PlayerCharacterAccount = playerCharacterPDA
        };

        tx.Add(IntrantInferisProgram.SetCurrentPlayerCharacter(playerAccounts, SolanaManager.Instance.programId));

        RequestResult<string> res = await SolanaManager.Instance.sessionWallet.SignAndSendTransaction(tx);
        Debug.Log($"Result: {Newtonsoft.Json.JsonConvert.SerializeObject(res)}");

        var client = new IntrantInferisClient(Web3.Rpc, Web3.WsRpc, SolanaManager.Instance.programId);
        var result = await client.GetPlayerCharacterAsync(playerCharacterPDA);
        if (result.ParsedResult.Locked)
        {
            lockedText.gameObject.SetActive(true);
            await Task.Delay(3000);
            lockedText.gameObject.SetActive(false);
        }
        else
        {
            _modeSelectionPanel.SetActive(true);
        }*/
    }
    
    public void StartGame()
    {
        if (modeSelected == -1)
        {
            return;
        }

        if (modeSelected == 1)
        {
            foreach (MapIndexing map in _mapIndexing)
            {
                if (map._selected)
                {
                    string _sceneString = "Week-" + map._itemIndex;
                    Debug.LogError($"SceneToBeLoaded {_sceneString}");
                    SceneManager.instance.LoadScene(_sceneString);
                }
            }
        }else if (modeSelected == 4)
        {
            SceneManager.instance.LoadScene("PvP");
        }
    }


}
