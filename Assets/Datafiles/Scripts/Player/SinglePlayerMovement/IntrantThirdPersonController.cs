using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using System.Threading.Tasks;
using Photon.Pun;

public enum MAP
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
    WEEK25
}

public class PlayerDataJson
{
    public string name;
    public bool _receivedTheraBox;
    public bool _receivedTheraCode;
    public bool _receivedAsimanaCodex;
    public bool _placedTheraCode;
    public bool _mainFrameEnabled;
    public bool _beenInAlkemiaChamber;
    public bool _headingToTerathsGate;
    public bool _beenToRecoletteMirror;
    public bool _collectedStarDevice;
    public bool _inOrbsOfKinesis;
    public bool _collectedMiniGameOrbs;
    public bool _insideOrbsEmitter;
    public bool _placedOrbs;
    public bool _collectedLastOrb;
    public bool _fightedKaire;
    public bool _playerenteredSafetyEnclosure;
    public bool _collectedClaw;
    public bool _fixedStoneCourtyard;
    public bool _killedBossMonster;
    public bool _collectedMagicalKey;
    public bool _unlockedMagicalDoor;
    public bool _foundThrowingStar;
    public bool _OrinAndRasveusFixedMira;
    public bool _meronNftOne;
    public bool _collectedAssembledMeronNft;
    public bool _hasPowerPotion;
    public int _switchedOnCount;
    public int _collectedTherCode;
    public int _collectedOrbs;
    public int _magicScrolls;
    public int _collectedAdacode;
    public int _forearmsArmor;
    public int _bodyArmor;
    public int _ShoulderArmor;
    public int _collectedClawCount;
    public int _completedMissionNo;
}

public class IntrantThirdPersonController : IntrantThirdPersonAnimator
{
    public static IntrantThirdPersonController instance;
    private Rigidbody _rb;
    public PlayerRangeHandler _playerRangeHandler;

    //Action
    public static event Action CollectedAsimanaCodex;
    public static event Action UnlockedDoor;

    [SerializeField] private List<MonsterMovementController> _enemyController;
    [SerializeField] public PlayerSeeThroughEffectManager _PlayerSeeThrough;
    [SerializeField] public PlayerTimelineManager _playerTimelineManager;
    [SerializeField] public ForceField _forceField;


    [Header("VFX")] 
    public GameObject _shieldFx;
    public GameObject _healingfx;
    public GameObject _buff;
    public GameObject _debuff;
    public GameObject _darkMagic;
    public GameObject _earthShield;
    public GameObject _mysticalPower;
    public GameObject _weaponShadowAura;
    public GameObject _llumanaandMalumanaMagic;
    public GameObject _attackOnefirstSlice;
    public GameObject _attackOneSecondSlice;
    public GameObject _attackOneThirdSlice;
    public GameObject _attacktwoFirstSlice;
    public GameObject _attacktwoSecondSlice;
    public GameObject _powerUpAura;
    public GameObject _binaryHologram;
    public GameObject _integrationMagic;
    public GameObject _rasVeus;
    public GameObject _Orin;
    public GameObject _bossFerraptor;
    public GameObject _starDevice;
    

    public GameObject _playerMinimapPointer;
    public CanvasGroup _shoulder;
    public CanvasGroup []_forearms;
    public CanvasGroup _body;
    private readonly GameObject _shieldFxInstance = null;
    public GameObject _checkPointFx;
    public Collider _hammerCollider;
    public Collider _PlayerShield;
    public Hammer _hammer;
    public Transform _shieldFxParent;

    public bool isShieldActive;
    public bool isAttacking;
    public bool canSprint = true;
    private readonly float shieldCooldown = 2.5f;
    private static readonly int IsAttackingOneHash = Animator.StringToHash("AttackOne");
    private static readonly int IsAttackingTwoHash = Animator.StringToHash("AttackTwo");

    [Header("Bool")] 
    public bool _paused;
    public bool _shieldUp;
    public bool isMobilePlatform;
    public bool _gameFinished;
    public bool _isAnimating;
    public bool _receivedTheraBox;
    public bool _receivedTheraCode;
    public bool _receivedAsimanaCodex;
    public bool _placedTheraCode;
    public bool _jumpButtonPressed;
    public bool _mainFrameEnabled;
    public bool _beenInAlkemiaChamber;
    public bool _headingToTerathsGate;
    public bool _attackOneOnCooldown;
    public bool _attackTwoOnCooldown;
    public bool _shieldOnCooldown;
    public bool _beenToRecoletteMirror;
    public bool _collectedStarDevice;
    public bool _inOrbsOfKinesis;
    public bool _collectedMiniGameOrbs;
    public bool _insideOrbsEmitter;
    public bool _placedOrbs;
    public bool _collectedLastOrb;
    public bool _fightedKaire;
    public bool _playerenteredSafetyEnclosure;
    public bool _collectedClaw;
    public bool _fixedStoneCourtyard;
    public bool _killedBossMonster;
    public bool _collectedMagicalKey;
    public bool _unlockedMagicalDoor;
    public bool _foundThrowingStar;
    public bool _OrinAndRasveusFixedMira;
    public bool _meronNftOne;
    public bool _collectedAssembledMeronNft;
    public bool _hasPowerPotion;

    [Header("Materials")] 
    public Material _alkemmanaSwitchonMat;

    [Header("Count")] 
    public int _switchedOnCount;
    public int _collectedTherCode;
    public int _collectedOrbs;
    public int _magicScrolls;
    public int _collectedAdacode;
    public int _forearmsArmor;
    public int _bodyArmor;
    public int _ShoulderArmor;
    public int _collectedClawCount;
    public int _completedMissionNo;

    [Header("Panel")] 
    public GameObject _grimoirPanel;
    public GameObject _weaponPowerUpPanel;
    public GameObject _mirasChallengePanel;
    public CanvasGroup _darkPanel;
    public GameObject _canvas;
    public GameObject _gameOverCanvas;
    public ItemSlot _malumanaMagic;
    public ItemSlot _llumanaMagic;
    public CustomToast _toast;

    public float movement_Speed = 3f;
    public string currentAreaName = "";
    public Stack<string> visitedAreas = new Stack<string>();
    public MAP _map;

    [Header("Props")] 
    public PostProcessProfile postProcessProfile;
    public Vignette vignette;

    [Header("Checkpoints")]
    public CheckpointTag[] _checkpoints;

    #region Player Data
    [Header("Player Data")]

    [SerializeField] private string bucketName = "playerdata-1";
    [SerializeField] private string filename = "playerdata";

    #endregion

    #region UI

    public PvPPlayerUI _playerUI;
    public TMP_Text _theraCount;
    public Image _attackOneCooldown;
    public Image _attackTwoCooldown;
    public Image _shieldCooldown;
    public Image _sprintCooldown;

    #endregion

    public bool pause
    {
        get => _paused;
        set => _paused = value;
    }

    private void Awake()
    {
        var platform = PlayerPrefs.GetInt("Platform");
        _rb = GetComponent<Rigidbody>();
        if (platform == 0)
            GetComponent<IntrantPlayerInput>().currentPlatform = PlatformType.Mobile;
        else if (platform == 1) 
            GetComponent<IntrantPlayerInput>().currentPlatform = PlatformType.PC;

        
    }

    private async void Start()
    {
        instance = this;

        // Checkpoints
        _checkpoints = FindObjectsOfType<CheckpointTag>();



        _hammerCollider.enabled = false;

        #region GetBehaviourComponent
        _playerTimelineManager = GetComponent<PlayerTimelineManager>();
        #endregion

        PlayerRangeHandler.OnPlayerEntered += PlayerEnteredInRange;
        MonsterHealthController.OnPlayerExit += PlayerExitRange;

        #region DisableCooldownUI

        _shieldCooldown.gameObject.SetActive(false);
        _sprintCooldown.gameObject.SetActive(false);
        _attackOneCooldown.gameObject.SetActive(false);
        _attackTwoCooldown.gameObject.SetActive(false);

        #endregion

        if (_map == MAP.WEEK2)
        {
            GetComponent<PvPPlayerUI>()._grimoire.gameObject.SetActive(true);
        }

        #region Set Target In All Enemies

        var _monsters = FindObjectsOfType<MonsterMovementController>().ToList();

        foreach (var monster in _monsters) monster.SetTarget(transform);

        #endregion

        #region Handle Exception (if vignetta set to dark and player goes to main menu it remain black)
        if(_map == MAP.WEEK2)
        {
            if (postProcessProfile != null)
            {
                if (postProcessProfile.TryGetSettings(out vignette))
                {
                    // Modify the vignette intensity
                    vignette.center.value = new Vector2(0.5f, 0.5f);
                    vignette.intensity.value = 0.3f; // Change this value as needed
                }
            }
        }
        #endregion
    }



    private void PlayerEnteredInRange(MonsterMovementController _enemy)
    {
        if (_enemyController.Contains(_enemy)) return;
        _enemyController.Add(_enemy);
    }

    private void PlayerExitRange(MonsterMovementController _enemy)
    {
        if (_enemyController.Contains(_enemy)) _enemyController.Remove(_enemy);
    }

    #region Shiedld

    public void Shield()
    {
        if (pause) return;
        if (!_playerUI._shield.interactable) return;
        StartCoroutine(ActivateShield());
    }

