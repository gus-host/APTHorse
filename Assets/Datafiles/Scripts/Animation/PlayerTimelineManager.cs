using Cinemachine;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityEngine.UI;


public class PlayerTimelineManager : MonoBehaviour
{
    [Header(" Timeline Asset")]
    [SerializeField]
    public PlayableDirector _playableDirector;
    public IntrantThirdPersonController _player;
    public GrimoireManager _grimoire;
    private PvPPlayerUI _playerUI;
    Stack<string> _storyList = new Stack<string>();
    Stack<string> _prevStoryList = new Stack<string>();

    [SerializeField]
    private GameObject _target;
    [SerializeField]
    private Image _messageIcon;
    [SerializeField]
    private Sprite []_messageIconSprite;
    private GameObject _miraChallengePanel;
    public Button _skip;
    public CanvasGroup _damageEffect;

    [Header("Bools")]
    public bool _shownAsimana = false;
    public bool _theraRequestedRevival = false;
    public bool _kairePassed = false;
    public bool _kairePassedTimelinePlayed = false;
    public bool _theraDied = false;
    public bool _theraDeathAnimPlayed = false;
    public bool _emiterExplained = false;
    public bool _initMilutActivation = false;
    public bool _milutArrived = false;
    public bool _epimanaPoint = false;
    public bool _clawAndOrbsMergePlayed = false;
    public bool _followingKair = false;
    public bool _narrating = false;

    #region UI


    #endregion

    #region Variables

    public int _glimmerType = 0;
    public int _currentStoryNumber = 0;
    public int _previousStoryNumber = 0;

    #endregion
    private void Start()
    {
        _skip.onClick.AddListener(SkipStory);
        GameObject.Find("Replay").GetComponent<Button>().onClick.AddListener(ReplayStory);
        _player = GetComponent<IntrantThirdPersonController>();
        _grimoire = GetComponent<GrimoireManager>();
        _playerUI = GetComponent<PvPPlayerUI>();
    }

    void ReplayStory()
    {
        Debug.LogError($"Replaying story {_prevStoryList.Count}");
        if(_narrating == false)
        {
            _storyList.Clear();
            var max = _prevStoryList.Count;
            for (int i = 0 ; i < max ; i++)
            {
                Debug.LogError($"Pushing story");
                if (_prevStoryList.Count != 0)
                {
                    _storyList.Push(_prevStoryList.Pop());
                }
            }
            InitStory();
        }
        else
        {
            _player._toast._text.text = "A Story is already in progress.";
            _player._toast.ShowToast();
        }
    }

    private void SkipStory()
    {
        _playableDirector.Stop();
        if (_storyList.Count < 1)
        {
            _player.pause = false; 
            _player._isAnimating = false;
            _narrating = false;
            PostStoryFunctionality();
        }
        else
        {
            InitStory();
        }
    }

