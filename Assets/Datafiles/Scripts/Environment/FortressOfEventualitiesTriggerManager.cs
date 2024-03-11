using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class FortressOfEventualitiesTriggerManager : MonoBehaviour
{
    public static FortressOfEventualitiesTriggerManager instance;

    [Header("Player Ref")]
    public GameObject _playerRef;

    [Header("Animations")]
    [SerializeField]
    private Animator _doorAnimation;

    [Header("Playable director")]
    [SerializeField]
    private PlayableDirector _kaireGlimpse;


    [Header("kair Sector")]
    public GameObject _kair;
    public GameObject _kairHolder;
    public GameObject _portal;

    [Header("Alkemanna hotspot")]
    public GameObject _theraBox;
    public GameObject _theraCode;
    public GameObject _fractureRock;
    public GameObject _AlkemanaHotspotCheckpointFx;


    [Header("Breakable wall")]
    public MeshCollider _pointCWallCollider;

    [Header("Asimana Codex")]
    public GameObject _asimanaCodexPrefab;

    [Header("Power Circuit Fx")]
    public GameObject _powerCircuitVfx;

    [Header("Kaire")]
    public GameObject _kaire;

    public Transform _kaireSpawnPoint;

    [Header("Kaire")]
    public GameObject _MainframeVFX;

    [Header("Asimana codex Spawn Points")]
    public GameObject []_asimanaCodexSpawnPoints;

    [Header("Bools")]
    public bool _theraCodePlaced = false;


    private void Start()
    {
        instance = this;

        if (_kaireGlimpse != null)
            _kaireGlimpse.Stop();
        if (_kairHolder != null)
            _kairHolder.SetActive(false);
        if (_portal != null)
            _portal.SetActive(false);
    }


    private void LateUpdate()
    {
        if (_theraCodePlaced)
        {
            StartCoroutine(DisintegrateTheraBoxandShowCode());
            _theraCodePlaced = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IntrantThirdPersonController>(out IntrantThirdPersonController _player))
        {
            Debug.LogError("Entered in alkemia..");
            _playerRef = other.gameObject;
            #region Alkemia Chamber

            if (_doorAnimation != null && this.gameObject.CompareTag(MapPoints.PointC))
            {
                Debug.LogError("Closing gate.."); 
                GetComponent<BoxCollider>().enabled = false; 
                _player._beenInAlkemiaChamber = true;
                StartCoroutine(ResumeGame(20));
                _doorAnimation.SetBool("Closed", true);
                Debug.LogError("Closed gate..");
            }

            #endregion

            #region Kaire Glimpse
            if (_kaireGlimpse != null)
            {
                _kaireGlimpse.SetGenericBinding(_kaireGlimpse.playableAsset, _kairHolder);
                // Play the specific playable
                _kaireGlimpse.Play();
                Invoke("StopTimeline", 3f);
                Invoke("Disable", 1.5f);
            }
            #endregion

            #region TheraBox
            if (this.gameObject.CompareTag(Tags.THERABOX_TAG))
            {
                _player._receivedTheraBox = true;
                Debug.Log("ReceivedTheraBox");
                Invoke("Disable", 0.5f);
                Invoke("DisableMesh", 0.5f);
            }
            #endregion

            #region TheraCode
            if (_theraBox != null && _player._receivedTheraBox)
            {
                PvPPlayerUI.instance._place.gameObject.SetActive(true);
                _player._checkPointFx = _AlkemanaHotspotCheckpointFx;
                PvPPlayerUI.instance._place.onClick.AddListener(() => { ShowTheraBox(); });
            }
            #endregion

/*            #region PointC
            if (this.gameObject.CompareTag(MapPoints.PointCExitDown) || this.gameObject.CompareTag(MapPoints.PointCExitUp))
            {
                StartCoroutine(ResumeGame(10));
                Invoke("Disable", 1f);
            }
            #endregion*/

            #region Asimanas codex
            if (_asimanaCodexPrefab!=null && IntrantThirdPersonController.instance._placedTheraCode)
            {
                int index = UnityEngine.Random.Range(0, _asimanaCodexSpawnPoints.Length);
                GameObject _asimanaCodex = Instantiate(_asimanaCodexPrefab, _asimanaCodexSpawnPoints[index].transform.position, Quaternion.identity);
                Invoke("Disable", 0.1f);
            }
            #endregion

            #region Kaire

            if (this.gameObject.CompareTag(Tags.KaireTrigger_TAG) && other.TryGetComponent<IntrantThirdPersonController>(out IntrantThirdPersonController player))
            {
                if (player._receivedAsimanaCodex && player.GetComponent<PlayerTheraConnectionManager>()._theraRef.activeSelf)
                {
                    Instantiate(_kaire, _kaireSpawnPoint.position, Quaternion.identity);
                    Invoke("Disable",0.2f);
                }
                
            }

            #endregion

            #region MainFrame
            if (this.gameObject.CompareTag(Tags.FINALSEQUENCE_TAG) && _player._receivedAsimanaCodex)
            {
                PvPPlayerUI.instance._chargeMainFrame.onClick.AddListener(() => { 
                    _MainframeVFX.gameObject.SetActive(true);
                    this.gameObject.GetComponent<Collider>().enabled = false;
                    _player.Resume();
                    _player._mainFrameEnabled = true;
                });
            }
            #endregion
        }
    }

    private void Disable()
    {
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
    }
    private void DisableMesh()
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
    
    private void StopTimeline()
    {
        _kaireGlimpse.Stop();
    }

    public void ShowTheraBox()
    {
        _theraBox.SetActive(true);
        _theraCodePlaced = true;
        PvPPlayerUI.instance._place.gameObject.SetActive(false);
    }

    IEnumerator DisintegrateTheraBoxandShowCode()
    {
        yield return new WaitForSeconds(5f);
        try
        {
            _fractureRock.SetActive(true);
            _fractureRock.GetComponent<Fracture>().FractureObject();
            _theraBox.gameObject.SetActive(false);
            _theraCode.SetActive(true);
            PvPPlayerUI.instance._collect.gameObject.SetActive(true);
            PvPPlayerUI.instance._collect.onClick.RemoveAllListeners();
            PvPPlayerUI.instance._collect.onClick.AddListener(() => { CollectCode(); });
            _theraCode.SetActive(true);
            Invoke("Disable", 0.5f);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    private void CollectCode()
    {
        _playerRef.gameObject.GetComponent<IntrantThirdPersonController>()._receivedTheraCode = true;
        _playerRef.gameObject.GetComponent<IntrantThirdPersonController>()._collectedTherCode++;
        _theraCode.SetActive(false);
        PvPPlayerUI.instance._collect.gameObject.SetActive(false);
    }

    IEnumerator ResumeGame(int _pauseTime)
    {
        Debug.Log("Pausing...");
        IntrantThirdPersonController.instance.pause = true;
        IntrantThirdPersonController.instance._isAnimating = true;
        yield return new WaitForSeconds(_pauseTime);
        IntrantThirdPersonController.instance.pause = false;
        IntrantThirdPersonController.instance._isAnimating = false;
        Debug.Log("Resume...");
    }
}