    private IEnumerator ActivateShield()
    {
        if (_shieldUp) yield return null;

        // Set the shield active flag and enable the VFX

        if (_playerUI._shield.interactable) // Check if the button is not on cooldown
        {
            _shieldOnCooldown = true;
            // Set the button on cooldown
            _playerUI._shield.interactable = false;
            _shieldCooldown.gameObject.SetActive(true);

            var cooldownDuration = 30f; // Cooldown time in seconds
            StartCoroutine(StartCooldown(cooldownDuration,
                _shieldCooldown,
                _playerUI._shield,
                _shieldOnCooldown));
        }

        Debug.LogWarning("Shield Activated");
        _shieldUp = true;
        _shieldFx.SetActive(true);
        animator.SetBool("Shield", true);
        _PlayerShield.enabled = true;
        _forceField.Active = true;
        gameObject.layer = LayerMask.NameToLayer("Default");
        yield return new WaitForSeconds(2f);
        gameObject.layer = LayerMask.NameToLayer("Player");
        _shieldFx.SetActive(false);
        _PlayerShield.enabled = false;
        _forceField.Active = false;
        _shieldUp = false;
    }

    #endregion

    #region Reload Game

    public void Reload(string _sceneName, GameObject _loadingScreen, Image _fill)
    {
        StartCoroutine(LoadSceneAsync(_sceneName, _loadingScreen, _fill));
    }