    private void FixedUpdate()
    {
        #region Week-1
        if (_player._map == MAP.WEEK1)
        {
            if(_player._switchedOnCount == 4)
            {
                _player._switchedOnCount = 5;
                StartCoroutine(ShowStory(4));
                StartCoroutine(PauseAndResume(10));
            }
            if (_player._receivedAsimanaCodex && !_shownAsimana)
            {
                _shownAsimana = true;
                StartCoroutine(ShowStory(6));
            }

            if(_player._mainFrameEnabled && !_theraRequestedRevival)
            {
                _theraRequestedRevival = true;
                _playerUI._realeaseThera.onClick.RemoveAllListeners();
                _playerUI._realeaseThera.interactable = true;
                _playerUI._realeaseThera.onClick.AddListener(() => {
                    PlayerTheraConnectionManager.instance.ReleaseThera();
                });
                StartCoroutine(ShowStory(8));
            }
            if(_kairePassed && !_kairePassedTimelinePlayed)
            {
                _kairePassedTimelinePlayed = true;
                StartCoroutine(ShowStory(9));
            }
    
            if (_theraDied && !_theraDeathAnimPlayed)
            {
                StartCoroutine(ShowStory(10));
                _theraDeathAnimPlayed = true;
            }
        }
        #endregion

        if (_player._placedOrbs && !_emiterExplained)
        {
            _emiterExplained = true;
            StartCoroutine(ShowStory(4));
        }

        #region Week-2

        if(_player._collectedLastOrb && !_followingKair)
        {
            _followingKair = true;
            StartCoroutine(PauseAndResume(10));
            StartCoroutine(ShowStory(13));
        }

        #endregion

        #region Week-3
        if (_player._map == MAP.WEEK3)
        {
            if (_initMilutActivation)
            {
                _initMilutActivation = false;
                StartCoroutine(PauseAndResume(10));
                StartCoroutine(ShowStory(5));
            }else if (_milutArrived)
            {
                _milutArrived = false;
                StartCoroutine(PauseAndResume(10));
                StartCoroutine(ShowStory(6));
            }
        }
        #endregion

        if(!_clawAndOrbsMergePlayed && _player._killedBossMonster)
        {
            _clawAndOrbsMergePlayed = true;
            StartCoroutine(PauseAndResume(10));
            StartCoroutine(ShowStory(19));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        #region Week-1
        if (_player._map == MAP.WEEK1)
        {
            if (other.gameObject.CompareTag(MapPoints.PointCExitUp))
            {
                StartCoroutine(ShowStory(12));
            }
            else if (other.gameObject.CompareTag(MapPoints.PointCExitDown))
            {
                StartCoroutine(ShowStory(13));
            }
            else if (other.gameObject.CompareTag(MapPoints.RecolettaMirror))
            {
                GetComponent<IntrantThirdPersonController>()._beenToRecoletteMirror = true;
                StartCoroutine(ShowStory(1));
                StartCoroutine(PauseAndResume(20));
                other.gameObject.GetComponent<BoxCollider>().enabled = false;
            }
            else if (other.gameObject.CompareTag(MapPoints.PointC))
            {
                StartCoroutine(ShowStory(2));
            }
            else if (other.gameObject.CompareTag(MapPoints.AlkemannaHotspot))
            {
                StartCoroutine(ShowStory(3));
                StartCoroutine(PauseAndResume(20));
            }
            else if (other.gameObject.CompareTag(MapPoints.PowerCircuits) && IntrantThirdPersonController.instance._switchedOnCount >= 4 && !IntrantThirdPersonController.instance._placedTheraCode)
            {
                _playerUI._PlaceCodes.gameObject.SetActive(true);
                _playerUI._PlaceCodes.onClick.AddListener(() => { PlaceTheraCodes(other.gameObject); });
            }
            else if (other.gameObject.CompareTag(Tags.FINALSEQUENCE_TAG))
            {
                StartCoroutine(ShowStory(7));
            }
            else if (other.gameObject.CompareTag(Tags.ASIMANAS_CODEX) && IntrantThirdPersonController.instance._receivedAsimanaCodex)
            {
                _playerUI._grimoire.gameObject.SetActive(true);
            }

        }

        #endregion

        #region Week-2

        if (_player._map == MAP.WEEK2) {
            if (other.gameObject.CompareTag(WeekTwo.PointA))
            {
                //_player.pause = true;
                StartCoroutine(PauseAndResume(20));
                StartCoroutine(ShowStory(1));
                GetComponent<PlayerTheraConnectionManager>()._theraRef.GetComponent<Thera>()._shutDownInit = true;
                GetComponent<GrimoireManager>()._hologramBinary.interactable = true;
                _grimoire._spts = other.gameObject.GetComponent<FortressOfEventualitiesTriggerManagerEarthandWater>()._spts;
            }else if (other.gameObject.CompareTag(WeekTwo.OrbsOfKinesis))
            {
                _player._inOrbsOfKinesis = true;
                StartCoroutine(PauseAndResume(10));
                StartCoroutine(ShowStory(2));
                LeanTween.scale(other.gameObject, Vector3.zero, 1f);
                Destroy(other.gameObject,1f);
                GetComponent<GrimoireManager>().EnterMiniGameWithCryptex();
                other.enabled = false;
            }else if (other.gameObject.CompareTag(WeekTwo.MiniGamePlatform))
            {
                _player._inOrbsOfKinesis = true;
                StartCoroutine(PauseAndResume(10));
                StartCoroutine(ShowStory(3));
                other.enabled = false;
            }else if (other.gameObject.CompareTag(WeekTwo.ShadowBlastWarning))
            {
                StartCoroutine(PauseAndResume(10));
                StartCoroutine(ShowStory(5));
                other.enabled = false;
            }else if (other.gameObject.CompareTag(WeekTwo.KrakepedeWarning))
            {
                StartCoroutine(PauseAndResume(10));
                StartCoroutine(ShowStory(6));
                CameraFollowingPlayer.Instance.Player = KrakePede.Instance.gameObject;
                CinemachineFollowZoom zoom = FindObjectOfType<CinemachineFollowZoom>();
                zoom.m_Width = 70;
                zoom.m_MaxFOV = 175;
            }else if (other.gameObject.CompareTag(WeekTwo.KaireFightingWarning))
            {
                StartCoroutine(PauseAndResume(10));
                StartCoroutine(ShowStory(7));
            }else if (other.gameObject.CompareTag(WeekTwo.MonsterEmergence))
            {
                StartCoroutine(PauseAndResume(10));
                StartCoroutine(ShowStory(8));
            }
            else if (other.gameObject.TryGetComponent<ManualTag>(out ManualTag tag))
            {
                if(tag._tag ==Tag.SecondOrbInfo)
                {
                    StartCoroutine(PauseAndResume(10));
                    StartCoroutine(ShowStory(11));
                }
                else if (tag._tag == Tag.ThirdOrbWarning)
                {
                    StartCoroutine(PauseAndResume(10));
                    StartCoroutine(ShowStory(12));
                }
            }
        }

        #endregion

        #region Week-3
        if(_player._map == MAP.WEEK3){
            if (other.gameObject.CompareTag(MapPointsWeekThree.MonsterEmergence))
            {
                StartCoroutine(PauseAndResume(10));
                StartCoroutine(ShowStory(1));
            }
            else if (other.gameObject.CompareTag(MapPointsWeekThree.ClawRange))
            {
                TheClaw claw = TheClaw.instance;
                if (claw!=null)
                {
                    CameraFollowingPlayer.Instance.Player = claw.gameObject;
                    CinemachineFollowZoom zoom = FindObjectOfType<CinemachineFollowZoom>();
                    zoom.m_Width = 20;
                    zoom.m_MaxFOV = 30;
                    StartCoroutine(PauseAndResume(10));
                    StartCoroutine(ShowStory(2));
                    other.gameObject.SetActive(false);
                }
                else
                {
                    _player._toast._text.text = "Open the chest at Stone enclosure";
                    _player._toast.ShowToast();
                }

            }
            else if (other.gameObject.CompareTag(MapPointsWeekThree.TheClaw))
            {
                StartCoroutine(PauseAndResume(10));
                StartCoroutine(ShowStory(3));
                CameraFollowingPlayer.Instance.Player = gameObject;
                CinemachineFollowZoom zoom = FindObjectOfType<CinemachineFollowZoom>();
                zoom.m_Width = 10;
                zoom.m_MaxFOV = 30;
                _player._darkMagic.SetActive(true);
                LeanTween.scale(TheClaw.instance.gameObject, new Vector3(0, 0, 0), 1f);
                Destroy(TheClaw.instance.gameObject);
                GetComponent<GrimoireManager>()._clawCount.text = "1";
                _player._collectedClaw = true;
                _player._collectedClawCount++;
            }
            else if (other.gameObject.CompareTag(MapPointsWeekThree.SafetyStoneEnclosure))
            {
                _player._playerenteredSafetyEnclosure = true;
                StartCoroutine(PauseAndResume(10));
                StartCoroutine(ShowStory(4));
            }
            else if (other.gameObject.CompareTag(MapPointsWeekThree.DisableForceField))
            {
                if (_player._earthShield.activeSelf)
                {
                    StartCoroutine(PauseAndResume(10));
                    StartCoroutine(ShowStory(7));
                    other.gameObject.SetActive(false);
                }
                else if (!_player._earthShield.activeSelf)
                {
                    _player._toast._text.text = "Go to stone enclosure for collecting more information.";
                    _player._toast.ShowToast();
                }
            }
            else if (other.gameObject.CompareTag(MapPointsWeekThree.ClawsRealPower))
            {
                StartCoroutine(PauseAndResume(20));
                StartCoroutine(ShowStory(8));
            }
            else if (other.gameObject.CompareTag(MapPointsWeekThree.StripMagicfromClaw))
            {
                if(_player._collectedTherCode > 4 && _player._magicScrolls > 2)
                {
                    StartCoroutine(PauseAndResume(10));
                    StartCoroutine(ShowStory(9));
                    other.gameObject.SetActive(false);
                }
                else
                {
                    _player._toast._text.text = "Need more ingredients. 5 Theracodes and 3 magic scrolls."; 
                    _player._toast.ShowToast();
                }
            }
            else if (other.gameObject.CompareTag(MapPointsWeekThree.StoneCourtyard))
            {
                StartCoroutine(PauseAndResume(10));
                StartCoroutine(ShowStory(13));
                transform.position = PlayerStage.instance.transform.position;
            }
            else if (other.gameObject.CompareTag(MapPointsWeekThree.EpimanaPointOfCourtyard) && !_epimanaPoint)
            {
                StartCoroutine(PauseAndResume(10));
                StartCoroutine(ShowStory(15));
            }
            else if (other.gameObject.CompareTag(MapPointsWeekThree.ExitCourtyard) )
            {
                StartCoroutine(PauseAndResume(10));
                StartCoroutine(ShowStory(17));
                other.enabled = false;
            }
            else if (other.gameObject.CompareTag(MapPointsWeekThree.TornodoPoint))
            {
                StartCoroutine(PauseAndResume(10));
                StartCoroutine(ShowStory(18));
                GetComponent<GrimoireManager>()._insideTornadoRange = true;
            }
        }
        #endregion

        #region Week-4
        if (_player._map == MAP.WEEK4)
        {
            if (other.TryGetComponent<ManualTag>(out ManualTag _manualTag))
            {
                if (_manualTag._tag == Tag.TheraClose)
                {
                    StartCoroutine(PauseAndResume(10));
                    StartCoroutine(ShowStory(2));
                }else if(_manualTag._tag == Tag.CaveEntrance)
                {
                    StartCoroutine(PauseAndResume(10));
                    StartCoroutine(ShowStory(3));
                    PvPPlayerUI.instance._collect.onClick.RemoveAllListeners();
                    PvPPlayerUI.instance._collect.gameObject.SetActive(true);
                    PvPPlayerUI.instance._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Arm and Fix thera";
                    PvPPlayerUI.instance._collect.onClick.AddListener(() =>
                    {
                        ArmandFixThera(tag);
                    });
                }else if (_manualTag._tag == Tag.ArturoArea)
                {
                    StartCoroutine(PauseAndResume(10));
                    StartCoroutine(ShowStory(4));
                }else if (_manualTag._tag == Tag.ArturoSpawner)
                {
                    StartCoroutine(PauseAndResume(10));
                    StartCoroutine(ShowStory(11));
                    MonsterMovementController[] _monsters = FindObjectsOfType<MonsterMovementController>();
                    foreach (var _monster in _monsters)
                    {
                        if (_monster._monsterType == MonsterType.Arturo)
                        {
                            CameraFollowingPlayer.Instance.Player = _monster.gameObject;
                            CinemachineFollowZoom zoom = FindObjectOfType<CinemachineFollowZoom>();
                            zoom.m_Width = 15;
                            zoom.m_MaxFOV = 20;
                        }
                    }
                }
                else if (_manualTag._tag == Tag.EpimanaPoint)
                {
                    StartCoroutine(PauseAndResume(10));
                    StartCoroutine(ShowStory(6));
                    _player.PlaceIngredients(other.gameObject);
                }
                else if (_manualTag._tag == Tag.CaveOfDissolvingMagic)
                {
                    StartCoroutine(PauseAndResume(10));
                    StartCoroutine(ShowStory(5));
                    WeekFourTriggerManager[] _weekfour = FindObjectsOfType<WeekFourTriggerManager>();
                    foreach (var _weekfourTrigger in _weekfour)
                    {
                        if(_weekfourTrigger.mapPoint == WeekFourTriggerManager.MapPoint.DissolvingMagic)
                        {
                            CameraFollowingPlayer.Instance.Player = _weekfourTrigger.gameObject;
                            CinemachineFollowZoom zoom = FindObjectOfType<CinemachineFollowZoom>();
                            zoom.m_Width = 15;
                            zoom.m_MaxFOV = 20;
                        }
                    }
                }
                else if (_manualTag._tag == Tag.ClawAndOrbSubmission)
                {
                    StartCoroutine(PauseAndResume(10));
                    StartCoroutine(ShowStory(7));
                    ManualTag[] _mapPoint = FindObjectsOfType<ManualTag>();
                    foreach (var map in _mapPoint)
                    {
                        if (map._tag == Tag.TheClaw)
                        {
                            CameraFollowingPlayer.Instance.Player = map.gameObject;
                            CinemachineFollowZoom zoom = FindObjectOfType<CinemachineFollowZoom>();
                            zoom.m_Width = 15;
                            zoom.m_MaxFOV = 20;
                        }
                    }
                }else if(_manualTag._tag == Tag.ArmorShowcaseStory)
                {
                    StartCoroutine(PauseAndResume(10));
                    StartCoroutine(ShowStory(10));
                }
            }
            else if (other.gameObject.TryGetComponent<WeekFourTriggerManager>(out WeekFourTriggerManager weekFour))
            {
                if(weekFour.mapPoint == WeekFourTriggerManager.MapPoint.DropletTrailPoint)
                {
                    StartCoroutine(PauseAndResume(10));
                    StartCoroutine(ShowStory(8));
                    WeekFourTriggerManager[] _weekfour = FindObjectsOfType<WeekFourTriggerManager>();
                    foreach (var _weekfourTrigger in _weekfour)
                    {
                        if (_weekfourTrigger.mapPoint == WeekFourTriggerManager.MapPoint.DropletTrailPoint)
                        {
                            CameraFollowingPlayer.Instance.Player = _weekfourTrigger._droplet;
                            CinemachineFollowZoom zoom = FindObjectOfType<CinemachineFollowZoom>();
                            zoom.m_Width = 15;
                            zoom.m_MaxFOV = 20;
                        }
                    }
                }
            }
        }
        #endregion

        #region Week-5

        if (_player._map == MAP.WEEK5)
        {
            if (other.TryGetComponent<ManualTag>(out ManualTag _manualTag))
            {
                if (_manualTag._tag == Tag.TheraScan)
                {
                    StartCoroutine(PauseAndResume(10));
                    StartCoroutine(ShowStory(1));
                    //Thera unable to move
                    GameObject _Thera = GetComponent<PlayerTheraConnectionManager>()._theraRef.gameObject;
                    Rigidbody _rb = _Thera.GetComponent<Rigidbody>();
                    NavMeshAgent _agent = _Thera.GetComponent<NavMeshAgent>();
                    _agent.isStopped = true;
                    _rb.constraints = RigidbodyConstraints.FreezeAll;
                }
                else if (_manualTag._tag == Tag.AsimanaLightShafts)
                {
                    StartCoroutine(PauseAndResume(10));
                    StartCoroutine(ShowStory(2));
                }
                else if (_manualTag._tag == Tag.AsimanaLightShaftsArtifactWarning)
                {
                    StartCoroutine(PauseAndResume(10));
                    StartCoroutine(ShowStory(3));
                    LeanTween.scale(other.gameObject, new Vector3(0, 0, 0), 1f);
                }
                else if (_manualTag._tag == Tag.LookUpToStarDeviceToFindMira)
                {
                    StartCoroutine(PauseAndResume(10));
                    StartCoroutine(ShowStory(6));
                }
            }
        }
        #endregion
    }

    IEnumerator ShowStory(int _story = 0)
    {           
        // Set the default profile to thera
        _messageIcon.sprite = _messageIconSprite[0];
        _currentStoryNumber = _story;
        _narrating = true;
        #region Week 1 Story

        if (_player._map == MAP.WEEK1)
        {
            if (_story == 1)
            {
                _messageIcon.sprite = _messageIconSprite[0];
                _storyList.Push(Messages.RecolettaMirrorPhare2);
                _storyList.Push(Messages.RecolettaMirrorPhare1);
                InitStory();
            }
            else if (_story == 2)
            {
                _storyList.Push(Messages.BreakSpell2);
                _storyList.Push(Messages.BreakSpell1);
                InitStory();
                // Play the specific playable
            }
            else if (_story == 3)
            {
                _storyList.Push(Messages.CollectTheraCode2);
                _storyList.Push(Messages.CollectTheraCode1);
                InitStory();
            }
            else if (_story == 4)
            {
                _messageIcon.sprite = _messageIconSprite[0];
                _storyList.Push(Messages.PowerCircuit);
                InitStory();
            }
            else if (_story == 5)
            {
                _messageIcon.sprite = _messageIconSprite[0];
                _storyList.Push(Messages.PlacedTheraCode2);
                _storyList.Push(Messages.PlacedTheraCode1);
                InitStory();
            }
            else if (_story == 6)
            {
                _messageIcon.sprite = _messageIconSprite[0];
                _storyList.Push(Messages.AsimanaCodex);
                InitStory();
            }
            else if (_story == 7)
            {
                _messageIcon.sprite = _messageIconSprite[0];
                _storyList.Push(Messages.FinalSeq);
                InitStory();
            }
            else if (_story == 8)
            {
                _messageIcon.sprite = _messageIconSprite[0];
                _storyList.Push(Messages.TheraUrgeToRevive);
                InitStory();
            }
            else if (_story == 9)
            {
                _messageIcon.sprite = _messageIconSprite[0];
                _storyList.Push(Messages.TerathianGate + FinishGate.instance.totalEnemies);
                InitStory();
            }
            else if (_story == 10)
            {
                _messageIcon.sprite = _messageIconSprite[0];
                _storyList.Push(Messages.TheraRevivalRequest);
                InitStory();
            }
            else if (_story == 11)
            {
                _messageIcon.sprite = _messageIconSprite[0];
                _storyList.Push(Messages.TheraImportant);
                InitStory();
            }
            else if (_story == 12)
            {
                _storyList.Push(Messages.RecolettaMirrorPhare1);
                _storyList.Push(Messages.PointCExitUp);
                InitStory();
            }
            else if (_story == 13)
            {
                _storyList.Push(Messages.PointCExitDown);
                InitStory();
            }
        }

        #endregion

        #region Week 2 Story

        else if (_player._map == MAP.WEEK2)
        {
            //Debug.LogError($"Starting week 2 story with story number {_story}");
            if (_story == 1)
            {
                _storyList.Push(Messages.UseSpellToShowBinaryHologram);
                _storyList.Push(Messages.TheraFrustated);
                InitStory();
            }
            else if (_story == 2)
            {
                _storyList.Push(Messages.EnterMiniGame);
                InitStory();
            }
            else if (_story == 3)
            {
                _storyList.Push(Messages.FindCryptex);
                InitStory();
            }
            else if (_story == 4)
            {
                _storyList.Push(Messages.TurnOrbsToEmitter);
                InitStory();
            }
            else if (_story == 5)
            {
                _storyList.Push(Messages.WarnPowerfullChest);
                InitStory();
            }
            else if (_story == 6)
            {
                _storyList.Push(Messages.WarnKrakePede);
                InitStory();
            }
            else if (_story == 7)
            {
                _storyList.Push(Messages.KaireFightingArea);
                InitStory();
            }
            else if (_story == 8)
            {
                _storyList.Push(Messages.MonsterEmergence);
                InitStory();
            }
            else if (_story == 9)
            {
                _storyList.Push(Messages.TerathianGate + FinishGate.instance.totalEnemies);
                InitStory();
            }
            else if (_story == 10)
            {
                _storyList.Push(Messages.TheraRevivalRequest);
                InitStory();
            }
            else if (_story == 11)
            {
                _storyList.Push(Messages.TheraSecondOrbInfo);
                InitStory();
            }
            else if (_story == 12)
            {
                _storyList.Push(Messages.DialogueThirdOrb);
                InitStory();
            }
            else if (_story == 13)
            {
                _storyList.Push(Messages.DialogueCollectedLastOrb);
                InitStory();
            }
        }

        #endregion

        #region Week 3 Story

        else if (_player._map == MAP.WEEK3)
        {
            if (_story == 1)
            {
                StartCoroutine(PauseAndResume(10));
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = Messages.MonsterEmergence;

                // Play the specific playable
                _playableDirector.Play();
                yield return new WaitForSeconds(10f);
            }
            else if (_story == 2)
            {
                StartCoroutine(PauseAndResume(10));
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = Messages.TheClaw;

                // Play the specific playable
                _playableDirector.Play();
                yield return new WaitForSeconds(10f);
                CameraFollowingPlayer.Instance.Player = gameObject;
                CinemachineFollowZoom zoom = FindObjectOfType<CinemachineFollowZoom>();
                zoom.m_MaxFOV = 25;
                zoom.m_Width = 35;
            }
            else if (_story == 3)
            {
                StartCoroutine(PauseAndResume(10));
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = Messages.DialogueClaw;

                // Play the specific playable
                _playableDirector.Play();
                yield return new WaitForSeconds(10f);
                CameraFollowingPlayer.Instance.Player = gameObject;
                CinemachineFollowZoom zoom = FindObjectOfType<CinemachineFollowZoom>();
                zoom.m_MaxFOV = 25;
                zoom.m_Width = 35;
            }
            else if (_story == 4)
            {
                StartCoroutine(PauseAndResume(10));
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = Messages.DialogueTheraCastReverseSpell;

                // Play the specific playable
                _playableDirector.Play();
                yield return new WaitForSeconds(10f);
                CameraFollowingPlayer.Instance.Player = gameObject;
                CinemachineFollowZoom zoom = FindObjectOfType<CinemachineFollowZoom>();
                zoom.m_MaxFOV = 25;
                zoom.m_Width = 35;
            }
            else if (_story == 5)
            {
                StartCoroutine(PauseAndResume(10));
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = Messages.DialogueSignalMilut;

                // Play the specific playable
                _playableDirector.Play();
                yield return new WaitForSeconds(10f);
            }
            else if (_story == 6)
            {
                StartCoroutine(PauseAndResume(10));
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = Messages.DialogueMilutWarning;

                // Play the specific playable
                _playableDirector.Play();
                yield return new WaitForSeconds(10f);

                GameObject milutRef = GetComponent<PlayerHenchmenconnectionManager>().milutRef;
                if (milutRef != null)
                {
                    LeanTween.scale(milutRef, new Vector3(0, 0, 0), 1f).setOnComplete(() =>
                    {
                        Destroy(milutRef);
                    });
                }
            }
            else if (_story == 7)
            {
                StartCoroutine(PauseAndResume(10));
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = Messages.DialogueTheraForceField;

                // Play the specific playable
                _playableDirector.Play();
                yield return new WaitForSeconds(5f);
                LeanTween.scale(_player._earthShield, new Vector3(0, 0, 0), 2f).setOnComplete(() =>{ _player._earthShield.GetComponent<ForceField>().Active = false; });
                yield return new WaitForSeconds(5f);
            }
            else if (_story == 8)
            {
                StartCoroutine(PauseAndResume(10));
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = Messages.DialogueClawsRealPower;

                // Play the specific playable
                _playableDirector.Play();
                yield return new WaitForSeconds(10f);
                LeanTween.scale(_player._darkMagic, new Vector3(0, 0, 0), 3f);
                _player._darkMagic.SetActive(false);
                _playableDirector.Stop();
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = Messages.DialogueClawsRealPowerTwo;

                // Play the specific playable
                _playableDirector.Play();
                GetComponent<Stamina_Script>().ReduceStamina(35);
                //LeanTween.
            }
            else if (_story == 9)
            {
                StartCoroutine(PauseAndResume(10));
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = Messages.DialogueCreateSpellToStrip;

                // Play the specific playable
                _playableDirector.Play();
                yield return new WaitForSeconds(10f);
                _playableDirector.Stop();
                _grimoire.EnablePanel();
                EnableDisablePanels(0, 0, 1);
                _grimoire._clawButton.gameObject.AddComponent<ButtonPingPongAnimations>();
            }
            else if (_story == 10)
            {
                _playableDirector.Stop();
                StartCoroutine(PauseAndResume(10));
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = Messages.DialogueNotEnoughEnergy;

                // Play the specific playable
                _playableDirector.Play();
                yield return new WaitForSeconds(10f);
                LeanTween.scale(_player._darkMagic, new Vector3(0.1f, 0.1f, 0.1f), 3f);
            }
            else if (_story == 11)
            {
                _playableDirector.Stop();
                StartCoroutine(PauseAndResume(10));
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = Messages.DialogueFreezeOutpouringDarkness;

                // Play the specific playable
                _playableDirector.Play();
                yield return new WaitForSeconds(10f);
                _playableDirector.Stop();
                _grimoire.EnablePanel();
                EnableDisablePanels(1, 0, 0);
                _grimoire._FreezeClaw.gameObject.AddComponent<ButtonPingPongAnimations>();
            }
            else if (_story == 12)
            {
                _playableDirector.Stop();
                StartCoroutine(PauseAndResume(10));
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = Messages.DialogueExtractEnergy;

                // Play the specific playable
                _playableDirector.Play();
                yield return new WaitForSeconds(10f);
            }
            else if (_story == 13)
            {
                _playableDirector.Stop();
                StartCoroutine(PauseAndResume(10));
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = Messages.DialogueExtractEnergy;

                // Play the specific playable
                _playableDirector.Play();
                yield return new WaitForSeconds(10f);
            }
            else if (_story == 14)
            {
                _playableDirector.Stop();
                StartCoroutine(PauseAndResume(10));
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = Messages.DialogueStoreClawInsideGrimoire;

                // Play the specific playable
                _playableDirector.Play();
                yield return new WaitForSeconds(10f);
                _playableDirector.Stop();
                _grimoire.EnablePanel();
                EnableDisablePanels(0, 0, 1);
                _grimoire._clawButton.gameObject.AddComponent<ButtonPingPongAnimations>();
            }
            else if (_story == 15)
            {
                _playableDirector.Stop();
                StartCoroutine(PauseAndResume(10));
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = Messages.DialogueEpimanaPoint;

                // Play the specific playable
                _playableDirector.Play();
                _epimanaPoint = true;
                yield return new WaitForSeconds(10f);
                _playableDirector.Stop();
                _grimoire.EnablePanel();
                EnableDisablePanels(0, 0, 1);
                _grimoire._clawButton.gameObject.AddComponent<ButtonPingPongAnimations>();
                yield return new WaitForSeconds(2f);
                _grimoire.EnablePanel();
                EnableDisablePanels(1, 0, 0);
                _grimoire._hologramBinary.interactable = true;
                _grimoire._hologramBinary.gameObject.AddComponent<ButtonPingPongAnimations>();
            }
            else if (_story == 16)
            {
                _playableDirector.Stop();
                yield return new WaitForSeconds(1f);
                StartCoroutine(PauseAndResume(10));
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = Messages.DialogueDisinetratingCourtyard;

                // Play the specific playable
                _playableDirector.Play();
                yield return new WaitForSeconds(10f);
                _playableDirector.Stop();
                _grimoire.EnablePanel();
            }
            else if (_story == 17)
            {
                _playableDirector.Stop();
                StartCoroutine(PauseAndResume(10));
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = Messages.DialogueOrin;

                // Play the specific playable
                _playableDirector.Play();
                yield return new WaitForSeconds(10f);
            }
            else if (_story == 18)
            {
                _playableDirector.Stop();
                StartCoroutine(PauseAndResume(10));
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = Messages.DialogueTornadoWarning;

                // Play the specific playable
                _playableDirector.Play();
                yield return new WaitForSeconds(10f);
                _playableDirector.Stop();
                _grimoire.EnablePanel();
                EnableDisablePanels(0, 0, 1);
                _grimoire._clawButton.gameObject.AddComponent<ButtonPingPongAnimations>();
            }
            else if (_story == 19)
            {
                _playableDirector.Stop();
                StartCoroutine(PauseAndResume(10));
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = Messages.DialoguePlaceOrbAndClaw;

                // Play the specific playable
                _playableDirector.Play();
                yield return new WaitForSeconds(10f);
                _playableDirector.Stop();
                _grimoire.EnablePanel();
                EnableDisablePanels(0, 0, 1);
                _grimoire._clawButton.gameObject.AddComponent<ButtonPingPongAnimations>();
            }
        }
        #endregion

        #region Week 4 Story
        else if (_player._map == MAP.WEEK4)
        {
            if (_story == 1)
            {
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = MessagesWeekFour.DialogueTheraFarAway;

                // Play the specific playable
                _playableDirector.Play();
                _messageIcon.sprite = _messageIconSprite[0];
                yield return new WaitForSeconds(10f);
                _playableDirector.Stop();
                StartCoroutine(PauseAndResume(10));
                CameraFollowingPlayer.Instance.Player = gameObject;
                CinemachineFollowZoom zoom = FindObjectOfType<CinemachineFollowZoom>();
                zoom.m_MaxFOV = 25;
                zoom.m_Width = 35;
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = MessagesWeekFour.DialogueTherGuidanceaboutRelicAndGrandFather;

                // Play the specific playable
                _playableDirector.Play();
                _messageIcon.sprite = _messageIconSprite[0];
                yield return new WaitForSeconds(10f);
                _playableDirector.Stop();
            }
            else if (_story == 2)
            {
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = MessagesWeekFour.DialogueTheraClose;

                // Play the specific playable
                _playableDirector.Play();
                _messageIcon.sprite = _messageIconSprite[0];
                yield return new WaitForSeconds(10f);
                _playableDirector.Stop();
            }
            else if (_story == 3)
            {
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = MessagesWeekFour.DialogueAutomaticCave;

                // Play the specific playable
                _playableDirector.Play();
                _messageIcon.sprite = _messageIconSprite[0];
                yield return new WaitForSeconds(10f);
                _playableDirector.Stop();
            }
            else if (_story == 4)
            {
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = MessagesWeekFour.DialogueWarningForArrows;

                // Play the specific playable
                _playableDirector.Play();
                _messageIcon.sprite = _messageIconSprite[0];
                yield return new WaitForSeconds(10f);
                _playableDirector.Stop();
            }
            else if (_story == 5)
            {
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = MessagesWeekFour.DialogueCollectIngrediants;

                // Play the specific playable
                _playableDirector.Play();
                _messageIcon.sprite = _messageIconSprite[0];
                yield return new WaitForSeconds(10f);
                _playableDirector.Stop();
                CameraFollowingPlayer.Instance.Player = gameObject;
                CinemachineFollowZoom zoom = FindObjectOfType<CinemachineFollowZoom>();
                zoom.m_MaxFOV = 25;
                zoom.m_Width = 35;
            }
            else if (_story == 6)
            {
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = MessagesWeekFour.DialoguePlaceIngredients;

                // Play the specific playable
                _playableDirector.Play();
                _messageIcon.sprite = _messageIconSprite[0];
                yield return new WaitForSeconds(10f);
                _playableDirector.Stop();
            }
            else if (_story == 7)
            {
                CinemachineFollowZoom zoom = FindObjectOfType<CinemachineFollowZoom>();
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = MessagesWeekFour.DialogueCollectandPlaceArtifacts;
                yield return new WaitForSeconds(3.5f);
                _playableDirector.Play();
                ManualTag[] _mapPoint = FindObjectsOfType<ManualTag>();
                foreach (var map in _mapPoint)
                {
                    if (map._tag == Tag.OrbsOfKinesis)
                    {
                        CameraFollowingPlayer.Instance.Player = map.gameObject;
                        zoom.m_Width = 25;
                        zoom.m_MaxFOV = 30;
                    }
                }
                yield return new WaitForSeconds(3.5f);
                WeekFourTriggerManager[] _weekfour = FindObjectsOfType<WeekFourTriggerManager>();
                foreach (var _weekfourTrigger in _weekfour)
                {
                    if (_weekfourTrigger.mapPoint == WeekFourTriggerManager.MapPoint.GatePlatform)
                    {
                        CameraFollowingPlayer.Instance.Player = _weekfourTrigger.gameObject;
                        zoom.m_Width = 15;
                        zoom.m_MaxFOV = 20;
                    }
                }
                // Play the specific playable
                _messageIcon.sprite = _messageIconSprite[0];
                yield return new WaitForSeconds(3f);
                _playableDirector.Stop();
                CameraFollowingPlayer.Instance.Player = gameObject;
                zoom.m_MaxFOV = 25;
                zoom.m_Width = 35;
            }
            else if (_story == 8)
            {
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = MessagesWeekFour.DialogueFollowTrail;

                // Play the specific playable
                _playableDirector.Play();
                _messageIcon.sprite = _messageIconSprite[0];
                yield return new WaitForSeconds(10f);
                _playableDirector.Stop();
                CameraFollowingPlayer.Instance.Player = gameObject;
                CinemachineFollowZoom zoom = FindObjectOfType<CinemachineFollowZoom>();
                zoom.m_MaxFOV = 25;
                zoom.m_Width = 35;
            }
            else if (_story == 9)
            {
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = MessagesWeekFour.DialogueTheraInformingAboutKnockDown;

                // Play the specific playable
                _playableDirector.Play();
                _messageIcon.sprite = _messageIconSprite[0];
                yield return new WaitForSeconds(10f);
                _playableDirector.Stop();
            }
            else if (_story == 10)
            {
                CinemachineFollowZoom zoom = FindObjectOfType<CinemachineFollowZoom>();
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = MessagesWeekFour.Dialoguearmor;
                _playableDirector.Play();
                yield return new WaitForSeconds(1f);
                ManualTag[] _mapPoint = FindObjectsOfType<ManualTag>();
                foreach (var map in _mapPoint)
                {
                    if (map._tag == Tag.ShoulderArmor)
                    {
                        CameraFollowingPlayer.Instance.Player = map.gameObject;
                        zoom.m_Width = 5;
                        zoom.m_MaxFOV = 15;
                    }
                }
                yield return new WaitForSeconds(3f);
                foreach (var map in _mapPoint)
                {
                    if (map._tag == Tag.BodyArmor)
                    {
                        CameraFollowingPlayer.Instance.Player = map.gameObject;
                        zoom.m_Width = 5;
                        zoom.m_MaxFOV = 10;
                    }
                }
                // Play the specific playable
                _messageIcon.sprite = _messageIconSprite[0];
                yield return new WaitForSeconds(3f);
                foreach (var map in _mapPoint)
                {
                    if (map._tag == Tag.ForearmsArmor)
                    {
                        CameraFollowingPlayer.Instance.Player = map.gameObject;
                        zoom.m_Width = 5;
                        zoom.m_MaxFOV = 10;
                    }
                }
                yield return new WaitForSeconds(3f);
                _playableDirector.Stop();
                CameraFollowingPlayer.Instance.Player = gameObject;
                zoom.m_MaxFOV = 25;
                zoom.m_Width = 35;
                _grimoire.EnablePanel();
                EnableDisablePanels(0, 0, 1);
                ButtonPingPongAnimations animation = _grimoire._armorState.gameObject.AddComponent<ButtonPingPongAnimations>();
                Destroy(animation);
            }
            else if (_story == 11)
            {
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = MessagesWeekFour.DialogueWarningForArturo;

                // Play the specific playable
                _playableDirector.Play();
                _messageIcon.sprite = _messageIconSprite[0];
                yield return new WaitForSeconds(10f);
                _playableDirector.Stop();
                CameraFollowingPlayer.Instance.Player = gameObject;
                CinemachineFollowZoom zoom = FindObjectOfType<CinemachineFollowZoom>();
                zoom.m_MaxFOV = 25;
                zoom.m_Width = 35;
            }
            else if (_story == 12)
            {
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = MessagesWeekFour.DialogueGlimmer;

                // Play the specific playable
                _playableDirector.Play();
                _messageIcon.sprite = _messageIconSprite[0];
                yield return new WaitForSeconds(10f);
                _playableDirector.Stop();
                CameraFollowingPlayer.Instance.Player = gameObject;
                CinemachineFollowZoom zoom = FindObjectOfType<CinemachineFollowZoom>();
                zoom.m_MaxFOV = 25;
                zoom.m_Width = 35;
                GlimmerPower();
            }
            else if (_story == 13)
            {
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = MessagesWeekFour.DialogueGlimmerTypeOne;

                // Play the specific playable
                _playableDirector.Play();
                _messageIcon.sprite = _messageIconSprite[0];
                yield return new WaitForSeconds(10f);
                _playableDirector.Stop();
                CameraFollowingPlayer.Instance.Player = gameObject;
                CinemachineFollowZoom zoom = FindObjectOfType<CinemachineFollowZoom>();
                zoom.m_MaxFOV = 25;
                zoom.m_Width = 35;
            }
            else if (_story == 14)
            {
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = MessagesWeekFour.DialogueGlimmerTypeTwo;

                // Play the specific playable
                _playableDirector.Play();
                _messageIcon.sprite = _messageIconSprite[0];
                yield return new WaitForSeconds(10f);
                _playableDirector.Stop();
                CameraFollowingPlayer.Instance.Player = gameObject;
                CinemachineFollowZoom zoom = FindObjectOfType<CinemachineFollowZoom>();
                zoom.m_MaxFOV = 25;
                zoom.m_Width = 35;
            }
            else if (_story == 15)
            {
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = MessagesWeekFour.DialogueGlimmerTypeThree;

                // Play the specific playable
                _playableDirector.Play();
                _messageIcon.sprite = _messageIconSprite[0];
                yield return new WaitForSeconds(10f);
                _playableDirector.Stop();
                CameraFollowingPlayer.Instance.Player = gameObject;
                CinemachineFollowZoom zoom = FindObjectOfType<CinemachineFollowZoom>();
                zoom.m_MaxFOV = 25;
                zoom.m_Width = 35;
            }
            else if (_story == 16)
            {
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = MessagesWeekFour.DialogueGlimmerTypeFour;

                // Play the specific playable
                _playableDirector.Play();
                _messageIcon.sprite = _messageIconSprite[0];
                yield return new WaitForSeconds(10f);
                _playableDirector.Stop();
                CameraFollowingPlayer.Instance.Player = gameObject;
                CinemachineFollowZoom zoom = FindObjectOfType<CinemachineFollowZoom>();
                zoom.m_MaxFOV = 25;
                zoom.m_Width = 35;
            }
            else if (_story == 17)
            {
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = MessagesWeekFour.DialogueGlimmerTypeFive;

                // Play the specific playable
                _playableDirector.Play();
                _messageIcon.sprite = _messageIconSprite[0];
                yield return new WaitForSeconds(10f);
                _playableDirector.Stop();
                CameraFollowingPlayer.Instance.Player = gameObject;
                CinemachineFollowZoom zoom = FindObjectOfType<CinemachineFollowZoom>();
                zoom.m_MaxFOV = 25;
                zoom.m_Width = 35;
            }
        }
        #endregion

        #region Week 5 Story
        else if (_player._map == MAP.WEEK5)
        {
            if (_story == 1)
            {
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = MessagesWeekFive.DialogueTheraScan;

                // Play the specific playable
                _playableDirector.Play();
                yield return new WaitForSeconds(10f);
                _playableDirector.Stop();
            }
            else if (_story == 2)
            {
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = MessagesWeekFive.DialogueAsimanaShaft;

                // Play the specific playable
                _playableDirector.Play();
                yield return new WaitForSeconds(10f);
                _playableDirector.Stop();
            }
            else if (_story == 3)
            {
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = MessagesWeekFive.DialogueAsimanaShaftWarning;

                // Play the specific playable
                _playableDirector.Play();
                yield return new WaitForSeconds(10f);
                _playableDirector.Stop();
            }
            else if (_story == 4)
            {
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = MessagesWeekFive.DialogueFindKey;

                // Play the specific playable
                _playableDirector.Play();
                yield return new WaitForSeconds(10f);
                _playableDirector.Stop();
            }
            else if (_story == 5)
            {
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = MessagesWeekFive.DialogueRevealMonster;

                // Play the specific playable
                _playableDirector.Play();
                yield return new WaitForSeconds(10f);
                _playableDirector.Stop();
            }
            else if (_story == 6)
            {
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = MessagesWeekFive.DialogueStarDeviceMiraFind;

                // Play the specific playable
                _playableDirector.Play();
                yield return new WaitForSeconds(10f);
                _playableDirector.Stop();
                _grimoire.EnablePanel();
                EnableDisablePanels(1, 0, 0);
                _grimoire._map.gameObject.AddComponent<Highlight>().ChangeColor();

            }
            else if (_story == 7)
            {
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = MessagesWeekFive.DialogueMirasChallenge;

                // Play the specific playable
                _playableDirector.Play();
                yield return new WaitForSeconds(10f);
                _playableDirector.Stop();
                _miraChallengePanel.SetActive(true);
            }
            else if (_story == 8)
            {
                yield return new WaitForSeconds(3f);
                GameObject SptsOne = GetComponent<PlayerTheraConnectionManager>()._spawnPoints[1];
                GameObject SptsTwo = GetComponent<PlayerTheraConnectionManager>()._spawnPoints[0];
                GameObject _rasveus = Instantiate(_player._rasVeus, SptsOne.transform.position, Quaternion.Euler(-90, 0, 0));
                GameObject _Orin = Instantiate(_player._Orin, SptsTwo.transform.position, Quaternion.Euler(-90, 0, 0));

                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = MessagesWeekFive.DialogueOrinAndRasveus;

                // Play the specific playable
                _playableDirector.Play();

                //Hault
                yield return new WaitForSeconds(10f);
                _playableDirector.Stop();
                Destroy(_rasveus);
                Destroy(_Orin);
                //Hault
                yield return new WaitForSeconds(3f);
                GeneralCharacterManager []_characters = FindObjectsOfType<GeneralCharacterManager>();
                GeneralCharacterManager characterRef = null;
                foreach (var _character in _characters)
                {
                    if (_character._characterType == GeneralCharacterManager.CharacterType.Mira)
                    {
                        Destroy(_character._smokeRef);
                        characterRef = _character;
                    }
                }
                WeekfiveTriggerManager[] triggers = FindObjectsOfType<WeekfiveTriggerManager>();
                foreach (var trigger in triggers)
                {
                    if(trigger.mapPoint == MapPoint.MiraTarget)
                    {
                        if (trigger._kairRef != null)
                        {
                            Destroy(trigger._kairRef);
                            Destroy(characterRef.gameObject); 
                        }
                    }
                }
                //Debug.Log("Playing MirasFinalTarget");

                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = MessagesWeekFive.DialogueMirasFinalTarget;

                // Play the specific playable
                _playableDirector.Play();

                CinemachineFollowZoom zoom = FindObjectOfType<CinemachineFollowZoom>();
                MonsterMovementController[] monster = FindObjectsOfType<MonsterMovementController>();

                foreach (var item in monster)
                {
                    if(item._monsterType == MonsterType.FARRAPTORBOSS)
                    {
                        CameraFollowingPlayer.Instance.Player = item.gameObject;
                        zoom.m_Width = 20;
                        zoom.m_MaxFOV = 30;
                    }
                }

                yield return new WaitForSeconds(5f);
                foreach (var trigger in triggers)
                {
                    if (trigger.mapPoint == MapPoint.Maze)
                    {
                        CameraFollowingPlayer.Instance.Player = trigger.gameObject;
                        zoom.m_Width = 20;
                        zoom.m_MaxFOV = 30;
                    }
                }


                //Hault
                yield return new WaitForSeconds(5f);
                _playableDirector.Stop();
                CameraFollowingPlayer.Instance.Player = gameObject;
                zoom.m_MaxFOV = 25;
                zoom.m_Width = 35;
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = MessagesWeekFive.DialogueFinal;

                // Play the specific playable
                _playableDirector.Play();
                //Hault
                yield return new WaitForSeconds(4f);
                _playableDirector.Stop();
            }
            else if (_story == 9)
            {
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
                _target.GetComponent<TextMeshProUGUI>().text = MessagesWeekFive.DialogueCollectMagicalKey;

                // Play the specific playable
                _playableDirector.Play();
                yield return new WaitForSeconds(10f);
                _playableDirector.Stop();
            }
        }
        #endregion
    }

    private void ArmandFixThera(string tag)
    {
        PvPPlayerUI.instance._collect.gameObject.SetActive(false);
        GetComponent<PlayerTheraConnectionManager>()._theraRef.GetComponent<Thera>().ArmorThera();
    }

    private void PlaceTheraCodes(GameObject _object)
    {
        StartCoroutine(ShowStory(5));
        StartCoroutine(PauseAndResume(3));
        _object.gameObject.GetComponent<FortressOfEventualitiesTriggerManager>()._powerCircuitVfx.gameObject.SetActive(true);
        IntrantThirdPersonController.instance._placedTheraCode = true;
        _playerUI._PlaceCodes.gameObject.SetActive(false);
    }

    public void PublicPauseAndResume(int val)
    {
        StartCoroutine(PauseAndResume(val));
    }

    IEnumerator PauseAndResume(int _pauseTime)
    {
        /* Debug.LogError("Pausing...");
         _player.pause = true;
         _player._isAnimating = true;
         yield return new WaitForSeconds(_pauseTime);
         _player.pause = false;
         _player._isAnimating = false;
         Debug.LogError("Resume...");*/
        yield return null;
    }

    #region Week 1 InternalCalls
    internal void TheraIsImportantInThisFight()
    {
        StartCoroutine(PauseAndResume(10));
        StartCoroutine(ShowStory(11));
    }
    #endregion

    #region Week 3 InternalCalls

    internal void NotEnoughEnergyToSpare()
    {
        StartCoroutine(PauseAndResume(10));
        StartCoroutine(ShowStory(10));
    }


    internal void FreezeEnergyFromClaw()
    {
        StartCoroutine(PauseAndResume(10));
        StartCoroutine(ShowStory(11));
    }

    internal void ExtractEnergy()
    {
        StartCoroutine(PauseAndResume(10));
        StartCoroutine(ShowStory(12));
    }

    internal void StoreClaw()
    {
        StartCoroutine(PauseAndResume(10));
        StartCoroutine(ShowStory(14));
    }

    public void RepairDisintegrated()
    {
        StartCoroutine(PauseAndResume(10));
        StartCoroutine(ShowStory(16));
    }

    #endregion

    #region Week 4 InternalCalls

    internal void TheraInformingAboutKnockDown()
    {
        StartCoroutine(PauseAndResume(10));
        StartCoroutine(ShowStory(9));
    }

    internal void InitCollectKey()
    {
        StartCoroutine(PauseAndResume(10));
        StartCoroutine(ShowStory(9));
    }

    internal void CollectGlimmerArtifact(int _glimmerType)
    {
        this._glimmerType = _glimmerType;
        StartCoroutine(PauseAndResume(10));
        StartCoroutine(ShowStory(12));
        GeneralArtifactsManager[] artifacts = FindObjectsOfType<GeneralArtifactsManager>();
        foreach (var artifact in artifacts)
        {
            if (artifact.artifact == GeneralArtifactsManager.Artifact.GlimmerArtifact)
            {
                CameraFollowingPlayer.Instance.Player = artifact.gameObject;
                CinemachineFollowZoom zoom = FindObjectOfType<CinemachineFollowZoom>();
                zoom.m_Width = 10;
                zoom.m_MaxFOV = 25;
            }
        }
    }

    void GlimmerPower()
    {
        if (_glimmerType == 0)
        {
            StartCoroutine(PauseAndResume(10));
            StartCoroutine(ShowStory(13));
        }
        else if (_glimmerType == 1)
        {
            StartCoroutine(PauseAndResume(10));
            StartCoroutine(ShowStory(14));
        }
        else if (_glimmerType == 2)
        {
            StartCoroutine(PauseAndResume(10));
            StartCoroutine(ShowStory(15));
        }
        else if (_glimmerType == 3)
        {
            StartCoroutine(PauseAndResume(10));
            StartCoroutine(ShowStory(16));
        }
        else if (_glimmerType == 4)
        {
            StartCoroutine(PauseAndResume(10));
            StartCoroutine(ShowStory(17));
        }
    }
    #endregion

    public void TheraFarAway()
    {
        StartCoroutine(PauseAndResume(10));
        CinemachineFollowZoom zoom = FindObjectOfType<CinemachineFollowZoom>();
        zoom.m_Width = 35;
        zoom.m_MaxFOV = 175;
        StartCoroutine(ShowStory(1));
    }

    internal void FindKey()
    {
        StartCoroutine(PauseAndResume(10));
        StartCoroutine(ShowStory(4));
    }

    internal void RevealHiddenMonsters()
    {
        StartCoroutine(PauseAndResume(10));
        StartCoroutine(ShowStory(5));
    }

    internal void MirasChallenge(GameObject mirasChallengePanel)
    {
        StartCoroutine(PauseAndResume(10));
        StartCoroutine(ShowStory(7));
        _miraChallengePanel = mirasChallengePanel;
    }

    internal void RepairMira()
    {
        StartCoroutine(PauseAndResume(30));
        StartCoroutine(ShowStory(8));
    }

    private void EnableDisablePanels(int panelA, int panelB, int panelC)
    {
        _grimoire._spellPanel.SetActive(panelA == 0 ? false : true);
        _grimoire._codePanel.SetActive(panelB == 0 ? false : true);
        _grimoire._counterPanel.SetActive(panelC == 0 ? false : true);
    }


    void InitStory()
    {
        Debug.LogError($"PrevStory{_previousStoryNumber} & Current story {_currentStoryNumber}");
        //Add animation to skip button
        try
        {
            _skip.gameObject.TryGetComponent<ButtonPingPongAnimations>(out ButtonPingPongAnimations _button);
            if (_button == null)
            {
                _skip.gameObject.AddComponent<ButtonPingPongAnimations>().animationDuration = 1f;
            }
        }catch (Exception ex) { }

        //Make player pause
        _player.pause = true;
        _player._isAnimating = true;
        _narrating = true;
        
        //Current story to be played
        string currentStory = _storyList.Pop();

        //Keep track of previous story
        if(_currentStoryNumber == _previousStoryNumber)
        {
            _prevStoryList.Push(currentStory);
        }else if(_currentStoryNumber != _previousStoryNumber)
        {
            _prevStoryList.Clear();
            Debug.LogError("Clearing previous story");
            _previousStoryNumber = _currentStoryNumber;
            _prevStoryList.Push(currentStory);
        }
        //Change story UI
        _messageIcon.sprite = _messageIconSprite[0];
        
        //Story 
        _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _target);
        _target.GetComponent<TextMeshProUGUI>().text = currentStory;

        // Play the specific playable
        _playableDirector.Play();
        StartCoroutine(StoryFunctionality());
    }

