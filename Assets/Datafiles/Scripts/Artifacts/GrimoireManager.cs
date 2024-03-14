using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GrimoireManager : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] public Button _revive;
    [SerializeField] public Button _hologramBinary;
    [SerializeField] public Button _map;
    [SerializeField] public Button _adacodeCryptex;
    [SerializeField] public Button _turnToEmitter;
    [SerializeField] public Button _clawButton;
    [SerializeField] public Button _FreezeClaw;
    [SerializeField] public Button _armorState;
    [SerializeField] public Button _glimmer;
    [SerializeField] public Button _help;
    [SerializeField] private Image _mapIcon;
    [SerializeField] private Image _adacodeIcon;
    [SerializeField] private Image _FreezeClawIcon;

    [Header("Counters")]
    [SerializeField] private TMP_Text _orbsCount;
    [SerializeField] public TMP_Text _clawCount;
    [SerializeField] public TMP_Text _magicScrolls;
    
    
    [Header("Camera")]
    [SerializeField] private Camera _mapFullViewCamera;


    [Header("Panel")]
    public GameObject _spellPanel;
    public GameObject _codePanel;
    public GameObject _counterPanel;
    [SerializeField] private GameObject _mapFullView;
    [SerializeField] private GameObject _grimoirePanel;
    [SerializeField] private GameObject _helpPanel;


    [Header("Sprite")] 
    public Sprite _lockSprite;
    public Sprite _mapSprite;
    public Sprite _scroll;
    public Sprite _FreezeClawSprite;

    [Header("Artifacts")]
    public GameObject _claw;
    public GameObject _clawNormal;
    public GameObject clawRef;
    public GameObject clawSpts;

    [Header("spts")]
    public GameObject[] _spts;

    [Header("Bool")]
    public bool _holoBinaryCreated = false;
    public bool _seqInit = false;
    public bool _mapEnabled = false;
    public bool _clawInstantiated = false;
    public bool _clawsFreezed = false;
    public bool _extractedEnergy = false;
    public bool _strippedDarkMagic = false;
    public bool _insideTornadoRange = false;
    public bool _enabledWeaponAura = false;


    int temptOrbCount;

    [Header("Player Controller")]
    public IntrantThirdPersonController _playerController;

    void Start()
    {
        _playerController = GetComponent<IntrantThirdPersonController>();
        
        //Setup Star Device Camera
        if(_playerController._map != MAP.WEEK1)
        {
           _mapFullViewCamera = GameObject.Find("StarDeviceCamera").GetComponent<Camera>();
           _mapFullViewCamera.gameObject.SetActive(false);
        }
        //Revive Thera
        # region Revive Thera
        if (_playerController._map == MAP.WEEK2 || _playerController._map == MAP.WEEK3 || _playerController._map == MAP.WEEK4)
        {
            _revive.interactable = false;
        }
        else
        {
            _revive.onClick.AddListener(() => { ReviveThera(); });
        }
        #endregion

        #region HologramBinary
        if (_playerController._map == MAP.WEEK2)
        {
            _hologramBinary.onClick.AddListener(() => { _hologramBinaryFunc();});
        }
        else if (_playerController._map == MAP.WEEK3)
        {
            _hologramBinary.onClick.AddListener(() => { InetegrationMagic();});
        }
        #endregion

        #region FullMapView
        if (_playerController._map == MAP.WEEK3)
        {
            EnableMapViewfunc();
        }
        else if (_playerController._map == MAP.WEEK4)
        {
            EnableMapViewfunc();
        }
        else if(_playerController._map == MAP.WEEK5)
        {
            EnableMapViewfunc();
        }
        #endregion

        _adacodeCryptex.onClick.AddListener(() => { AdacodeCryptex(); });
        _turnToEmitter.onClick.AddListener(() => { TurnToEmitters(); });
        _clawButton.onClick.AddListener(() => { BringForthClaw(); });
        _FreezeClaw.onClick.AddListener(() => { FreezeClawsEnergy(); });
        _help.onClick.AddListener(() => { _helpPanel.SetActive(!_helpPanel.activeSelf); });

        #region Decide weather grimoire should be visible from the start
        if (_playerController._map == MAP.WEEK2 || _playerController._map == MAP.WEEK3 || _playerController._map == MAP.WEEK4 || _playerController._map == MAP.WEEK5)
        {
            GetComponent<PvPPlayerUI>()._grimoire.gameObject.SetActive(true);
        }
        #endregion
    }

    private void FreezeClawsEnergy()
    {
        TheClaw.instance._darkMagic.SetActive(false);
        TheClaw.instance._freeze.SetActive(true);
        DisablePanel();
        _clawsFreezed = true;
        _FreezeClaw.interactable = false;
        TryGetComponent<ButtonPingPongAnimations>(out ButtonPingPongAnimations pingpongAnim);
        if (pingpongAnim != null)
        {
            Destroy(pingpongAnim);
        }
    }

    private void Update()
    {
        if (temptOrbCount != _playerController._collectedOrbs) { 
            temptOrbCount = _playerController._collectedOrbs;
            _orbsCount.text = temptOrbCount.ToString();
        }
    }

    private void ReviveThera()
    {
        var _thera = GetComponent<PlayerTheraConnectionManager>()._theraRef;
        _thera.GetComponent<Thera>().enabled = true;
        _thera.GetComponent<Thera>()._therareviveVFX.gameObject.SetActive(true);
        _thera.GetComponent<TheraHealthManager>().Health = 100;
        _thera.GetComponent<TheraHealthManager>().HealthUI_Canvas.SetActive(true);
        _thera.GetComponent<NavMeshAgent>().enabled = true;
        DisablePanel();
    }
    
    private void _hologramBinaryFunc()
    {
        if (!_holoBinaryCreated && _spts.Length>0)
        {
            IntrantThirdPersonController _player = GetComponent<IntrantThirdPersonController>();
            int index = UnityEngine.Random.Range(0,2);
            GameObject hologramBinary = Instantiate(_player._binaryHologram, _spts[index].transform.position, Quaternion.Euler(-90, 0, 0));
            PvPPlayerUI.instance._initiateBinarySequence.gameObject.SetActive(true);
            PvPPlayerUI.instance._initiateBinarySequence.onClick.RemoveAllListeners();
            PvPPlayerUI.instance._initiateBinarySequence.onClick.AddListener(() => { InitiateSequence(hologramBinary); });
            _hologramBinary.interactable = false;
            DisablePanel();
        }
        TryGetComponent<ButtonPingPongAnimations>(out ButtonPingPongAnimations pingpongAnim);
        if(pingpongAnim != null)
        {
            Destroy(pingpongAnim);
        }
    }
    private void InetegrationMagic()
    {
        if (_playerController._magicScrolls>2 && _playerController._collectedTherCode>4)
        {
            GameObject[] _sp = GetComponent<PlayerTheraConnectionManager>()._spawnPoints;
            int index = UnityEngine.Random.Range(0, 2);
            GameObject hologramBinary = Instantiate(_playerController._integrationMagic, _sp[index].transform.position, Quaternion.Euler(-90, 0, 0));
            PvPPlayerUI.instance._initiateBinarySequence.gameObject.SetActive(true);
            PvPPlayerUI.instance._initiateBinarySequence.onClick.RemoveAllListeners();
            PvPPlayerUI.instance._initiateBinarySequence.onClick.AddListener(() => { InitiateSequence(hologramBinary); });
            _hologramBinary.interactable = false;
            DisablePanel();
        }
        TryGetComponent<ButtonPingPongAnimations>(out ButtonPingPongAnimations pingpongAnim);
        if (pingpongAnim != null)
        {
            Destroy(pingpongAnim);
        }
    }
    private void InitiateSequence(GameObject holo)
    {
        if (!_seqInit)
        {
            StartCoroutine(CompletedSequence(holo));
        }
    }

    private IEnumerator CompletedSequence(GameObject holo)
    {
        PvPPlayerUI.instance._initiateBinarySequence.gameObject.SetActive(false);
        holo.GetComponent<BinaryHolo>().Initiate();
        yield return new WaitForSeconds(5f);
        if (_playerController._map == MAP.WEEK2)
        {
            GameObject _starDevice = Instantiate(_playerController._starDevice, holo.transform.position, quaternion.identity);
        }
        else if (_playerController._map == MAP.WEEK3)
        {
            FortressOfEventualitiesElemental[] _fortress = FindObjectsOfType<FortressOfEventualitiesElemental>();
            foreach (var fortress in _fortress)
            {
                if (fortress.stoneCourtyard)
                {
                    fortress.Integrate();
                }
            }
            LeanTween.moveLocalY(TheClaw.instance.gameObject, 15f, 2f);
            _playerController._fixedStoneCourtyard = true;
        }
        
        Destroy(holo);
    }

    public void EnableMapViewfunc()
    {
        _map.interactable = true;
        _mapIcon.sprite = _mapSprite;
        _map.onClick.AddListener(FullMapView);
    }

    public void FullMapView()
    {
        _mapFullView.SetActive(!_mapEnabled);
        _mapFullViewCamera.gameObject.SetActive(!_mapEnabled);
        _mapEnabled = !_mapEnabled;
        DisablePanel();
    }

    public void DisablePanel()
    {
        //Time.timeScale = 1f;
        _playerController.pause = false;
        _grimoirePanel.SetActive(false);
    }
    public void EnablePanel()
    {
        //Time.timeScale = 0;
        _playerController.pause = true;
        _grimoirePanel.SetActive(true);
    }

    public void EnableAdacodeCryptex()
    {
        _adacodeIcon.sprite = _scroll;
        _adacodeCryptex.interactable = true;
    }

    private void AdacodeCryptex()
    {
        _playerController.DisableDarkness();
        _adacodeCryptex.interactable = false;
        DisablePanel();
        TryGetComponent<ButtonPingPongAnimations>(out ButtonPingPongAnimations pingpongAnim);
        if (pingpongAnim != null)
        {
            Destroy(pingpongAnim);
        }
    }

    public void EnterMiniGameWithCryptex()
    {
        _adacodeCryptex.onClick.RemoveAllListeners();
        //_adacodeCryptex.interactable = true;
        _adacodeCryptex.onClick.AddListener(EnterMiniGame);
    }

    private void EnterMiniGame()
    {
        MiniGameManager._instance.InitTPEffect();
        StartCoroutine(TeleportPlayer());
        _adacodeCryptex.interactable = false;
        DisablePanel();
        TryGetComponent<ButtonPingPongAnimations>(out ButtonPingPongAnimations pingpongAnim);
        if (pingpongAnim != null)
        {
            Destroy(pingpongAnim);
        }
    }

    public void RemoveMagicFromOrbs()
    { 
        _adacodeCryptex.onClick.RemoveAllListeners();
        _adacodeCryptex.interactable = true;
        _adacodeCryptex.onClick.AddListener(()=>OrbsOfKinesis.instance.RemoveMagic(_adacodeCryptex, _grimoirePanel));
        DisablePanel();
    }

    private IEnumerator DisablePanelAfterDuration(float val)
    {
        yield return new WaitForSeconds(val);
        Time.timeScale = 1f;
        _grimoirePanel.SetActive(false);
    }

    IEnumerator TeleportPlayer()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        //isTeleporting = true;
        int length = MiniGameManager._instance.spawnPoints.Length;
        int index = Random.Range(0, length);
        //Debug.LogError($"TP at {index}");
        float teleportDuration = 1.0f;
        Vector3 initialPosition = transform.position;
        Transform targetSpawnPoint = MiniGameManager._instance.spawnPoints[index].transform;
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

    public void BringForthClaw()
    {
        //Debug.LogError("BringingForthClaw");
        if(!_insideTornadoRange)
        {
            //Debug.LogError("BringingForthClaw");
            if (_playerController != null && clawRef == null)
            {
               //Debug.LogError($"BringingForthClaw {_playerController._collectedClaw}  and {_clawInstantiated}");
                if (_playerController._collectedClaw && !_clawInstantiated)
                {
                    //Debug.LogError("BringingForthClaw");
                    _clawInstantiated = true;
                    clawRef = Instantiate(_claw, clawSpts.transform.position, quaternion.identity);
                    DisablePanel();
                    if (!_strippedDarkMagic)
                    {
                        //Debug.LogError("BringingForthClaw");
                        StartCoroutine(FreezeClawEmergingPower());
                    }
                    _strippedDarkMagic = true;
                }
                else if (_playerController._collectedClaw && !_clawInstantiated && _strippedDarkMagic)
                {
                    _clawInstantiated = true;
                    clawRef = Instantiate(_clawNormal, clawSpts.transform.position, quaternion.identity);
                    DisablePanel();
                }
            }
            else if (clawRef != null && clawRef.activeSelf)
            {
                Destroy(clawRef.gameObject);
                clawRef = null;
                _clawInstantiated = false;
                DisablePanel();
            }
        }
        else if (_insideTornadoRange)
        {
            //Debug.LogError("BringingForthClaw");
            if (_enabledWeaponAura)
            {
                if (_playerController != null && clawRef == null)
                {
                    //Debug.LogError($"BringingForthClaw {_playerController._collectedClaw}  and {_clawInstantiated}");
                    if (_playerController._collectedClaw && !_clawInstantiated)
                    {
                        //Debug.LogError("BringingForthClaw");
                        _clawInstantiated = true;
                        clawRef = Instantiate(_claw, clawSpts.transform.position, quaternion.identity);
                        DisablePanel();
                        if (!_strippedDarkMagic)
                        {
                            //Debug.LogError("BringingForthClaw");
                            StartCoroutine(FreezeClawEmergingPower());
                        }
                        _strippedDarkMagic = true;
                    }
                    else if (_playerController._collectedClaw && !_clawInstantiated && _strippedDarkMagic)
                    {
                        _clawInstantiated = true;
                        clawRef = Instantiate(_clawNormal, clawSpts.transform.position, quaternion.identity);
                        DisablePanel();
                    }
                }
                else if (clawRef != null && clawRef.activeSelf)
                {
                    Destroy(clawRef.gameObject);
                    clawRef = null;
                    _clawInstantiated = false;
                    DisablePanel();
                }
            }
            else if(_insideTornadoRange && ! _enabledWeaponAura)
            {
                //Debug.LogError("BringingForthClaw");
                _playerController.EnableWeaponShadowAura();
                _enabledWeaponAura = true;  
                DisablePanel();
            }
           
        }
        TryGetComponent<ButtonPingPongAnimations>(out ButtonPingPongAnimations pingpongAnim);
        if (pingpongAnim != null)
        {
            Destroy(pingpongAnim);
        }
    }

    private IEnumerator FreezeClawEmergingPower()
    {
        GetComponent<PlayerTimelineManager>().FreezeEnergyFromClaw();
        yield return new WaitForSeconds(10);
        EnablePanel();
        _FreezeClaw.gameObject.AddComponent<ButtonPingPongAnimations>();
        _FreezeClaw.interactable = true;
        _FreezeClawIcon.sprite = _FreezeClawSprite;
        yield return null;
    }

    public void TurnToEmitters()
    {
        EmitterOrbsManager.instance.TurToEmitters();
        _turnToEmitter.interactable = false;
        DisablePanel();
        TryGetComponent<ButtonPingPongAnimations>(out ButtonPingPongAnimations pingpongAnim);
        if (pingpongAnim != null)
        {
            Destroy(pingpongAnim);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(_clawsFreezed && other.gameObject.CompareTag(MapPointsWeekThree.PlayersClaw) && !_extractedEnergy)
        {
            _extractedEnergy = true;
            PvPPlayerUI.instance._collect.gameObject.SetActive(true);
            PvPPlayerUI.instance._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Extract Energy";
            PvPPlayerUI.instance._collect.onClick.RemoveAllListeners();
            PvPPlayerUI.instance._collect.onClick.AddListener(() =>
            {
                LeanTween.scale(TheClaw.instance._freeze, new Vector3(0.02f, 0.02f, 0.02f), 1f);
                PvPPlayerUI.instance._collect.gameObject.SetActive(false);
                _playerController._mysticalPower.SetActive(true);
                TheClaw.instance._energyConsume.SetActive(true);
                GetComponent<PlayerTimelineManager>().StoreClaw();
            });
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_clawsFreezed && other.gameObject.CompareTag(MapPointsWeekThree.PlayersClaw) && !_extractedEnergy)
        {
            _extractedEnergy = true;
            PvPPlayerUI.instance._collect.gameObject.SetActive(true);
            PvPPlayerUI.instance._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Extract Energy";
            PvPPlayerUI.instance._collect.onClick.RemoveAllListeners();
            PvPPlayerUI.instance._collect.onClick.AddListener(() =>
            {
                LeanTween.scale(TheClaw.instance._freeze, new Vector3(0.02f, 0.02f, 0.02f), 1f);
                PvPPlayerUI.instance._collect.gameObject.SetActive(false);
                _playerController._mysticalPower.SetActive(true);
                TheClaw.instance._energyConsume.SetActive(true);
                GetComponent<PlayerTimelineManager>().StoreClaw();
            });
        }
    }
}
