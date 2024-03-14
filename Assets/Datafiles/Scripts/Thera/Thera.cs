using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum state
{
    Idle,
    walk   
}

public class Thera : TheraBehaviour
{
    public static Thera instance;
    public state state;
    public Animator _theraAnimator;
    public GameObject _theraBody;
    public GameObject _theraSpawnVFX;
    public GameObject _therareviveVFX;
    public GameObject _summonVFX;
    public Texture emissionTexture;
    public MeshRenderer []_bodyMesh;
    public SkinnedMeshRenderer []_bodySkinnedMesh;
    public Light []_lights; 
    public GameObject []shutdownObjects;
    public GameObject []rasVeusSpawnPoint;
    public TheraHealthManager healthManager;


    private NavMeshAgent agent;
    private Rigidbody _rb;

    [Header("Player Reference")]
    public GameObject _playerRef;
    public GameObject target;
    public GameObject _rasVeusRef;

    public int stoppingDistance = 3;

    [Header("Bool")]
    public bool _isMoving = false;
    public bool _gotPlayer = false;
    public bool  _shutDownInit = false;
    public bool  _broken = false;
    public bool  _armored = false;
    
    private void Start()
    {
        instance = this;
        StartCoroutine(Init());
        agent = GetComponent<NavMeshAgent>();
        _theraAnimator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        healthManager = GetComponent<TheraHealthManager>();
        target = _playerRef.GetComponent<PlayerTheraConnectionManager>()._spawnPoints[Random.Range(0, _playerRef.GetComponent<PlayerTheraConnectionManager>()._spawnPoints.Length)];
    }
    private void FixedUpdate()
    {
        if (_broken)
        {
            _broken = false;
            int index = Random.Range(0, 2);
            Shutdown(shutdownObjects, agent);
            healthManager.HealthUI_Canvas.gameObject.SetActive(false);
        }

        if (_shutDownInit && _rasVeusRef == null)
        {
            Debug.LogError("ShutdownInitiating");
            _shutDownInit = false;
            int index = Random.Range(0, 2);
            Shutdown(shutdownObjects, agent);
            Vector3 rot = new Vector3(-90, 0, 0);
            _rasVeusRef = Instantiate(_playerRef.GetComponent<IntrantThirdPersonController>()._rasVeus,
                rasVeusSpawnPoint[index].transform.position, Quaternion.Euler(-90, 0, 0));
            GetComponent<TheraHealthManager>().health_UI.gameObject.SetActive(false);
            StartCoroutine(ReviveWithDelay(20));
        }

        if (_playerRef != null &&
            Vector3.Distance(transform.position, target.transform.position) > stoppingDistance && !shutdown)
        {
            state = state.walk;
            _isMoving = true;
        }
        else if (Vector3.Distance(transform.position, target.transform.position) < stoppingDistance)
        {
            state = state.Idle;
            _isMoving = false;
        }

        Move();
    }

    private void Move()
    {
       if (state == state.walk && !shutdown)
       {
           StartCoroutine(Walk());
       }
       else if (state == state.Idle)
       {
           Idle();
       }
    }

    private void Idle()
    {
        _theraAnimator.SetBool("Walking", _isMoving);
        agent.isStopped = true;
        _rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private IEnumerator Walk()
    {
        _theraAnimator.SetBool("Walking", _isMoving);
        _rb.constraints = RigidbodyConstraints.None;
        yield return new WaitForSeconds(0.29f);
        _isMoving = true;
        agent.isStopped = false;
        agent.SetDestination(_playerRef.transform.position);
        // Calculate the rotation needed to look at the target
        Quaternion targetRotation = Quaternion.LookRotation(_playerRef.transform.position - transform.position);
        // Smoothly interpolate the current rotation towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2 * Time.deltaTime);
    }

    IEnumerator Init()
    {
        _theraBody.SetActive(false);
        yield return new WaitForSeconds(2.5f);
        _theraBody.SetActive(true);
        _theraSpawnVFX.SetActive(false);
    }

    public IEnumerator Summon()
    {
        _summonVFX.SetActive(true);
        yield return new WaitForSeconds(3f);
        healthManager.HealthUI_Canvas.SetActive(true);
        _summonVFX.SetActive(false);
    }

    public void ReviveTheraWithDelay()
    {
        StartCoroutine(ReviveWithDelay(0));
    }

   IEnumerator ReviveWithDelay(int val)
   {
       Debug.LogError("Reviving thera after delay");
       yield return new WaitForSeconds(val);
       Debug.LogError("Revived thera");
       if (!_armored)
       {
           Reactivate(shutdownObjects, agent);
       }
       GetComponent<TheraHealthManager>().health_UI.gameObject.SetActive(true);
        if (_rasVeusRef != null)
        {
            Destroy(_rasVeusRef);
        }
   }

    public void ArmorThera()
    {
        _armored = true;
        foreach (var light in _lights)
        {
            light.gameObject.SetActive(false);
        }
        //Make sure to enable the Keywords
        foreach(var mesh in _bodyMesh)
        {
            mesh.material.SetTexture("_MainTex", null);
            mesh.material.EnableKeyword("_EMISSION");

            // Set emission texture
            mesh.material.SetTexture("_EmissionMap", emissionTexture);
            mesh.material.color = Color.grey;
        }

        foreach (var mesh in _bodySkinnedMesh)
        {
            mesh.material.SetTexture("_MainTex", null);
            mesh.material.EnableKeyword("_EMISSION");

            // Set emission texture
            mesh.material.SetTexture("_EmissionMap", emissionTexture);
            mesh.material.color = Color.grey;
        }
    }
}