    IEnumerator StoryFunctionality()
    {
        if (_player._map == MAP.WEEK2)
        {
            if (_currentStoryNumber == 3)
            {
                ManualTag[] manualTag = FindObjectsOfType<ManualTag>();
                CinemachineFollowZoom zoom = FindObjectOfType<CinemachineFollowZoom>();
                foreach (var tag in manualTag)
                {
                    if (tag._tag == Tag.AsimanaCryptex)
                    {
                        CameraFollowingPlayer.Instance.Player = tag.gameObject;
                        zoom.m_MaxFOV = 25;
                        zoom.m_Width = 35;
                    }
                }
                yield return new WaitForSeconds(4f);
                foreach (var tag in manualTag)
                {
                    if (tag._tag == Tag.MinigamesOrbsOfKinesis)
                    {
                        CameraFollowingPlayer.Instance.Player = tag.gameObject;
                        zoom.m_MaxFOV = 25;
                        zoom.m_Width = 35;
                    }
                }
            }
            else if(_currentStoryNumber == 12)
            {
                ManualTag[] manualTag = FindObjectsOfType<ManualTag>();
                CinemachineFollowZoom zoom = FindObjectOfType<CinemachineFollowZoom>();
                foreach (var tag in manualTag)
                {
                    if (tag._tag == Tag.ThirdOrb)
                    {
                        CameraFollowingPlayer.Instance.Player = tag.gameObject;
                        zoom.m_MaxFOV = 25;
                        zoom.m_Width = 35;
                    }
                }
                yield return new WaitForSeconds(5f);
                foreach (var tag in manualTag)
                {
                    if (tag._tag == Tag.FerraptorCave)
                    {
                        CameraFollowingPlayer.Instance.Player = tag.gameObject;
                        zoom.m_MaxFOV = 25;
                        zoom.m_Width = 35;
                    }
                }
            } 
        }    
    }

