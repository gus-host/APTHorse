using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.UI;

public enum MAPName
{
    WEEK1,
    WEEK2,
    WEEK3,
    WEEK4,
    WEEK5,
    WEEK6,
    WEEK7,
    WEEK8,
    WEEK9,
    WEEK10,
    WEEK11,
    WEEK12,
    WEEK13,
    WEEK14,
    WEEK15,
    WEEK16,
    WEEK17,
    WEEK18,
    WEEK19,
    WEEK20,
    WEEK21,
    WEEK22,
    WEEK23,
    WEEK24,
    WEEK25,
}

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public MAPName _mapName;
    [Header("Addressable")]
    [SerializeField] private AssetLabelReference addressableRef;
    [SerializeField] private GameObject addressableInstance;
    [SerializeField] private AsyncOperationHandle<SceneInstance> handle;
    // URL of the AssetBundle on the server
    public string assetBundleURL = "https://storage.googleapis.com/testbucketexp/Android/";

    // Name of the AssetBundle file
    public string assetBundleName = "";
    
    [Header("Player SpotLight Color")] 
    public Color _playerSpotLightColor;
    
    [Header("Player SpotLight Color")] 
    public Material transRoofMat;
    public Material transWallMat;
    public Material transStairMat;
    public Material transPillarMat;
   
    public Material WallMat;
    public Material RoofMat;
    public Material StairMat;
    public Material PillarMat;
    
    [Header("--------Minimap Pointer---------")]
    [SerializeField] public GameObject clawPointer;
    [SerializeField] public GameObject orbPointer;
    


    [Header("--------Boss Sector---------")]    
    [SerializeField] private GameObject _opponentPointer;
    [SerializeField] private GameObject _playerPointer;
    public GameObject bossEnemy;
    public GameObject BossEnemy_HealthUI;
    public static int enemyDead_Count = 0;

    [Header(" ")]
    [Header(" ")]
    [Header(" ")]
    [Header("--------Player Sector---------")]
    [SerializeField] public GameObject[] _player;
    [SerializeField] public IntrantThirdPersonController playerController;
    [SerializeField] public GameObject _spawnPointPlayer;
    [SerializeField] public GameObject _currentPlayerRef;


    public CharacterSoundFX sfx;


    [Header("Loading")]
    [SerializeField]
    public GameObject _loadingScreen;
    public Image loadingSlider;
    public TMP_Text loadingText;
    public TMP_Text smallloadingText;

    [Header("Initial Screen")]
    [SerializeField]
    public GameObject _initialScreen;

    [Header("Initial Screen")]

    [Header("TheraCode")]
    [SerializeField]
    public int _maxTheraCode;
    
    [SerializeField]
    private GameObject[] _theraCodeSpawnPoints;

    [SerializeField]
    private GameObject _theraCode;
    [Header("MagicScroll")]
    [SerializeField]
    public int _maxMagicscroll;

    [SerializeField]
    private GameObject[] _magicScrollSpts;

    [SerializeField]
    private GameObject _magicScroll;


    [Header("Bools")]
    [SerializeField] public bool _gameEnded = false;
    [SerializeField] public bool _monsterEmerged = false;
    [SerializeField] public bool _clawSpawned = false;
    [SerializeField] public bool _clawDestroyed = false;
    [SerializeField] public bool _loadAssetsFromAssetbundle = false;

    [SerializeField]
    public PlayerStage _PlayerStage;

    [Header("--------Maps Activities Objects---------")]
    public GameObject portalOne;
    public GameObject portalTwo;
    public GameObject portalThree;
    public GameObject celorian;
    public GameObject []celorianSpts;
    public GameObject []celorianTypeTwoSpts;
    public GameObject []celorianTypeThreeSpts;
    public Stack<GameObject> _celorianSptsStack = new Stack<GameObject>();
    public List<GameObject> _celorianTypeThreeRefList = new List<GameObject>();
    public GameObject MagicalKey;
    public GameObject _mira;
    public GameObject _miraMinimapPointer;
    public GameObject _miraSpts;
    public GameObject _chestSpawner;
    public GameObject _healthPotions;
    public GameObject _healthPotionPointer;
    public GameObject _powerPotions;
    public GameObject _powerPotionPointer;
    public GameObject [] _healthPotionsSpts;
    public GameObject [] _powerPotionsSpts;
    public GameObject _hiidenPath;

    [Header("--------Maps Activities Counts---------")]
    public int maxCelorins;

    [Header("--------Story Trigger---------")]
    public GameObject _stoneEncloser;



    [Header("--------Props Sector---------")]
    [Header("Claw")]
    public GameObject _claw;
    public GameObject clawRef;
    public GameObject []_clawSpts;


    [Header("Orbs of kinesis")]
    public GameObject _orbsOfKinesis;
    public GameObject _orbsOfKinesisPointer;
    public GameObject []_orbsOfKinesisSp;
    public int _numberOrOrbsofKinesis = 3;

    [Header("PostProcessing")] 
    
    public PostProcessProfile postProcessingProfile;
    public Vignette vignette;
    
    
    private void OnEnable()
    {
        instance = this;
    }

    private void Awake()
    {
        #region Load Bundle
        if (_loadAssetsFromAssetbundle)
        {
            StartCoroutine(LoadAsset());
        }
        #endregion
    }

    private void Start()
    {
        Stack<GameObject> usedSpawnPoints = new Stack<GameObject>();
        Stack<GameObject> usedSpawnPointsOfKinesis = new Stack<GameObject>();
        _currentPlayerRef = Instantiate(_player[0], _spawnPointPlayer.transform.position, _spawnPointPlayer.transform.rotation);
        _currentPlayerRef.GetComponent<PlayerLightManager>().playerSpotlight.color = _playerSpotLightColor;
        playerController = _currentPlayerRef.GetComponent<IntrantThirdPersonController>();
        PlayerSeeThroughEffectManager _playerSeeThrough = playerController._PlayerSeeThrough;
        Debug.LogError("Pausing player before startup");
        Debug.LogError("Pausing player before startup");
        playerController.pause = true;
        playerController.postProcessProfile = postProcessingProfile;
        playerController.vignette = vignette;
        IntrantThirdPersonController.UnlockedDoor += BringForthMira;

        #region Set Map
        if (_mapName == MAPName.WEEK1)
        {
            playerController._map = MAP.WEEK1;
        }else if (_mapName == MAPName.WEEK2)
        {
            playerController._map = MAP.WEEK2;
        }else if (_mapName == MAPName.WEEK3)
        {
            playerController._map = MAP.WEEK3;
        }else if (_mapName == MAPName.WEEK4)
        {
            playerController._map = MAP.WEEK4;
        }else if (_mapName == MAPName.WEEK5)
        {
            playerController._map = MAP.WEEK5;
        }else if (_mapName == MAPName.WEEK6)
        {
            playerController._map = MAP.WEEK6;
        }else if (_mapName == MAPName.WEEK7)
        {
            playerController._map = MAP.WEEK7;
        }else if (_mapName == MAPName.WEEK8)
        {
            playerController._map = MAP.WEEK8;
        }else if (_mapName == MAPName.WEEK9)
        {
            playerController._map = MAP.WEEK9;
        }else if (_mapName == MAPName.WEEK10)
        {
            playerController._map = MAP.WEEK10;
        }else if (_mapName == MAPName.WEEK11)
        {
            playerController._map = MAP.WEEK11;
        }else if (_mapName == MAPName.WEEK12)
        {
            playerController._map = MAP.WEEK12;
        }else if (_mapName == MAPName.WEEK13)
        {
            playerController._map = MAP.WEEK13;
        }else if (_mapName == MAPName.WEEK14)
        {
            playerController._map = MAP.WEEK14;
        }else if (_mapName == MAPName.WEEK15)
        {
            playerController._map = MAP.WEEK15;
        }else if (_mapName == MAPName.WEEK16)
        {
            playerController._map = MAP.WEEK16;
        }else if (_mapName == MAPName.WEEK17)
        {
            playerController._map = MAP.WEEK17;
        }else if (_mapName == MAPName.WEEK18)
        {
            playerController._map = MAP.WEEK18;
        }else if (_mapName == MAPName.WEEK19)
        {
            playerController._map = MAP.WEEK19;
        }else if (_mapName == MAPName.WEEK20)
        {
            playerController._map = MAP.WEEK20;
        }else if (_mapName == MAPName.WEEK21)
        {
            playerController._map = MAP.WEEK21;
        }else if (_mapName == MAPName.WEEK22)
        {
            playerController._map = MAP.WEEK22;
        }else if (_mapName == MAPName.WEEK23)
        {
            playerController._map = MAP.WEEK23;
        }else if (_mapName == MAPName.WEEK24)
        {
            playerController._map = MAP.WEEK24;
        }else if (_mapName == MAPName.WEEK25)
        {
            playerController._map = MAP.WEEK25;
        }
        #endregion

        #region Assign Transparent MAT

        if (transRoofMat != null)
        {
            _playerSeeThrough.transRoofMat = transRoofMat;
        }

        if (transWallMat != null)
        {
            _playerSeeThrough.transWallMat = transWallMat;
        }
        
        if (transStairMat != null)
        {
            _playerSeeThrough.transStairMat = transStairMat;
        }
        
        if (transPillarMat != null)
        {
            _playerSeeThrough.transPillarMat = transPillarMat;
        }
        
        if (WallMat != null)
        {
            _playerSeeThrough.WallMat = WallMat;
        }
        
        if (RoofMat != null)
        {
            _playerSeeThrough.RoofMat = RoofMat;
        }
        if (StairMat != null)
        {
            _playerSeeThrough.StairMat = StairMat;
        }        
        if (PillarMat != null)
        {
            _playerSeeThrough.PillarMat = PillarMat;
        }

        #endregion

        CameraFollowingPlayer.Instance.Player = _currentPlayerRef;
        try
        {
            SeethroughWall.Instance.lookPoint = _currentPlayerRef.transform;
        }catch(Exception e)
        {

        }

        if (_mapName == MAPName.WEEK1)
        {
            _PlayerStage.CreateWallBreakPotion();
            CreateTheraCode(usedSpawnPoints);
        }
        else if (_mapName == MAPName.WEEK2)
        {
            int index = 0;
            int indextemp = 0;
            for (int i = 1; i <= _numberOrOrbsofKinesis; i++)
            {
                index = UnityEngine.Random.Range(0, _orbsOfKinesisSp.Length);
                if (!usedSpawnPointsOfKinesis.Contains(_orbsOfKinesisSp[index]))
                {
                    usedSpawnPointsOfKinesis.Push(_orbsOfKinesisSp[index]);
                    GameObject _orbsOfKinseisRef = Instantiate(_orbsOfKinesis, _orbsOfKinesisSp[index].transform.position, quaternion.identity);
                    GameObject _orbsOfKinseisRefPointer = Instantiate(_orbsOfKinesisPointer, _orbsOfKinesisSp[index].transform.position, quaternion.identity);
                    _orbsOfKinseisRefPointer.GetComponent<PlayerMinimapPointer>().target = _orbsOfKinseisRef.transform;
                }
                else
                {
                    i--;
                }
            }
        }
        else if(_mapName == MAPName.WEEK3)
        {
            for (int i = 0; i < _maxTheraCode; i++)
            {
                int _index = UnityEngine.Random.Range(0, _theraCodeSpawnPoints.Length);
                if (usedSpawnPoints.Contains(_theraCodeSpawnPoints[_index]))
                {
                    i--;
                    continue;
                }
                Vector3 _spawnPoint = new Vector3(_theraCodeSpawnPoints[_index].transform.position.x,
                    _theraCodeSpawnPoints[_index].transform.position.y + 0.2f,
                    _theraCodeSpawnPoints[_index].transform.position.z);

                GameObject _g = Instantiate(_theraCode, _spawnPoint, Quaternion.identity);
                usedSpawnPoints.Push(_theraCodeSpawnPoints[_index]);
            }
            for (int i = 0; i < _maxMagicscroll; i++)
            {
                usedSpawnPoints.Clear();
                int _index = UnityEngine.Random.Range(0, _magicScrollSpts.Length);
                if (usedSpawnPoints.Contains(_magicScrollSpts[_index]))
                {
                    i--;
                    continue;
                }
                Vector3 _spawnPoint = new Vector3(_magicScrollSpts[_index].transform.position.x,
                    _magicScrollSpts[_index].transform.position.y + 0.2f,
                    _magicScrollSpts[_index].transform.position.z);

                GameObject _g = Instantiate(_magicScroll, _spawnPoint, Quaternion.identity);
                usedSpawnPoints.Push(_magicScrollSpts[_index]);
            }
        }
        else if (_mapName == MAPName.WEEK4)
        {
            
        }
        else if (_mapName == MAPName.WEEK5)
        {
            for(int i = 0; i<celorianSpts.Length;i++)
            {
                int index = UnityEngine.Random.Range(0, celorianSpts.Length);

                if (_celorianSptsStack.Count < 1)
                {
                    GameObject _celorianRef = Instantiate(celorian, celorianSpts[index].transform.position, Quaternion.identity);
                    _celorianRef.GetComponent<GeneralCharacterManager>()._characterType = GeneralCharacterManager.CharacterType.Celorian;
                    _celorianSptsStack.Push(celorianSpts[index]);
                }

                if (!_celorianSptsStack.Contains(celorianSpts[index]))
                {
                    GameObject _celorianRef = Instantiate(celorian, celorianSpts[index].transform.position, Quaternion.identity);
                    _celorianRef.GetComponent<GeneralCharacterManager>()._characterType = GeneralCharacterManager.CharacterType.Celorian;
                    _celorianSptsStack.Push(celorianSpts[index]);
                }
                else if(_celorianSptsStack.Contains(celorianSpts[index]) && _celorianSptsStack.Count != celorianSpts.Length)
                {
                    i--;
                }
            }
            _celorianSptsStack.Clear();
            for (int i = 0; i < celorianTypeTwoSpts.Length; i++)
            {
                int index = UnityEngine.Random.Range(0, celorianTypeTwoSpts.Length);

                if (_celorianSptsStack.Count < 1)
                {
                    GameObject _celorianRef = Instantiate(celorian, celorianTypeTwoSpts[index].transform.position, Quaternion.identity);
                    _celorianRef.GetComponent<GeneralCharacterManager>()._characterType = GeneralCharacterManager.CharacterType.CelorianTypeTwo;
                    _celorianSptsStack.Push(celorianTypeTwoSpts[index]);
                }

                if (!_celorianSptsStack.Contains(celorianTypeTwoSpts[index]))
                {
                    GameObject _celorianRef = Instantiate(celorian, celorianTypeTwoSpts[index].transform.position, Quaternion.identity);
                    _celorianRef.GetComponent<GeneralCharacterManager>()._characterType = GeneralCharacterManager.CharacterType.CelorianTypeTwo;
                    _celorianSptsStack.Push(celorianTypeTwoSpts[index]);
                }
                else if (_celorianSptsStack.Contains(celorianTypeTwoSpts[index]) && _celorianSptsStack.Count != celorianTypeTwoSpts.Length)
                {
                    i--;
                }
            }
            _celorianSptsStack.Clear();
            for (int i = 0; i < celorianTypeThreeSpts.Length; i++)
            {
                int index = UnityEngine.Random.Range(0, celorianTypeThreeSpts.Length);

                if (_celorianSptsStack.Count < 1)
                {
                    GameObject _celorianRef = Instantiate(celorian, celorianTypeThreeSpts[index].transform.position, Quaternion.identity);
                    _celorianRef.GetComponent<GeneralCharacterManager>()._characterType = GeneralCharacterManager.CharacterType.CelorianTypeThree;
                    _celorianSptsStack.Push(celorianTypeThreeSpts[index]);
                    _celorianTypeThreeRefList.Add(_celorianRef);
                }

                if (!_celorianSptsStack.Contains(celorianTypeThreeSpts[index]))
                {
                    GameObject _celorianRef = Instantiate(celorian, celorianTypeThreeSpts[index].transform.position, Quaternion.identity);
                    _celorianRef.GetComponent<GeneralCharacterManager>()._characterType = GeneralCharacterManager.CharacterType.CelorianTypeThree;
                    _celorianSptsStack.Push(celorianTypeThreeSpts[index]);
                    _celorianTypeThreeRefList.Add(_celorianRef);
                }
                else if (_celorianSptsStack.Contains(celorianTypeThreeSpts[index]) && _celorianSptsStack.Count != celorianTypeThreeSpts.Length)
                {
                    i--;
                }
            }
            CreateTheraCode(usedSpawnPoints);
        }

        //Remove after addressable implementation
        if(!_loadAssetsFromAssetbundle)
        {
            StartCoroutine(InitiateScreen());
        }

        Invoke("SetPointer",1f);
        GeneralCharacterManager._turnedtoStoneAction += GhostTurnedToStone;
    }

    IEnumerator LoadAsset()
    {
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(addressableRef);

        while (!handle.IsDone)
        {
            // Update the loading progress
            float progress = handle.PercentComplete;
            _loadingScreen.SetActive(true);
            loadingText.gameObject.SetActive(true);
            loadingSlider.fillAmount = progress;
            loadingText.text = "Loading asset...";
            smallloadingText.text = "Hang in there...";
            // Optionally, you can yield here to give Unity time to update the UI
            yield return new WaitForSeconds(0.1f);
        }

        // Hide the loading screen after the download is complete
        _loadingScreen.SetActive(false);
        StartCoroutine(InitiateScreen());

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Instantiate(handle.Result);
            addressableInstance = handle.Result;
        }
        else
        {
            Debug.LogError($"Operation Failed: {handle.OperationException}");
        }
    }



    private void CreateTheraCode(Stack<GameObject> usedSpawnPoints)
    {
        for (int i = 0; i < _maxTheraCode; i++)
        {
            int _index = UnityEngine.Random.Range(0, _theraCodeSpawnPoints.Length);
            if (usedSpawnPoints.Contains(_theraCodeSpawnPoints[_index]))
            {
                i--;
                continue;
            }
            Vector3 _spawnPoint = new Vector3(_theraCodeSpawnPoints[_index].transform.position.x,
                _theraCodeSpawnPoints[_index].transform.position.y + 0.2f,
                _theraCodeSpawnPoints[_index].transform.position.z);

            GameObject _g = Instantiate(_theraCode, _spawnPoint, Quaternion.identity);
            _theraCode.GetComponent<Rigidbody>().isKinematic = true;
            _theraCode.GetComponent<BoxCollider>().isTrigger = true;
            usedSpawnPoints.Push(_theraCodeSpawnPoints[_index]);
        }
    }

    public void GhostTurnedToStone(GameObject _obj)
    {
        _celorianTypeThreeRefList.Remove(_obj);
        if (_celorianTypeThreeRefList.Count < 1)
        {
            Debug.LogError("Magic Key");
            Vector3 _newPos = new Vector3(_obj.transform.position.x+2f, _obj.transform.position.y+1, _obj.transform.position.z +2f);
            GameObject key = Instantiate(MagicalKey, _newPos, Quaternion.identity);
            playerController._playerTimelineManager.InitCollectKey();
        }
    }
    IEnumerator InitiateScreen()
    {
        _initialScreen.SetActive(true);
        yield return new WaitForSeconds(1f);
        _currentPlayerRef.GetComponent<IntrantThirdPersonController>().pause = false;
        _initialScreen.SetActive(false);
    }
    public void SetPointer()
    {
        Debug.LogError("Setting Pointer");
        List<MonsterMovementController> _monsters = new List<MonsterMovementController>(FindObjectsOfType<MonsterMovementController>());

        foreach (var item in _monsters)
        {
            GameObject _g = Instantiate(_opponentPointer);
            _g.GetComponent<PlayerMinimapPointer>().target = item.gameObject.transform;
            item._minimapPointer = _g;
            item._minimapPointer.SetActive(false);
        }
        GameObject _h = Instantiate(_playerPointer);
        _h.GetComponent<PlayerMinimapPointer>().target = _currentPlayerRef.gameObject.transform;
        _currentPlayerRef.GetComponent<IntrantThirdPersonController>()._playerMinimapPointer = _h;
    }

    private void BringForthMira()
    {
        GameObject mira = Instantiate(_mira, _miraSpts.transform.position, Quaternion.identity);
        GameObject _orbsOfKinseisRefPointer = Instantiate(_miraMinimapPointer);
        _orbsOfKinseisRefPointer.GetComponent<PlayerMinimapPointer>().target = mira.transform;
    }

    private void Update()
    {
        if(_gameEnded)
        {
            EnemyController []_enemies = FindObjectsOfType<EnemyController>();
            foreach (EnemyController _enemy in _enemies) 
            {
                Destroy(_enemy.gameObject);
            }
            _gameEnded = false;
        }
        if(_mapName == MAPName.WEEK3)
        {
            if(!_clawSpawned && _monsterEmerged)
            {
                clawRef = Instantiate(_claw,_clawSpts[UnityEngine.Random.Range(0, _clawSpts.Length)].transform.position, Quaternion.identity);
                _clawSpawned = true;
            }
            if(_clawSpawned && !_clawDestroyed && clawRef == null)
            {
                _clawDestroyed = true;
                float portalOnedesiredY = portalOne.transform.position.y - 15.0f;
                LeanTween.moveY(portalOne, portalOnedesiredY, 1f);
                float portalTwodesiredY = portalTwo.transform.position.y - 15.0f;
                LeanTween.moveY(portalTwo, portalTwodesiredY, 1f);
                float portalThreedesiredY = portalThree.transform.position.y - 15.0f;
                LeanTween.moveY(portalThree, portalThreedesiredY, 1f);
                _stoneEncloser.SetActive(true);
            }
        }

    }
    
    public void onReplayBtn_Click()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex); // Reload the current scene
    }

    public void onQuitBtn_Click()
    {
        Application.Quit(); // Quit the game
    }

    private void OnDestroy()
    {
        IntrantThirdPersonController.UnlockedDoor -= BringForthMira;
        Resources.UnloadUnusedAssets();
        Addressables.Release(addressableInstance);
    }
}