    private IEnumerator LoadSceneAsync(string sceneName, GameObject _loadingScreen, Image _fill)
    {
        yield return new WaitForSeconds(2.5f);
        if (_loadingScreen) _loadingScreen.SetActive(true);
        var asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            // You can display a loading progress bar or perform other tasks here if needed
            var progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            if (_fill != null) _fill.fillAmount = progress;
            Debug.LogError("Loading progress: " + progress * 100 + "%");

            yield return null;
        }
    }

    #endregion

    #region Break Wall

    public void AtivateBreakWallUI()
    {
        _playerUI._wallBreakbtn.gameObject.SetActive(true);
        _playerUI._breakPotionCollect.gameObject.SetActive(false);
    }

    public void BreakWall()
    {
        SmashableWall._instance._canSmash = true;
    }

    #endregion

    #region Trigger

    private void OnTriggerEnter(Collider other)
    {
        //Teleport on Fall
        if (other.gameObject.CompareTag(Tags.DeadZone))
        {
            StartCoroutine(TelePort());
        }

        //Checkpoint 
        if (other.gameObject.TryGetComponent<CheckpointTag>(out CheckpointTag checkpointTag))
        {
            if(checkpointTag != null && _toast != null)
            {
                CheckPointManager(checkpointTag);
            }
        }

        #region Week 1
        if (_map == MAP.WEEK1)
        {
            if (other.gameObject.CompareTag(Tags.ASIMANAS_CODEX))
            {
                CollectedAsimanaCodex?.Invoke();
                _receivedAsimanaCodex = true;
                LeanTween.scale(other.gameObject, Vector3.zero, 1f);
                Destroy(other.gameObject, 1f);
            }

            if (other.gameObject.CompareTag("WallBreakSpell"))
            {
                _playerUI._breakPotionCollect.gameObject.SetActive(true);
                Destroy(other.gameObject, 0.1f);
            }

            if (other.gameObject.CompareTag(MapPoints.AlkemannaHotspotSwitch) && _receivedTheraCode &&
                _collectedTherCode == GameManager.instance._maxTheraCode + 1 && _beenInAlkemiaChamber)
            {
                _playerUI._switchOnAlkemmana.gameObject.SetActive(true);
                _playerUI._switchOnAlkemmana.onClick.RemoveAllListeners();
                _playerUI._switchOnAlkemmana.onClick.AddListener(() => {

                    ChangeMat(other.gameObject.GetComponent<MeshRenderer>(), _alkemmanaSwitchonMat);
                    other.gameObject.GetComponent<BoxCollider>().enabled = false;
                }
                );
            }else if (other.gameObject.CompareTag(MapPoints.AlkemannaHotspotSwitch) &&
                _collectedTherCode < GameManager.instance._maxTheraCode + 1)
            {
                _toast._text.text = "Need at least 6 theracode in order to turn on switch";
                _toast.ShowToast();
            }

            if (other.gameObject.CompareTag(Tags.THERABOX_TAG)) Destroy(other.gameObject, 0.3f);

            if (other.gameObject.CompareTag(Tags.TheraCode_TAG) && _collectedTherCode < 5)
            {
                _playerUI._collect.gameObject.SetActive(true);
                _playerUI._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Collect Theracode";
                _playerUI._collect.onClick.RemoveAllListeners();
                _playerUI._collect.onClick.AddListener(() => { CollectCode(other.gameObject); });
            }

            if (other.gameObject.CompareTag(Tags.HEALTH_TAG)) StartCoroutine(IncreaseHealth(other.gameObject));

            if(other.gameObject.TryGetComponent<ManualTag>(out ManualTag _manualTag))
            {
                if (_manualTag._tag == Tag.MazeGateWeekOne && GetComponent<PlayerTheraConnectionManager>()._theraActivated)
                {
                    _playerUI._collect.gameObject.SetActive(true);
                    _playerUI._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Open";
                    _playerUI._collect.onClick.RemoveAllListeners();
                    _playerUI._collect.onClick.AddListener(() => {
                        other.gameObject.GetComponent<Animator>().enabled = true;
                        _playerUI._collect.gameObject.SetActive(false);
                        _playerTimelineManager.TheraIsImportantInThisFight();
                        _manualTag.gameObject.GetComponent<BoxCollider>().enabled = false;
                        });
                }else
                {
                    _toast._text.text = "Thera should be along with you";
                    _toast.ShowToast();
                }
            }
        }
        #endregion

        #region Week-2
        if (_map == MAP.WEEK2)
        {
            if (other.gameObject.CompareTag(WeekTwo.StarDevice) && !_collectedStarDevice)
            {
                PvPPlayerUI.instance._collect.onClick.AddListener(()=>
                {
                    CollectStarDevice(other.gameObject);
                });
                PvPPlayerUI.instance._collect.gameObject.SetActive(true);
                PvPPlayerUI.instance._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Collect Starr Device";
            }

            else if (other.gameObject.CompareTag(WeekTwo.BinaryCircle))
            {
               EnableDarkness(other);
            }
            else if (other.gameObject.CompareTag(WeekTwo.AdacodeCrytex))
            {
                LeanTween.scale(other.gameObject, new Vector3(0, 0, 0), 0.5f);
                Destroy(other.gameObject,0.5f);
                GetComponent<GrimoireManager>().EnableAdacodeCryptex();
            }
            else if(other.gameObject.CompareTag(WeekTwo.AsimanaCryptex))
            {
                GetComponent<GrimoireManager>().RemoveMagicFromOrbs();
                LeanTween.scale(other.gameObject, new Vector3(0, 0, 0), 0.5f);
                Destroy(other.gameObject,0.5f);
            }
            else if (other.gameObject.CompareTag(WeekTwo.OrbsOfKinesisMiniGame))
            {
                PvPPlayerUI.instance._collect.onClick.RemoveAllListeners();
                PvPPlayerUI.instance._collect.gameObject.SetActive(true);
                PvPPlayerUI.instance._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Collect and Replace";
                PvPPlayerUI.instance._collect.onClick.AddListener(()=>
                {
                    CollectOrbMiniGame(other.gameObject);
                });
            }
            else if (other.gameObject.CompareTag(WeekTwo.OrbsOfKinesisTwo))
            {
                PvPPlayerUI.instance._collect.onClick.RemoveAllListeners();
                PvPPlayerUI.instance._collect.gameObject.SetActive(true);
                PvPPlayerUI.instance._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Collect orb";
                PvPPlayerUI.instance._collect.onClick.AddListener(()=>
                {
                    CollectOrbTwo(other.gameObject);
                });
            }
            else if (other.gameObject.CompareTag(WeekTwo.OrbsOfKinesisThree))
            {
                PvPPlayerUI.instance._collect.onClick.RemoveAllListeners();
                PvPPlayerUI.instance._collect.gameObject.SetActive(true);
                PvPPlayerUI.instance._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Collect orb";
                PvPPlayerUI.instance._collect.onClick.AddListener(()=>
                {
                    CollectOrbTwo(other.gameObject);
                });
            }
            else if (other.gameObject.CompareTag(WeekTwo.SubmitOrb) && _collectedOrbs > 2)
            {
                _insideOrbsEmitter = true;
                PvPPlayerUI.instance._collect.gameObject.SetActive(true);
                PvPPlayerUI.instance._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Place Orbs";
                PvPPlayerUI.instance._collect.onClick.RemoveAllListeners();
                PvPPlayerUI.instance._collect.onClick.AddListener(() =>
                {
                    StartCoroutine(PlaceOrbs(other));
                });
            }else if (other.gameObject.CompareTag(WeekTwo.ShadowBlast))
            {
                Invoke("Blurred", 1f);
                
                Invoke("LoadScen",3f);
            }
        }
        #endregion

        #region Week-3
        if (_map == MAP.WEEK3)
        {
            if (other.gameObject.CompareTag(MapPointsWeekThree.MagicScroll))
            {
                LeanTween.scale(other.gameObject,new Vector3(0, 0, 0), 1f);
                Destroy(other.gameObject, 1f);
                _magicScrolls++;
                GetComponent<GrimoireManager>()._magicScrolls.text = _magicScrolls.ToString();
            }
            if (other.gameObject.CompareTag(Tags.TheraCode_TAG))
            {
                _playerUI._collect.gameObject.SetActive(true);
                _playerUI._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Collect Theracode";
                _playerUI._collect.onClick.RemoveAllListeners();
                _playerUI._collect.onClick.AddListener(() => { CollectCode(other.gameObject); });
            }
        }
        #endregion

        #region Week-4
        if (_map == MAP.WEEK4)
        {
            if (other.gameObject.TryGetComponent<GeneralArtifactsManager>(out GeneralArtifactsManager artifactManager))
            {
                if (artifactManager != null && artifactManager.artifact == GeneralArtifactsManager.Artifact.MeronNftPartOne)
                {
                    PvPPlayerUI.instance._collect.onClick.RemoveAllListeners();
                    PvPPlayerUI.instance._collect.gameObject.SetActive(true);
                    PvPPlayerUI.instance._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Collect Meron nft part";
                    PvPPlayerUI.instance._collect.onClick.AddListener(() =>
                    {
                        _meronNftOne = true;
                        PutInPocket(other.gameObject);
                        PvPPlayerUI.instance._collect.gameObject.SetActive(false);
                        Destroy(other.gameObject);
                    });
                   
                }
                else if (artifactManager != null && artifactManager.artifact == GeneralArtifactsManager.Artifact.MeronNft)
                {
                    PvPPlayerUI.instance._collect.onClick.RemoveAllListeners();
                    PvPPlayerUI.instance._collect.gameObject.SetActive(true);
                    PvPPlayerUI.instance._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Collect Meron nft";
                    PvPPlayerUI.instance._collect.onClick.AddListener(() =>
                    {
                        LeanTween.scale(other.gameObject, new Vector3(0, 0, 0), 1f);
                        Destroy(other.gameObject, 1f);
                        _collectedAssembledMeronNft = true;
                        PvPPlayerUI.instance._collect.gameObject.SetActive(false);
                    });
              
                }
                else if (artifactManager != null && artifactManager.artifact == GeneralArtifactsManager.Artifact.GlimmerArtifact)
                {
                    PvPPlayerUI.instance._collect.onClick.RemoveAllListeners();
                    PvPPlayerUI.instance._collect.gameObject.SetActive(true);
                    PvPPlayerUI.instance._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Collect Glimmer";
                    PvPPlayerUI.instance._collect.onClick.AddListener(() =>
                    {
                        CollectArtifact(artifactManager, null);
                    });
                    _playerTimelineManager.CollectGlimmerArtifact(artifactManager.glimmerType);
                    other.enabled = false;
                }
                else if (artifactManager.artifact == GeneralArtifactsManager.Artifact.PowerPotion)
                {
                    _hasPowerPotion = true;
                    _weaponShadowAura.SetActive(true);
                    Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z);
                    LeanTween.scale(other.gameObject, new Vector3(0, 0, 0), 1f);
                    LeanTween.move(other.gameObject, pos, 1f);
                }
                else if (artifactManager.artifact == GeneralArtifactsManager.Artifact.HealthPotion)
                {
                    Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z);
                    LeanTween.scale(other.gameObject, new Vector3(0, 0, 0), 1f);
                    LeanTween.move(other.gameObject, pos, 1f);
                    StartCoroutine(IncreaseHealth(other.gameObject));
                }
            }
            else if (other.gameObject.TryGetComponent<ManualTag>(out ManualTag tag))
            {
                if (tag != null && tag._tag == Tag.MeronNftAssemble)
                {
                    PvPPlayerUI.instance._collect.onClick.RemoveAllListeners();
                    PvPPlayerUI.instance._collect.gameObject.SetActive(true);
                    PvPPlayerUI.instance._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Put together all Nft";
                    PvPPlayerUI.instance._collect.onClick.AddListener(() =>
                    {
                        Assemble(tag);
                    });
                }else if (tag != null && tag._tag == Tag.ForearmsArmor)
                {
                    PvPPlayerUI.instance._collect.onClick.RemoveAllListeners();
                    PvPPlayerUI.instance._collect.gameObject.SetActive(true);
                    PvPPlayerUI.instance._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Collect armour";
                    PvPPlayerUI.instance._collect.onClick.AddListener(() =>
                    {
                        CollectArmor(tag,0);
                    });
                }
                else if (tag != null && tag._tag == Tag.ShoulderArmor)
                {
                    PvPPlayerUI.instance._collect.onClick.RemoveAllListeners();
                    PvPPlayerUI.instance._collect.gameObject.SetActive(true);
                    PvPPlayerUI.instance._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Collect armour";
                    PvPPlayerUI.instance._collect.onClick.AddListener(() =>
                    {
                        CollectArmor(tag, 1);
                    });
                }
                else if (tag != null && tag._tag == Tag.BodyArmor)
                {
                    PvPPlayerUI.instance._collect.onClick.RemoveAllListeners();
                    PvPPlayerUI.instance._collect.gameObject.SetActive(true);
                    PvPPlayerUI.instance._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Collect armour";
                    PvPPlayerUI.instance._collect.onClick.AddListener(() =>
                    {
                        CollectArmor(tag, 2);
                    });
                }
                else if (tag != null && tag._tag == Tag.Ingredients)
                {
                    PvPPlayerUI.instance._collect.onClick.RemoveAllListeners();
                    PvPPlayerUI.instance._collect.gameObject.SetActive(true);
                    PvPPlayerUI.instance._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Collect Ingredients";
                    PvPPlayerUI.instance._collect.onClick.AddListener(() =>
                    {
                        CollectIngredients(tag);
                    });
                }
                else if (tag != null && tag._tag == Tag.TheClaw)
                {

                    PvPPlayerUI.instance._collect.onClick.RemoveAllListeners();
                    PvPPlayerUI.instance._collect.gameObject.SetActive(true);
                    PvPPlayerUI.instance._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Collect Claw";
                    PvPPlayerUI.instance._collect.onClick.AddListener(() =>
                    {
                        CollectArtifact(null, tag);
                    });
                }
                else if (tag != null && tag._tag == Tag.OrbsOfKinesis)
                {
                    PvPPlayerUI.instance._collect.onClick.RemoveAllListeners();
                    PvPPlayerUI.instance._collect.gameObject.SetActive(true);
                    PvPPlayerUI.instance._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Collect Orb";
                    PvPPlayerUI.instance._collect.onClick.AddListener(() =>
                    {
                        CollectArtifact(null, tag);
                    });
                }
            }
            else if (other.gameObject.CompareTag(Tags.TheraCode_TAG))
            {
                _playerUI._collect.gameObject.SetActive(true);
                _playerUI._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Collect Theracode";
                _playerUI._collect.onClick.RemoveAllListeners();
                _playerUI._collect.onClick.AddListener(() => { CollectCode(other.gameObject); });
            }
            else if(other.gameObject.TryGetComponent<WeekFourTriggerManager>(out WeekFourTriggerManager triggerManager))
            {
                if(triggerManager != null && triggerManager.mapPoint == WeekFourTriggerManager.MapPoint.GatePlatform) {
                    PvPPlayerUI.instance._collect.onClick.RemoveAllListeners();
                    PvPPlayerUI.instance._collect.gameObject.SetActive(true);
                    PvPPlayerUI.instance._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Place orb and claw";
                    PvPPlayerUI.instance._collect.onClick.AddListener(() =>
                    {
                        PlaceArtifacts(triggerManager);
                    });
                }else if (triggerManager.mapPoint == WeekFourTriggerManager.MapPoint.AutoCave)
                {
                    SceneManager.instance.LoadScene("Week-4");
                    SceneManager.instance._text.gameObject.SetActive(true);
                    SceneManager.instance._text.text = "Mission Failed";
                }
            }
        }
        #endregion

        #region Week-5
        if (_map == MAP.WEEK5)
        {
            if (other.gameObject.TryGetComponent<GeneralCharacterManager>(out GeneralCharacterManager _generalCharacterManager))
            {
                if (_generalCharacterManager._characterType == GeneralCharacterManager.CharacterType.CelorianTypeThree && !_generalCharacterManager._collectedInformation)
                {
                    GetComponent<PvPPlayerUI>()._collect.onClick.RemoveAllListeners();
                    GetComponent<PvPPlayerUI>()._collect.onClick.AddListener(() =>
                    {
                        CollectInformation(other.gameObject, _generalCharacterManager);
                    });
                    GetComponent<PvPPlayerUI>()._collect.gameObject.SetActive(true);
                    GetComponent<PvPPlayerUI>()._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Collect information";
                }
            }
            else if(other.gameObject.TryGetComponent<ManualTag>(out ManualTag tag))
            {
                if(tag._tag == Tag.AsimanaLightShaftsArtifact)
                {
                    GetComponent<PvPPlayerUI>()._collect.onClick.RemoveAllListeners();
                    GetComponent<PvPPlayerUI>()._collect.onClick.AddListener(() =>
                    {
                        CollectInformation(other.gameObject);
                    });
                    GetComponent<PvPPlayerUI>()._collect.gameObject.SetActive(true);
                    GetComponent<PvPPlayerUI>()._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Collect information";
                }
                else if (tag._tag == Tag.EnterMiniGame)
                {
                    GetComponent<PvPPlayerUI>()._collect.onClick.RemoveAllListeners();
                    GetComponent<PvPPlayerUI>()._collect.onClick.AddListener(() =>
                    {
                        EnterMiniGame();
                    });
                    GetComponent<PvPPlayerUI>()._collect.gameObject.SetActive(true);
                    GetComponent<PvPPlayerUI>()._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Enter minigame";
                    GetComponent<PlayerTimelineManager>().RevealHiddenMonsters();
                }
                else if(tag._tag == Tag.MagicalDoor)
                {
                    if (_collectedMagicalKey)
                    {
                        GetComponent<PvPPlayerUI>()._collect.onClick.RemoveAllListeners();
                        GetComponent<PvPPlayerUI>()._collect.onClick.AddListener(() =>
                        {
                            UnlockMagicalDoor(other.gameObject);
                        });
                        GetComponent<PvPPlayerUI>()._collect.gameObject.SetActive(true);
                        GetComponent<PvPPlayerUI>()._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Unlock door";
                    }
                    else
                    {
                        GetComponent<PlayerTimelineManager>().FindKey();
                    }
                }
                else if (tag._tag == Tag.MirasChallenge)
                {
                    if(_mirasChallengePanel != null)
                    {
                        GetComponent<PlayerTimelineManager>().MirasChallenge(_mirasChallengePanel);
                    }
                }
                else if (tag._tag == Tag.MirasTarget)
                {
                    if (_mirasChallengePanel != null)
                    {
                        GeneralCharacterManager []generalCharacterManager = FindObjectsOfType<GeneralCharacterManager>();
                        foreach (var character in generalCharacterManager)
                        {
                            if(character._characterType == GeneralCharacterManager.CharacterType.Mira)
                            {
                                character.KaireEffect();
                                GetComponent<PlayerTimelineManager>().RepairMira();
                                other.gameObject.SetActive(false);
                            }
                        }
                    }
                }
            }
            else if (other.gameObject.TryGetComponent<GeneralArtifactsManager>(out GeneralArtifactsManager _artifact))
            {
                if(_artifact.artifact == GeneralArtifactsManager.Artifact.MagicalKey)
                {
                    _playerUI._collect.gameObject.SetActive(true);
                    _playerUI._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Collect MagicalKey";
                    _playerUI._collect.onClick.RemoveAllListeners();
                    _playerUI._collect.onClick.AddListener(() => {
                        _collectedMagicalKey = true;
                        LeanTween.scale(other.gameObject, new Vector3(0, 0, 0), 1f);
                        _playerUI._collect.gameObject.SetActive(false);
                    });

                }
                else if (_artifact.artifact == GeneralArtifactsManager.Artifact.TheraCode && _collectedTherCode < 5)
                {
                    _playerUI._collect.gameObject.SetActive(true);
                    _playerUI._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Collect Theracode";
                    _playerUI._collect.onClick.RemoveAllListeners();
                    _playerUI._collect.onClick.AddListener(() => { CollectCode(other.gameObject); });
                }
            }
           
        }
        #endregion
    }



    private void OnCollisionEnter(Collision collision)
    {
        //Teleport on Fall

        if (collision.gameObject.CompareTag(Tags.DeadZone))
        {
            StartCoroutine(TelePort());
        }

        #region Week 1
        if (_map == MAP.WEEK1)
        {
            if (collision.gameObject.CompareTag(Tags.TheraCode_TAG) && _collectedTherCode < 5)
            {
                _playerUI._collect.gameObject.SetActive(true);
                _playerUI._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Collect Theracode";
                _playerUI._collect.onClick.RemoveAllListeners();
                _playerUI._collect.onClick.AddListener(() => { CollectCode
                    (collision.gameObject); });
            }
        }
        #endregion
    }

    private void OnCollisionExit(Collision collision)
    {
        #region Week 1
        if (_map == MAP.WEEK1)
        {
            if (collision.gameObject.CompareTag(Tags.TheraCode_TAG))
            {
                _playerUI._collect.gameObject.SetActive(false);
            }
        }
        #endregion
    }

    private void OnTriggerExit(Collider other)
    {
        if (_map == MAP.WEEK1)
        {
            if (other.gameObject.CompareTag("WallBreakSpell")) _playerUI.gameObject.SetActive(true);

            if (other.gameObject.CompareTag(Tags.TheraCode_TAG)) _playerUI._collect.gameObject.SetActive(false);

            if (other.gameObject.CompareTag(MapPoints.AlkemannaHotspotSwitch))
            {
                _playerUI._switchOnAlkemmana.gameObject.SetActive(false);
            }
        }
        else if (_map == MAP.WEEK2)
        {

        }
        else if (_map == MAP.WEEK3)
        {
            if (other.gameObject.CompareTag(Tags.TheraCode_TAG))
            {
                _playerUI._collect.gameObject.SetActive(false);
            }
        }
        else if (_map == MAP.WEEK4)
        {
            if (other.gameObject.CompareTag(Tags.TheraCode_TAG))
            {
                _playerUI._collect.gameObject.SetActive(false);
            }
            else if (other.gameObject.TryGetComponent<ManualTag>(out ManualTag tag))
            {
                if (tag != null && tag._tag == Tag.MeronNftAssemble)
                {
                    PvPPlayerUI.instance._collect.gameObject.SetActive(false);
                    PvPPlayerUI.instance._collect.onClick.RemoveAllListeners();
                }
                else if (tag != null && tag._tag == Tag.ForearmsArmor)
                {
                    PvPPlayerUI.instance._collect.gameObject.SetActive(false);
                    PvPPlayerUI.instance._collect.onClick.RemoveAllListeners();
                }
                else if (tag != null && tag._tag == Tag.ShoulderArmor)
                {
                    PvPPlayerUI.instance._collect.gameObject.SetActive(false);
                    PvPPlayerUI.instance._collect.onClick.RemoveAllListeners();
                }
                else if (tag != null && tag._tag == Tag.BodyArmor)
                {
                    PvPPlayerUI.instance._collect.gameObject.SetActive(false);
                    PvPPlayerUI.instance._collect.onClick.RemoveAllListeners();
                }
            }
        }
        else if (_map == MAP.WEEK5)
        {
            if (other.gameObject.TryGetComponent<GeneralCharacterManager>(out GeneralCharacterManager _generalCharacterManager))
            {
                if (_generalCharacterManager._characterType == GeneralCharacterManager.CharacterType.CelorianTypeThree)
                {
                    GetComponent<PvPPlayerUI>()._collect.gameObject.SetActive(false);
                }
            }
        }
    }

    #endregion
    private void CheckPointManager(CheckpointTag checkpointTag)
    {
        Debug.LogError($"Current Area Name {currentAreaName} checkpoint name {checkpointTag.CheckPointInfo.checkPointName}");
        if (currentAreaName != checkpointTag.CheckPointInfo.checkPointName)
        {
            _toast._text.text = "Area : " + checkpointTag.CheckPointInfo.checkPointName;
            _toast.ShowToast();
            currentAreaName = checkpointTag.CheckPointInfo.checkPointName;
        }
        HandleOnChainSync(checkpointTag);
    }

    private void HandleOnChainSync(CheckpointTag checkpointTag)
    {
        if (_map == MAP.WEEK1)
        {
            //if not visited alkemia chamber push the first checkpoint and return no need to sync any checkpoint
            if (!_beenInAlkemiaChamber
            && !_beenToRecoletteMirror
            && !_receivedTheraBox
            && !_receivedAsimanaCodex
            && !_mainFrameEnabled
            && checkpointTag.checkPoint == CheckPoint.origoCorridor
            && !visitedAreas.Contains(currentAreaName))
             {
                 Debug.LogError("Visited area  ");
                 visitedAreas.Push(currentAreaName);
                 Sync(checkpointTag.CheckPointInfo.index);
                 return;
             }
             if (_beenInAlkemiaChamber
                 && checkpointTag.checkPoint == CheckPoint.alkemiachamber
                 && !visitedAreas.Contains(currentAreaName))
             {
                 Debug.LogError("Visited area  ");
                 visitedAreas.Push(currentAreaName);
                 Sync(checkpointTag.CheckPointInfo.index);
                 return;
             }
             if (_beenInAlkemiaChamber 
                 && _beenToRecoletteMirror
                 && checkpointTag.checkPoint == CheckPoint.thewaterwaysofalkem
                 && !visitedAreas.Contains(currentAreaName))
             {
                 Debug.LogError("Visited area  ");
                 visitedAreas.Push(currentAreaName);
                 Sync(checkpointTag.CheckPointInfo.index);
                 return;
             }
             if (_beenInAlkemiaChamber
                 && _beenToRecoletteMirror 
                 && _receivedTheraBox
                 && checkpointTag.checkPoint == CheckPoint.thetheracodetrap
                 && !visitedAreas.Contains(currentAreaName))
             {
                 Debug.LogError("Visited area  ");
                 visitedAreas.Push(currentAreaName);
                 Sync(checkpointTag.CheckPointInfo.index);
                 return;
             }
             if (_beenInAlkemiaChamber 
                 && _beenToRecoletteMirror 
                 && _receivedTheraBox 
                 && _placedTheraCode
                 && checkpointTag.checkPoint == CheckPoint.theriddlingstairs
                 && !visitedAreas.Contains(currentAreaName))
             {
                 Debug.LogError("Visited area  ");
                 visitedAreas.Push(currentAreaName);
                 Sync(checkpointTag.CheckPointInfo.index);
                 return;
             }
             if (_beenInAlkemiaChamber 
                 && _beenToRecoletteMirror 
                 && _receivedTheraBox 
                 && _receivedAsimanaCodex
                 && checkpointTag.checkPoint == CheckPoint.theasimanamaze
                 && !visitedAreas.Contains(currentAreaName))
             {
                 Debug.LogError("Visited area  ");
                 visitedAreas.Push(currentAreaName);
                 Sync(checkpointTag.CheckPointInfo.index);
                 return;
             }
             if (_beenInAlkemiaChamber 
                 && _beenToRecoletteMirror 
                 && _receivedTheraBox 
                 && _receivedAsimanaCodex 
                 && _mainFrameEnabled
                 && checkpointTag.checkPoint == CheckPoint.thetowerofeventualities
                 && !visitedAreas.Contains(currentAreaName))
             {
                 Debug.LogError("Visited area  ");
                 visitedAreas.Push(currentAreaName);
                 Sync(checkpointTag.CheckPointInfo.index);
                 return;
             }
             if (_beenInAlkemiaChamber 
                 && _beenToRecoletteMirror 
                 && _receivedTheraBox 
                 && _receivedAsimanaCodex 
                 && _mainFrameEnabled
                 && checkpointTag.checkPoint == CheckPoint.TowerofEventualitiesTerathian
                 && !visitedAreas.Contains(currentAreaName))
             {
                 Debug.LogError("Visited area  ");
                 visitedAreas.Push(currentAreaName);
                 Sync(checkpointTag.CheckPointInfo.index);
                 return;
             }
        }
        else if (_map == MAP.WEEK2)
        {
            if (!_collectedStarDevice
                && checkpointTag.checkPoint == CheckPoint.TowerofEventualitiesTerathian
                && !visitedAreas.Contains(currentAreaName))
            {
                Debug.LogError("Visited area  ");
                visitedAreas.Push(currentAreaName);
                Sync(checkpointTag.CheckPointInfo.index);
                return;
            }
            if (_collectedStarDevice
                && checkpointTag.checkPoint == CheckPoint.TheTerathianAbyss
                && !visitedAreas.Contains(currentAreaName))
            {
                Debug.LogError("Visited area  ");
                visitedAreas.Push(currentAreaName);
                Sync(checkpointTag.CheckPointInfo.index);
                return;
            }
            if (_collectedStarDevice
                &&_inOrbsOfKinesis
                && checkpointTag.checkPoint == CheckPoint.ManaMussenden
                && !visitedAreas.Contains(currentAreaName))
            {
                Debug.LogError("Visited area  ");
                visitedAreas.Push(currentAreaName);
                Sync(checkpointTag.CheckPointInfo.index);
                return;
            }
            if (_collectedStarDevice
                && _inOrbsOfKinesis
                && _collectedMiniGameOrbs
                && checkpointTag.checkPoint == CheckPoint.TheTerathianAbyss
                && !visitedAreas.Contains(currentAreaName))
            {
                Debug.LogError("Visited area  ");
                visitedAreas.Push(currentAreaName);
                Sync(checkpointTag.CheckPointInfo.index);
                return;
            }
            if (_collectedStarDevice
                && _inOrbsOfKinesis
                && _collectedMiniGameOrbs
                && _collectedOrbs == 2
                && checkpointTag.checkPoint == CheckPoint.ManaWalkways
                && !visitedAreas.Contains(currentAreaName))
            {
                Debug.LogError("Visited area  ");
                visitedAreas.Push(currentAreaName);
                Sync(checkpointTag.CheckPointInfo.index);
                return;
            }
            if (_collectedStarDevice
                && _inOrbsOfKinesis
                && _collectedMiniGameOrbs
                && _collectedOrbs == 3
                && checkpointTag.checkPoint == CheckPoint.TheTerathianAbyss
                && !visitedAreas.Contains(currentAreaName))
            {
                Debug.LogError("Visited area  ");
                visitedAreas.Push(currentAreaName);
                Sync(checkpointTag.CheckPointInfo.index);
                return;
            }
            if (_collectedStarDevice
                && _inOrbsOfKinesis
                && _collectedMiniGameOrbs
                && _collectedOrbs == 3
                && checkpointTag.checkPoint == CheckPoint.ManaMacabreValley
                && !visitedAreas.Contains(currentAreaName))
            {
                Debug.LogError("Visited area  ");
                visitedAreas.Push(currentAreaName);
                Sync(checkpointTag.CheckPointInfo.index);
                return;
            }
            if (_collectedStarDevice
                && _inOrbsOfKinesis
                && _collectedMiniGameOrbs
                && _collectedOrbs == 3
                && _collectedLastOrb
                && checkpointTag.checkPoint == CheckPoint.TheTerathianEpimanaPowerPoint
                && !visitedAreas.Contains(currentAreaName))
            {
                Debug.LogError("Visited area  ");
                visitedAreas.Push(currentAreaName);
                Sync(checkpointTag.CheckPointInfo.index);
                return;
            }
            if (_collectedStarDevice
                && _inOrbsOfKinesis
                && _collectedMiniGameOrbs
                && _collectedOrbs == 3
                && _collectedLastOrb
                && checkpointTag.checkPoint == CheckPoint.TheDispellingLabrinyth
                && !visitedAreas.Contains(currentAreaName))
            {
                Debug.LogError("Visited area  ");
                visitedAreas.Push(currentAreaName);
                Sync(checkpointTag.CheckPointInfo.index);
                return;
            }
            if (_collectedStarDevice
                && _inOrbsOfKinesis
                && _collectedMiniGameOrbs
                && _collectedOrbs == 3
                && _collectedLastOrb
                && _fightedKaire
                && checkpointTag.checkPoint == CheckPoint.TheTerathianChasm
                && !visitedAreas.Contains(currentAreaName))
            {
                Debug.LogError("Visited area  ");
                visitedAreas.Push(currentAreaName);
                Sync(checkpointTag.CheckPointInfo.index);
                return;
            }
        }
    }

    private void Sync(int checkPoint)
    {
        _completedMissionNo = checkPoint;
        //For prasanta to write on chain sync code
    }

    private void PlaceArtifacts(WeekFourTriggerManager _triggerManager)
    {
        if (_collectedClawCount >= 1 && _collectedOrbs >= 1)
        {
            _triggerManager.PlacedArtifacts();
            PvPPlayerUI.instance._collect.gameObject.SetActive(false);
        }
        else
        {
            _toast._text.text = "Need artifacts to place";
            _toast.ShowToast();
            PvPPlayerUI.instance._collect.gameObject.SetActive(false);
        }
    }
    private void CollectIngredients(ManualTag tag)
    {
        PvPPlayerUI.instance._collect.gameObject.SetActive(false);
        _magicScrolls += 3;
        _collectedTherCode += 2;
        _collectedAdacode += 1;
        GrimoireManager grimoire = GetComponent<GrimoireManager>();
        grimoire._magicScrolls.text = _magicScrolls.ToString();
        _theraCount.text = _collectedTherCode.ToString();
        tag.gameObject.SetActive(false);
    }

    private void CollectArmor(ManualTag tag, int _armorType)
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z);
        //forearm 
        if (_armorType == 0)
        {
            LeanTween.scale(tag.gameObject, new Vector3(0, 0, 0), 1f);
            LeanTween.move(tag.gameObject, pos, 1f);
            Destroy(tag.gameObject, 1);
            _forearms[_forearmsArmor].alpha = 1;
            _forearmsArmor++;
            PvPPlayerUI.instance._collect.gameObject.SetActive(false);
        }
        //shoulder
        else if(_armorType == 1)
        {
            LeanTween.scale(tag.gameObject, new Vector3(0, 0, 0), 1f);
            LeanTween.move(tag.gameObject, pos, 1f);
            Destroy(tag.gameObject, 1);
            _ShoulderArmor++;
            _shoulder.alpha = 1;
            PvPPlayerUI.instance._collect.gameObject.SetActive(false);
        }
        //Body
        else if (_armorType == 2)
        {
            LeanTween.scale(tag.gameObject, new Vector3(0, 0, 0), 1f);
            LeanTween.move(tag.gameObject, pos, 1f);
            Destroy(tag.gameObject, 1);
            _bodyArmor++;
            _body.alpha = 1;
            PvPPlayerUI.instance._collect.gameObject.SetActive(false);
        }
    }

    private void CollectArtifact(GeneralArtifactsManager tag = null, ManualTag manualTag = null)
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z);
        if(tag != null)
        {
            if(tag.artifact == GeneralArtifactsManager.Artifact.GlimmerArtifact)
            {
                LeanTween.scale(tag.gameObject, new Vector3(0, 0, 0), 1f);
                LeanTween.move(tag.gameObject, pos, 1f);
                Destroy(tag.gameObject, 1);
                _playerTimelineManager._grimoire._glimmer.interactable = true;
                _playerTimelineManager._grimoire._glimmer.onClick.AddListener(() =>
                {
                    GiveGlimmerFunctionality(tag);
                });
            }
        }
        else if (tag != null && manualTag != null)
        { 
            if (manualTag._tag != Tag.OrbsOfKinesis && manualTag._tag != Tag.TheClaw)
            {
                LeanTween.scale(tag.gameObject, new Vector3(0, 0, 0), 1f);
                LeanTween.move(tag.gameObject, pos, 1f);
                Destroy(tag.gameObject, 1);
            }
        }
        else if(manualTag._tag == Tag.OrbsOfKinesis)
        {
            _collectedOrbs++;
            LeanTween.scale(manualTag.gameObject, new Vector3(0, 0, 0), 1f);
            LeanTween.move(manualTag.gameObject, pos, 1f);
            Destroy(manualTag.gameObject, 1);
        }else if(manualTag._tag == Tag.TheClaw)
        {
            _collectedClawCount++;
            LeanTween.scale(manualTag.gameObject, new Vector3(0, 0, 0), 1f);
            LeanTween.move(manualTag.gameObject, pos, 1f);
            Destroy(manualTag.gameObject, 1);
        }
        else if (manualTag != null)
        {
            LeanTween.scale(manualTag.gameObject, new Vector3(0, 0, 0), 1f);
            LeanTween.move(manualTag.gameObject, pos, 1f);
            Destroy(manualTag.gameObject, 1);
        }
        PvPPlayerUI.instance._collect.gameObject.SetActive(false);
    }

    private void GiveGlimmerFunctionality(GeneralArtifactsManager artifact)
    {
        //0 -- Reveals hidden chests with loot - yellow circle.
        //1 -- Reveals hidden potions - red circle.
        //2 -- Reveals hidden artifacts - blue circle.
        //3 -- Reveals hidden pathways through walls - purple circle.
        //4 -- Reveals hidden information - green circle.

        GameManager _gameManager = GameManager.instance;

        if (artifact.glimmerType == 0)
        {
            _gameManager._chestSpawner.SetActive(true);
        }
        else if(artifact.glimmerType == 1)
        {
            GameObject _healthPotionInstance = null;
            GameObject _powerPotionInstance = null;
            int randIndex = UnityEngine.Random.Range(0, _gameManager._healthPotionsSpts.Length);
            _healthPotionInstance = Instantiate(_gameManager._healthPotions, _gameManager._healthPotionsSpts[randIndex].transform.position, Quaternion.identity);
            for(int i =0; i < 10; i++)
            {
                int tempPrevIndex = randIndex;
                randIndex = UnityEngine.Random.Range(0, _gameManager._healthPotionsSpts.Length);
                if(tempPrevIndex != randIndex)
                {
                    _powerPotionInstance = Instantiate(_gameManager._powerPotions, _gameManager._healthPotionsSpts[randIndex].transform.position, Quaternion.identity);
                    Debug.LogError("Called");
                    break;
                }
            }
            
            
        
            GameObject _healthPotionPointerInstance = Instantiate(_gameManager._healthPotionPointer);
            GameObject _powerPotionPointerInstance = Instantiate(_gameManager._powerPotionPointer);

            _healthPotionPointerInstance.GetComponent<PlayerMinimapPointer>().target = _healthPotionInstance.transform;
            _powerPotionPointerInstance.GetComponent<PlayerMinimapPointer>().target = _powerPotionInstance.transform;
        }
        else if (artifact.glimmerType == 2)
        {
            ManualTag []_manualTags = FindObjectsOfType<ManualTag>();
            GameObject _clawPointer = Instantiate(_gameManager.clawPointer);
            GameObject _orbPointer = Instantiate(_gameManager.orbPointer);
            foreach (ManualTag manualTag in _manualTags)
            {
                if(manualTag._tag == Tag.TheClaw)
                {
                    _clawPointer.GetComponent<PlayerMinimapPointer>().target = manualTag.transform;
                }else if(manualTag._tag == Tag.OrbsOfKinesis)
                {
                    _orbPointer.GetComponent<PlayerMinimapPointer>().target = manualTag.transform;
                }
            }
        }
        else if (artifact.glimmerType == 3)
        {
            _gameManager._hiidenPath.SetActive(true);
        }
        else if (artifact.glimmerType == 4)
        {

        }
        _playerTimelineManager._grimoire.DisablePanel();
        _playerTimelineManager._grimoire._glimmer.interactable = false;
    }

    private void UnlockMagicalDoor(GameObject _obj)
    {
        foreach(var key in _obj.GetComponent<WeekfiveTriggerManager>().keys){
            key.SetActive(true);
        }
        _obj.GetComponent<WeekfiveTriggerManager>()._unlockAnimation.SetTrigger("Unlock");
        _obj.SetActive(false);
        GetComponent<PvPPlayerUI>()._collect.gameObject.SetActive(false);
        _unlockedMagicalDoor = true;
        UnlockedDoor?.Invoke();
    }

    private void EnterMiniGame()
    {
        GetComponent<PvPPlayerUI>()._collect.gameObject.SetActive(false);
        _weaponPowerUpPanel.gameObject.SetActive(true);
        GetComponent<PvPPlayerUI>()._collect.gameObject.SetActive(true);
        GetComponent<PvPPlayerUI>()._collect.onClick.RemoveAllListeners();
        GetComponent<PvPPlayerUI>()._collect.onClick.AddListener(() =>
        {
            ExitMiniGame();
        });
        GetComponent<PvPPlayerUI>()._collect.gameObject.SetActive(true);
        GetComponent<PvPPlayerUI>()._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Exit minigame";
        pause = true;
    }

    private void ExitMiniGame()
    {
        if (_llumanaMagic.filledLlumanaMagic && _malumanaMagic.filledMalumanaMagic)
        {
            _weaponPowerUpPanel.gameObject.SetActive(false);
            GetComponent<PvPPlayerUI>()._collect.gameObject.SetActive(false);
            _llumanaandMalumanaMagic.SetActive(true);
            pause = false;
        }
        else
        {
            _toast._text.text = "Need more magic";
            _toast.ShowToast();
        }
    }

    private void Assemble( ManualTag tag)
    {
        tag.GetComponent<WeekFourTriggerManager>()._putTogetherAnimator.SetTrigger("Play");
        tag.gameObject.SetActive(false);
        PvPPlayerUI.instance._collect.gameObject.SetActive(false);
    }
    private void CollectInformation(GameObject _obj, GeneralCharacterManager _characterManaager = null)
    {
        if(_characterManaager != null)
        {
            _characterManaager.TurnToStone();
            _characterManaager._informationPanel.SetActive(false);
            PvPPlayerUI.instance._collect.gameObject.SetActive(false);
            _characterManaager._collectedInformation = true;
        }
        else
        {
            PvPPlayerUI.instance._collect.gameObject.SetActive(false);
            _obj.SetActive(false);
        }
    }

    private IEnumerator TelePort()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        yield return new WaitForSeconds(0.5f);
        transform.position = PlayerStage.instance.transform.position;
        yield return new WaitForSeconds(0.5f);
        GetComponent<Rigidbody>().isKinematic = false;
    }
    
    private void Blurred()
    {
        LeanTween.alphaCanvas(_darkPanel, 1, 3f);
    }

    private void LoadScen()
    {
        SceneManager.instance.LoadScene("Week-3");
        SceneManager.instance._text.gameObject.SetActive(true);
        SceneManager.instance._text.text = "Level Completed";
    }

    private void CollectOrbTwo(GameObject otherGameObject)
    {
        otherGameObject.GetComponent<BoxCollider>().enabled = false;
        LeanTween.scale(otherGameObject, new Vector3(0, 0, 0), 2f);
        Destroy(otherGameObject, 2f);
        _collectedOrbs++;
        PvPPlayerUI.instance._collect.gameObject.SetActive(false);
    }

    private void CollectOrbMiniGame(GameObject otherGameObject)
    {
        OrbsOfKinesis _orbs = otherGameObject.GetComponent<OrbsOfKinesis>();
        _collectedOrbs++;
        _collectedMiniGameOrbs = true;
        _orbs.rock.SetActive(true);
        _orbs.orb.SetActive(false);
        otherGameObject.GetComponent<SphereCollider>().enabled = false;
        PvPPlayerUI.instance._collect.gameObject.SetActive(false);
    }

    private void CollectStarDevice(GameObject other)
    {
        Destroy(other, 0.1f);
        GetComponent<GrimoireManager>().EnableMapViewfunc();
        PvPPlayerUI.instance._collect.gameObject.SetActive(false);
    }


    private IEnumerator IncreaseHealth(GameObject _obj)
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z);
        LeanTween.scale(_obj.gameObject, new Vector3(0, 0, 0), 1f);
        LeanTween.move(_obj.gameObject, pos, 1f);
        Destroy(_obj, 2f);
        while (GetComponent<IntrantPlayerHealthManager>().currentHealth < 100)
        {
            yield return new WaitForSeconds(0.1f);
            GetComponent<IntrantPlayerHealthManager>().Increasehealth(1);
        }
    }

    private void CollectCode(GameObject _code)
    {
        //Collect thera stray code 
        if (!_code) return; // Check if the object is null before destroying

        Destroy(_code);
        _collectedTherCode++;
        _theraCount.text = _collectedTherCode.ToString();
        _playerUI._collect.gameObject.SetActive(false);
        if(_map == MAP.WEEK3)
        {
            _theraCount.text = _collectedTherCode.ToString();
        }
    }

    public void ChangeMat(MeshRenderer _mesh, Material _mat)
    {
        _mesh.material = _mat;
        _playerUI._switchOnAlkemmana.gameObject.SetActive(false);
        _switchedOnCount++;
    }


    #region Pause and Resume

    public void Resume()
    {
        //Time.timeScale = 1f;
        pause = false;
        Debug.LogError("Resuming game..");
        _grimoirPanel.SetActive(false);
    }

    public void Pause()
    {
        //Time.timeScale = 0f;
        pause = true;
        Debug.LogError("Resuming game..");
        _grimoirPanel.SetActive(true);
    }

    #endregion

    private void Disable()
    {
        //Turnoff break button in start
        _playerUI._wallBreakbtn.gameObject.SetActive(false);
    }

    public virtual void ControlAnimatorRootMotion()
    {
        if (!enabled) return;

        if (inputSmooth == Vector3.zero)
        {
            transform.position = animator.rootPosition;
            transform.rotation = animator.rootRotation;
        }

        if (useRootMotion)
            MoveCharacter(moveDirection);
    }

    public virtual void ControlLocomotionType()
    {
        if (pause) return;
        if (lockMovement) return;

        if ((locomotionType.Equals(LocomotionType.FreeWithStrafe) && !isStrafing) ||
            locomotionType.Equals(LocomotionType.OnlyFree))
        {
            SetControllerMoveSpeed(freeSpeed);
            SetAnimatorMoveSpeed(freeSpeed);
        }
        else if (locomotionType.Equals(LocomotionType.OnlyStrafe) ||
                 (locomotionType.Equals(LocomotionType.FreeWithStrafe) && isStrafing))
        {
            isStrafing = true;
            SetControllerMoveSpeed(strafeSpeed);
            SetAnimatorMoveSpeed(strafeSpeed);
        }

        if (!useRootMotion)
            MoveCharacter(moveDirection);
    }

    public virtual void ControlRotationType()
    {
        if (pause) return;
        if (lockRotation) return;

        var validInput = input != Vector3.zero ||
                         (isStrafing ? strafeSpeed.rotateWithCamera : freeSpeed.rotateWithCamera);

        if (validInput)
        {
            // calculate input smooth
            inputSmooth = Vector3.Lerp(inputSmooth, input,
                (isStrafing ? strafeSpeed.movementSmooth : freeSpeed.movementSmooth) * Time.deltaTime);

            var dir = ((isStrafing && (!isSprinting || sprintOnlyFree == false)) ||
                       (freeSpeed.rotateWithCamera && input == Vector3.zero)) && rotateTarget
                ? rotateTarget.forward
                : moveDirection;
            RotateToDirection(dir);
        }
    }

    public virtual void UpdateMoveDirection(Transform referenceTransform = null)
    {
        if (pause) return;
        if (input.magnitude <= 0.01)
        {
            moveDirection = Vector3.Lerp(moveDirection, Vector3.zero,
                (isStrafing ? strafeSpeed.movementSmooth : freeSpeed.movementSmooth) * Time.deltaTime);
            return;
        }

        if (referenceTransform && !rotateByWorld)
        {
            //get the right-facing direction of the referenceTransform
            var right = referenceTransform.right;
            right.y = 0;
            //get the forward direction relative to referenceTransform Right
            var forward = Quaternion.AngleAxis(-90, Vector3.up) * right;
            // determine the direction the player will face based on input and the referenceTransform's right and forward directions
            moveDirection = inputSmooth.x * right + inputSmooth.z * forward;
        }
        else
        {
            moveDirection = new Vector3(inputSmooth.x, 0, inputSmooth.z);
        }
    }

    public virtual void Sprint(bool value)
    {
        if (pause) return;

        if (!canSprint)
        {
            isSprinting = canSprint;
            return;
        }

        var sprintConditions = input.sqrMagnitude > 0.1f && isGrounded &&
                               !(isStrafing && !strafeSpeed.walkByDefault && (horizontalSpeed >= 0.5 ||
                                                                              horizontalSpeed <= -0.5 ||
                                                                              verticalSpeed <= 0.1f));

        if (value && sprintConditions)
        {
            if (input.sqrMagnitude > 0.1f)
            {
                if (isGrounded && useContinuousSprint)
                    isSprinting = !isSprinting;
                else if (!isSprinting) isSprinting = true;
            }
            else if (!useContinuousSprint && isSprinting)
            {
                isSprinting = false;
            }
        }
        else if (isSprinting)
        {
            isSprinting = false;
        }
    }

    public virtual void Strafe()
    {
        if (pause) return;
        isStrafing = !isStrafing;
    }

    public virtual void Jump()
    {
        if (pause) return;
        // trigger jump behaviour
        jumpCounter = jumpTimer;
        isJumping = true;

        // trigger jump animations
        if (input.sqrMagnitude < 0.1f)
            animator.CrossFadeInFixedTime("Jump", 0.1f);
        else
            animator.CrossFadeInFixedTime("JumpMove", .2f);
    }

    public virtual void AttackOne()
    {
        //DisableButton
        if (!_playerUI._firstAttack.interactable)
            return; // Check if the button is not on cooldown

        if (_playerUI._firstAttack.interactable) // Check if the button is not on cooldown
        {
            // Set the button on cooldown
            _playerUI._firstAttack.interactable = false;
            _attackOneCooldown.gameObject.SetActive(true);

            var cooldownDuration = 4f; // Cooldown time in seconds
            var cooldownTimer = cooldownDuration;

            StartCoroutine(StartCooldown(cooldownDuration,
                _attackOneCooldown,
                _playerUI._firstAttack,
                _attackOneOnCooldown));
        }
        if (_weaponShadowAura.activeSelf)
        {
            _hammer.damageToDeal = UnityEngine.Random.Range(195, 205);
        }
        else if (_hasPowerPotion)
        {
            _hammer.damageToDeal = UnityEngine.Random.Range(35, 50);
        }
        else
        {
            _hammer.damageToDeal = UnityEngine.Random.Range(23,27);
        }
       

        if (pause) return;
        if (isAttacking) return;
        animator.SetBool(IsAttackingOneHash, true);
        StartCoroutine(InitiateAttacKOneSliceVfx(1, 2.05f));
        StartCoroutine(InitiateAttacKOneWeaponCollider(1, 3.15f));
        StartCoroutine(DisableAnimation(IsAttackingOneHash, 2.05f));
    }

    private IEnumerator InitiateAttacKOneSliceVfx(int index, float delay)
    {
        if (isAttacking) yield return null;

        isAttacking = true;
        yield return new WaitForSeconds(1f);
        _powerUpAura.SetActive(true);
        _attackOnefirstSlice.SetActive(true);
        yield return new WaitForSeconds(1f);
        _attackOnefirstSlice.SetActive(false);
        _attackOneSecondSlice.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        _attackOneSecondSlice.SetActive(false);
        _attackOneThirdSlice.SetActive(true);
        yield return new WaitForSeconds(0.10f);
        _attackOneThirdSlice.SetActive(false);
        _powerUpAura.SetActive(false);
        isAttacking = false;
    }

    private IEnumerator InitiateAttacKOneWeaponCollider(int index, float delay)
    {
        if (!isAttacking) yield return null;

        if (index == 1 && isAttacking)
        {
            yield return new WaitForSeconds(delay * 0.3f);
            _hammerCollider.enabled = true;
            yield return new WaitForSeconds(delay * 0.7f);
            _hammerCollider.enabled = false;
        }
        else if (index == 2 && isAttacking)
        {
            yield return new WaitForSeconds(delay * 0.4f);
            _hammerCollider.enabled = true;
            yield return new WaitForSeconds(delay * 0.6f);
            _hammerCollider.enabled = false;
        }
        else
        {
            yield return null;
        }
    }

    private IEnumerator InitiateSliceVfx(int index, float delay)
    {
        if (isAttacking) yield return null;
        isAttacking = true;
        yield return new WaitForSeconds(0.8f);
        _powerUpAura.SetActive(true);
        _attacktwoFirstSlice.SetActive(true);
        yield return new WaitForSeconds(1f);
        _attacktwoFirstSlice.SetActive(false);
        _attacktwoSecondSlice.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        _attacktwoSecondSlice.SetActive(false);
        _powerUpAura.SetActive(false);
        isAttacking = false;
    }

    private IEnumerator DisableAnimation(int AttackingHash, float delay)
    {
        isAttacking = true;
        yield return new WaitForSeconds(delay);
        animator.SetBool(AttackingHash, false);
        isAttacking = false;
    }

    public virtual void AttackTwo()
    {
        if (!_playerUI._secondAttack.interactable) return; // Check if the button is not on cooldown

        _attackTwoOnCooldown = true;
        if (_playerUI._secondAttack.interactable) // Check if the button is not on cooldown
        {
            // Set the button on cooldown
            _playerUI._secondAttack.interactable = false;
            _attackTwoCooldown.gameObject.SetActive(true);

            var cooldownDuration = 30f; // Cooldown time in seconds
            StartCoroutine(StartCooldown(cooldownDuration,
                _attackTwoCooldown,
                _playerUI._secondAttack,
                _attackTwoOnCooldown));
        }
        if (_weaponShadowAura.activeSelf)
        {
            _hammer.damageToDeal = UnityEngine.Random.Range(445, 455);
        }
        else if (_hasPowerPotion)
        {
            _hammer.damageToDeal = UnityEngine.Random.Range(125, 150);
        }
        else
        {
            _hammer.damageToDeal = UnityEngine.Random.Range(95, 105);
        }
        if (pause) return;
        // Set the animator parameter for the local player
        if (isAttacking) return;
        animator.SetBool(IsAttackingTwoHash, true);
        StartCoroutine(InitiateSliceVfx(2, 2.05f));
        StartCoroutine(InitiateAttacKOneWeaponCollider(2, 2.05f));
        StartCoroutine(DisableAnimation(IsAttackingTwoHash, 2.15f));
    }

    private IEnumerator StartCooldown(float cooldownDuration,
        Image _coolDownImage,
        Button _attackButton,
        bool val)
    {
        var cooldownTimer = cooldownDuration;

        while (cooldownTimer > 0f)
        {
            // Update the cooldown timer
            cooldownTimer -= Time.deltaTime;
            _coolDownImage.fillAmount = cooldownTimer / cooldownDuration;
            yield return null; // Wait for the next frame
        }

        // Cooldown is over, allow the button to be interactable again
        _attackButton.interactable = true;
        _coolDownImage.gameObject.SetActive(false);
        val = false;
    }

    private void FixedUpdate()
    {

        if(Input.GetKeyDown(KeyCode.P))
        {
            foreach(var checkpoint in visitedAreas)
            {
                Debug.LogError($" Visited Areas: {checkpoint}");
            }
        }

        if (_map == MAP.WEEK1)
        {
            if (_checkPointFx != null)
                if (!_checkPointFx.activeSelf && _switchedOnCount > 3)
                    _checkPointFx.SetActive(true);
            //Handle Exception for Theracode
            if (_collectedTherCode >= 6)
            {
                _receivedTheraCode = true;
                _collectedTherCode = GameManager.instance._maxTheraCode + 1;
            }

            //Theracode display
            if (_theraCount.text != _collectedTherCode.ToString())
                _theraCount.text = _collectedTherCode + $"/{GameManager.instance._maxTheraCode + 1}";
        }


        if (isAttacking && _enemyController.Count > 0 && input.sqrMagnitude != 0)
        {
            MonsterMovementController closestEnemy = null;
            var closestDistance = Mathf.Infinity;

            // Loop through each enemy
            foreach (var enemy in _enemyController)
            {
                // Calculate the distance between the player and the enemy
                if(enemy == null) continue;
                var distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

                // Check if the current enemy is closer than the previously closest enemy
                if (distanceToEnemy < closestDistance)
                {
                    closestEnemy = enemy;
                    closestDistance = distanceToEnemy;
                }
            }
            if(closestEnemy != null)
            {
                Vector3 direction = (closestEnemy.transform.position - transform.position).normalized;
                direction.y = 0;
                transform.LookAt(transform.position + direction);
                //transform.LookAt(closestEnemy.gameObject.transform);
            }
        }

        if (pause)
        {
            input = Vector3.zero;
            SetAnimatorToIdle();
        }
        if (isAttacking)
        {
            input = Vector3.zero;
            SetAnimatorToIdle();
        }

        if (_map == MAP.WEEK2)
        {
            if (_collectedMiniGameOrbs)
            {
                _collectedMiniGameOrbs = false;
                PvPPlayerUI.instance._collect.gameObject.SetActive(true);
                PvPPlayerUI.instance._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Continue";
                PvPPlayerUI.instance._collect.onClick.RemoveAllListeners();
                PvPPlayerUI.instance._collect.onClick.AddListener(() =>
                {
                    StartCoroutine(TeleportPlayer());
                });
            }
        }

        if (_map == MAP.WEEK3)
        {
            if (_playerenteredSafetyEnclosure)
            {
                _playerenteredSafetyEnclosure = false;
                /*LeanTween.scale(_darkMagic, new Vector3(0, 0, 0), 3f);
                _darkMagic.SetActive(false);*/
                Vector3 scale = _earthShield.transform.localScale;
                _earthShield.gameObject.GetComponent<ForceField>().Active = true;
                LeanTween.scale(_earthShield, new Vector3(0, 0, 0), 0.1f);
                _earthShield.SetActive(true);
                LeanTween.scale(_earthShield, scale, 0.1f);
                GetComponent<PlayerTimelineManager>()._initMilutActivation = true;
                Invoke("SignalMilutActivation", 1f);
            }
        }
    }

    private void SignalMilutActivation()
    {
        PvPPlayerUI.instance._collect.gameObject.SetActive(true);
        PvPPlayerUI.instance._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Signal milut";
        PvPPlayerUI.instance._collect.onClick.RemoveAllListeners();
        PvPPlayerUI.instance._collect.onClick.AddListener(() =>
        {
            GetComponent<PlayerHenchmenconnectionManager>().BringMilut();
            PvPPlayerUI.instance._collect.gameObject.SetActive(false);
        });
    }

    private IEnumerator PlaceOrbs(Collider other)
    {
        other.gameObject.GetComponent<Animator>().SetTrigger("Place");
        _placedOrbs = true;
        PvPPlayerUI.instance._collect.gameObject.SetActive(false);
        GetComponent<GrimoireManager>()._turnToEmitter.interactable = true;
        yield return null;
    }

    IEnumerator TeleportPlayer()
    {
        PvPPlayerUI.instance._collect.gameObject.SetActive(false);
        GetComponent<Rigidbody>().isKinematic = true;
        MiniGameManager._instance.InitTPEffect();
        //isTeleporting = true;
        int length = MiniGameManager._instance.spawnPoints.Length;
        int index = UnityEngine.Random.Range(0, length);
        Debug.LogError($"TP at {index}");
        float teleportDuration = 1.0f;
        Vector3 initialPosition = transform.position;
        Transform targetSpawnPoint = PlayerStage.instance.spOne.transform;
        float elapsedTime = 0;

        while (elapsedTime < teleportDuration)
        {
            transform.position = Vector3.Lerp(initialPosition, targetSpawnPoint.position, elapsedTime / teleportDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position is exactly the target position
        transform.position = targetSpawnPoint.position;
        
        //isTeleporting = false;
        GetComponent<Rigidbody>().isKinematic = false;
    }
    
    private IEnumerator EnableShield()
    {
        isShieldActive = true;
        _shieldFxInstance.SetActive(true);

        // Wait for the cooldown duration
        yield return new WaitForSeconds(shieldCooldown);

        // Reset the shield active flag and disable the VFX
        isShieldActive = false;
        _shieldFxInstance.SetActive(false);
    }

    public void EnableDarkness(Collider other)
    {
        if (postProcessProfile != null)
        {
            if (postProcessProfile.TryGetSettings(out vignette))
            {
                // Modify the vignette intensity
                vignette.intensity.value = 1f; // Change this value as needed
            }

            other.enabled = false;
        }
    }

    public void DisableDarkness()
    {
        if (postProcessProfile != null)
        {
            if (postProcessProfile.TryGetSettings(out vignette))
            {
                // Modify the vignette intensity
                vignette.intensity.value = 0.3f; // Change this value as needed
            }
        }
    }

    internal void CollectRemainingOrb()
    {
        PvPPlayerUI.instance._collect.gameObject.SetActive(true);
        PvPPlayerUI.instance._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Collect Remaining Orb";
        PvPPlayerUI.instance._collect.onClick.RemoveAllListeners();
        PvPPlayerUI.instance._collect.onClick.AddListener(() =>
        {
            _collectedOrbs = 1;
            EmitterOrbsManager.instance.GetComponent<Animator>().SetTrigger("CollectRemainingOrb");
            PvPPlayerUI.instance._collect.gameObject.SetActive(false);
            _collectedLastOrb = true;
        });
    }

    public void Knockout()
    {
        Debug.LogError("KnockoutProcess");
        StartCoroutine(InitKnockDownProcess());
    }

    private IEnumerator InitKnockDownProcess()
    {
        Debug.LogError("KnockoutProcess");
        pause = true;
        LeanTween.alphaCanvas(_darkPanel, 1, 5f);
        _rigidbody.isKinematic = true;
        WeekFourTriggerManager []_weekFour = FindObjectsOfType<WeekFourTriggerManager>();
        animator.SetBool("Died", true);
        yield return new WaitForSeconds(5f);
        animator.SetBool("Died", false);
        foreach(var weekFour in _weekFour)
        {
            if(weekFour.mapPoint == WeekFourTriggerManager.MapPoint.DropletTarget)
            {
                transform.position = weekFour.gameObject.transform.position;
            }
        }
        _rigidbody.isKinematic = false;
        LeanTween.alphaCanvas(_darkPanel, 0, 5f).setOnComplete(DisableDarkPanel);
        GetComponent<PlayerTimelineManager>().TheraInformingAboutKnockDown();
        yield return new WaitForSeconds(11.12f);
        pause = false;
    }

    private void DisableDarkPanel()
    {
        _darkPanel.gameObject.SetActive(false);
    }

    internal void EnableWeaponShadowAura()
    {
        _weaponShadowAura.SetActive(true);
        Invoke("DisableWeapoonShadow",45f);
    }

    private void DisableWeapoonShadow()
    {
        _weaponShadowAura.SetActive(false);
    }

    internal void PlaceIngredients(GameObject other)
    {
        PvPPlayerUI.instance._collect.onClick.RemoveAllListeners();
        PvPPlayerUI.instance._collect.gameObject.SetActive(true);
        PvPPlayerUI.instance._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Place Ingredients";
        PvPPlayerUI.instance._collect.onClick.AddListener(() =>
        {
            PlaceAllIngredients(other);
        });
    }

    private void PlaceAllIngredients(GameObject other)
    {
        if (_collectedAdacode > 0 && _magicScrolls > 2 && _collectedTherCode >1)
        {
            PvPPlayerUI.instance._collect.gameObject.SetActive(false);
            GetComponent<PlayerTheraConnectionManager>().ReleaseThera();
            GetComponent<PlayerTheraConnectionManager>()._theraRef.GetComponent<Thera>()._armored = false;
            other.gameObject.SetActive(false);
        }else
        {
            PvPPlayerUI.instance._collect.gameObject.SetActive(false);
            _toast._text.text = "Need more Ingredients, Collect it from dissolving magic.";
            _toast.ShowToast();
        }
    }

    private void OnDestroy()
    {
        PlayerRangeHandler.OnPlayerEntered -= PlayerEnteredInRange;
        MonsterHealthController.OnPlayerExit -= PlayerExitRange;
    }
}