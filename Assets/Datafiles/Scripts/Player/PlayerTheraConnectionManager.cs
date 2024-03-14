using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerTheraConnectionManager : MonoBehaviour
{
    public static PlayerTheraConnectionManager instance;

    [Header("Thera")]
    [SerializeField]
    public GameObject _theraPrefab;
    public GameObject _theraRef;

    [Header("SpawnPoints")]
    public GameObject[] _spawnPoints;

    public float theraClosestDist = 13;

    [Header("Bools")]
    public bool _theraInstantiated = false;
    public bool _theraActivated = true;


    void Start()
    {
        instance = this;
        IntrantThirdPersonController _player = GetComponent<IntrantThirdPersonController>();

        #region Thera will be there along with player or not
        if (_player._map == MAP.WEEK2 || _player._map == MAP.WEEK3 || _player._map == MAP.WEEK5)
        {
            _theraInstantiated = true;
            _theraRef = Instantiate(_theraPrefab,
                _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Length)].transform.position,
                Quaternion.identity);
            _theraActivated = true;
            _theraRef.GetComponent<Thera>()._playerRef = this.gameObject;
            PvPPlayerUI _playerUI = GetComponent<PvPPlayerUI>();
            _playerUI._realeaseThera.onClick.RemoveAllListeners();
            _playerUI._realeaseThera.interactable = true;
            _playerUI._realeaseThera.onClick.AddListener(() => {
                ReleaseThera();
            });
        }
        else if(_player._map == MAP.WEEK4)
        {
            _theraInstantiated = true;
            WeekFourTriggerManager[] _triggerManager = FindObjectsOfType<WeekFourTriggerManager>();
            foreach (var trigger in _triggerManager)
            {
                if(trigger.mapPoint == WeekFourTriggerManager.MapPoint.TheraSpawnPoint)
                {
                    _theraRef = Instantiate(_theraPrefab, trigger.transform.position, Quaternion.Euler(0, -90, 0));
                    _theraActivated = true;
                    _theraRef.GetComponent<Thera>()._playerRef = this.gameObject;
                    _theraRef.GetComponent<Thera>()._broken = true;
                }
            }
            PvPPlayerUI _playerUI = GetComponent<PvPPlayerUI>();
            _playerUI._realeaseThera.onClick.RemoveAllListeners();
            _playerUI._realeaseThera.interactable = true;
            _playerUI._realeaseThera.onClick.AddListener(() => {
                ReleaseThera();
            });
        }
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        if(_theraRef == null){return;}
        if(_theraRef.GetComponent<TheraHealthManager>().Health < 0)
        {
            GetComponent<PlayerTimelineManager>()._theraDied = true;
        }
        #region TheraTeleport
        if (_theraRef != null && Vector3.Distance(transform.position, _theraRef.transform.position) >= theraClosestDist && !_theraRef.GetComponent<Thera>()._shutDownInit 
            && GetComponent<IntrantThirdPersonController>()._map != MAP.WEEK4 && !_theraRef.GetComponent<Thera>().shutdown
            && !_theraRef.GetComponent<TheraHealthManager>().playerDied)
        {
            _theraRef.SetActive(false);
            Vector3 randomOffset = Random.insideUnitSphere * 3f; // Adjust the radius as needed
            _theraRef.transform.position = transform.position + randomOffset;
            _theraRef.SetActive(true);
        }else if(_theraRef != null && !_theraRef.GetComponent<Thera>().shutdown && Vector3.Distance(transform.position, _theraRef.transform.position) >= theraClosestDist && GetComponent<IntrantThirdPersonController>()._map == MAP.WEEK4)
        {
            if (_theraRef.GetComponent<Thera>()._armored)
            {
                _theraRef.SetActive(false);

                Vector3 randomOffset = Random.insideUnitSphere * 3f; // Adjust the radius as needed
                Vector3 newPosition = transform.position + randomOffset;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(newPosition, out hit, 10f, NavMesh.AllAreas))
                {
                    _theraRef.transform.position = hit.position;
                    _theraRef.GetComponent<NavMeshAgent>().ResetPath();
                }

                _theraRef.SetActive(true);
            }
        }
        #endregion
    }

    public void ReleaseThera()
    {
        Debug.LogError("Release thera... " + _theraActivated + _theraInstantiated);
        if (!_theraInstantiated && _theraRef == null)
        {
            _theraInstantiated = true;
            _theraRef = Instantiate(_theraPrefab,
                _spawnPoints[Random.Range(0, _spawnPoints.Length)].transform.position,
                Quaternion.identity);
            _theraActivated = true;
            _theraRef.GetComponent<Thera>()._playerRef = this.gameObject;
            IntrantThirdPersonController.instance.Resume();
        }
        else if (_theraInstantiated || _theraRef != null)
        {
            IntrantThirdPersonController.instance.Resume();
            _theraRef.GetComponent<Thera>().ReviveTheraWithDelay();
            StartCoroutine(SummonVFXCoroutine());
        }
    }
    IEnumerator SummonVFXCoroutine()
    {
        _theraActivated = !_theraActivated;
        if (_theraActivated == false)
        {
            Debug.LogError("Thera JustActivating");
            _theraRef.GetComponent<Thera>()._theraBody.SetActive(_theraActivated);
            yield return null;
        }
        else
        {
            Debug.LogError("Thera Pos JustActivating");
            _theraRef.GetComponent<Thera>()._theraBody.SetActive(_theraActivated);
            _theraRef.GetComponent<Rigidbody>().isKinematic = true;
            _theraRef.GetComponent<NavMeshAgent>().enabled = false;
            _theraRef.transform.position = _spawnPoints[Random.Range(0, _spawnPoints.Length)].transform.position;
            _theraRef.GetComponent<NavMeshAgent>().enabled = true;
            _theraRef.GetComponent<Rigidbody>().isKinematic = false;
            StartCoroutine(_theraRef.GetComponent<Thera>().Summon());
            _theraRef.GetComponent<Thera>()._theraBody.SetActive(false);
            yield return new WaitForSeconds(3f);
            _theraRef.GetComponent<Thera>()._theraBody.SetActive(_theraActivated);
            //_theraRef.transform.position = _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Length)].transform.position;
        }
    }
}