    void PostStoryFunctionality()
    {
        if (_player._map == MAP.WEEK1)
        {
            if (_currentStoryNumber == 7)
            {
                _grimoire.EnablePanel();
                EnableDisablePanels(0, 1, 0);
                _playerUI._chargeMainFrame.gameObject.AddComponent<ButtonPingPongAnimations>();
            }else if (_currentStoryNumber == 8)
            {
                _grimoire.EnablePanel();
                EnableDisablePanels(0, 1, 0);
                _playerUI._realeaseThera.gameObject.AddComponent<ButtonPingPongAnimations>();
            }
            else if (_currentStoryNumber == 10)
            {
                _grimoire.EnablePanel();
                EnableDisablePanels(1, 0, 0);
                _grimoire._revive.gameObject.AddComponent<ButtonPingPongAnimations>();
            }
        }else if (_player._map == MAP.WEEK2)
        {
            if (_currentStoryNumber == 1)
            {
                _grimoire.EnablePanel();
                _grimoire._hologramBinary.gameObject.AddComponent<ButtonPingPongAnimations>();
            }else if (_currentStoryNumber == 2)
            {
                _grimoire.EnablePanel();
                _grimoire._adacodeCryptex.gameObject.AddComponent<ButtonPingPongAnimations>();
            }
            else if (_currentStoryNumber == 3)
            {
                CinemachineFollowZoom zoom = FindObjectOfType<CinemachineFollowZoom>();
                CameraFollowingPlayer.Instance.Player = gameObject;
                zoom.m_MaxFOV = 25;
                zoom.m_Width = 35;
                _grimoire.EnablePanel();
                _grimoire._adacodeCryptex.gameObject.AddComponent<ButtonPingPongAnimations>();
            }
            else if (_currentStoryNumber == 4)
            {
                _grimoire.EnablePanel();
                _grimoire._turnToEmitter.gameObject.AddComponent<ButtonPingPongAnimations>();
            }
            else if (_currentStoryNumber == 6)
            {
                CameraFollowingPlayer.Instance.Player = gameObject;
                CinemachineFollowZoom zoom = FindObjectOfType<CinemachineFollowZoom>();
                zoom.m_MaxFOV = 25;
                zoom.m_Width = 35;
            }else if (_currentStoryNumber == 12)
            {
                CinemachineFollowZoom zoom = FindObjectOfType<CinemachineFollowZoom>();
                CameraFollowingPlayer.Instance.Player = gameObject;
                zoom.m_MaxFOV = 25;
                zoom.m_Width = 35;
            }
        }
    }
}
