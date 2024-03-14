using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GeneralCharacterManager : MonoBehaviour
{
    public enum CharacterType
    {
        Celorian,
        CelorianTypeTwo,
        CelorianTypeThree,
        TypeTwo, 
        Mira,
        DropletTrail,
        Arturo
    }
    public enum State
    {
        Idle,
        Chase,
        Attack
    }

    [Tooltip("1. Celorian is just ghosts wandering around." +
        "\n2. Celorian Two is just ghosts wandering around and can hide ghost inside." +
        "\n3. Celorian Three contains information.")]
    public CharacterType _characterType;
    public State _state;

    public IntrantThirdPersonController _playerRef;
    public Animator _animator;
    public GameObject _enemySpts;
    public GameObject smokeSpts;
    public GameObject smokePrefab;
    public GameObject _smokeRef;
    public GameObject _arrow;
    public GameObject _arrowSpts;
    public Transform target;
    public GameObject _informationPanel;
    public GameObject[] _enemyPrefabs; 


    public float moveSpeed = 2.0f;
    public float smoothRotationSpeed = 5.0f;  // Adjust this value to control the rotation speed.
    public CharacterController controller;
    public Vector3 moveDirection = Vector3.forward;
    public LayerMask avoidlayers;

    public bool _turnToStone;
    public bool _reachedDestination;
    public bool _turnedToStone;
    public bool _pause = false;
    public bool _collectedInformation = false;
    public bool _foundThrowingStar = false;


    public int maxCamouflaged = 1;
    public int localMonsterCount = 0;
    public float stoppingDistance = 1f;


    public Material _stoneMaterial;
    public SkinnedMeshRenderer _meshRenderer;

    // Add a variable to control the rotation
    private int rotationAngle = 180;

    public static Action<GameObject> _turnedtoStoneAction;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();   

        #region Mira
        if (_characterType == CharacterType.Mira)
        {
            ManualTag []_mapTriggers = FindObjectsOfType<ManualTag>();
            foreach (var mapTrigger in _mapTriggers)
            {
                if (mapTrigger._tag == Tag.MirasTarget)
                {
                    target = mapTrigger.gameObject.transform;
                }
            }
        }
        #endregion

        #region DroplteTrail

        if (_characterType == CharacterType.DropletTrail)
        {
            WeekFourTriggerManager []_mapTriggers = FindObjectsOfType<WeekFourTriggerManager>();
            foreach (var mapTrigger in _mapTriggers)
            {
                if (mapTrigger.mapPoint == WeekFourTriggerManager.MapPoint.DropletTarget)
                {
                    target = mapTrigger.gameObject.transform;
                }
            }
        }

        #endregion

        #region Celorian
        if (_characterType == CharacterType.Celorian || _characterType == CharacterType.CelorianTypeTwo)
        {
            _informationPanel.SetActive(false);
        }
        #endregion
    }

    private void LateUpdate()
    {
        #region Celorian
        if (_characterType == CharacterType.Celorian || _characterType == CharacterType.CelorianTypeTwo || _characterType == CharacterType.CelorianTypeThree && !_pause && !_turnedToStone)
        {
            MoveCelorian();
        }

        if (_turnToStone && !_turnedToStone)
        {
            TurnToStone();
        }
        #endregion

        #region Celorian
        _playerRef = IntrantThirdPersonController.instance;
        _foundThrowingStar = _playerRef._foundThrowingStar;
        if (_characterType == CharacterType.Mira && _foundThrowingStar) {
            MoveMira();
        }
        #endregion

        #region Droplet trail

        if(_characterType == CharacterType.DropletTrail)
        {
            if (target != null)
            {
                NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
                if (navMeshAgent != null)
                {
                    // Set the destination for the NavMeshAgent
                    if (Vector3.Distance(transform.position, _playerRef.transform.position) > 5f)
                    {
                        navMeshAgent.isStopped = true;
                        _state = State.Idle;
                        //Debug.LogError("Droplet is not range");
                    }
                    else if (Vector3.Distance(transform.position, _playerRef.transform.position) < 5f)
                    {
                        if (Vector3.Distance(transform.position, target.position) < stoppingDistance)
                        {
                            //Debug.LogError("Droplet reached target");
                            navMeshAgent.isStopped = true;
                            _state = State.Idle;
                            LeanTween.scale(gameObject, new Vector3(0, 0, 0), 1f);
                            Destroy(gameObject, 1);
                        }
                        else
                        {
                            //Debug.LogError("Droplte chasing");
                            navMeshAgent.isStopped = false;
                            _state = State.Chase;
                            navMeshAgent.SetDestination(target.position);
                        }
                    }
                }
                else
                {
                    //Debug.LogError("NavMeshAgent component not found on this object.");
                }
            }
            else
            {
                //Debug.LogError("Target is null. Please assign a valid target.");
            }
        }
        #endregion
    }



    private void MoveMira()
    {
        if (target != null)
        {
            NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.stoppingDistance = stoppingDistance;

            if (navMeshAgent != null)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, _playerRef.transform.position);

                if (distanceToPlayer > 7f)
                {
                    navMeshAgent.isStopped = true;
                    _state = State.Idle;
                    _animator.SetBool("IsWalking", false);
                    //Debug.LogError("Mira player is out of range");
                }
                else
                {
                    // Player is in range
                    if (distanceToPlayer < stoppingDistance && !_reachedDestination)
                    {
                        //Debug.LogError("Mira reached target");
                        navMeshAgent.isStopped = true;
                        _state = State.Idle;
                        _animator.SetBool("IsWalking", false);
                        _reachedDestination = true;
                    }
                    else if (!_reachedDestination)
                    {
                        //Debug.LogError("Mira chasing");
                        navMeshAgent.isStopped = false;
                        _state = State.Chase;
                        _animator.SetBool("IsWalking", true);
                        navMeshAgent.SetDestination(target.position);
                    }
                }
            }
            else
            {
                //Debug.LogError("NavMeshAgent component not found on this object.");
            }
        }
        else
        {
            //Debug.LogError("Target is null. Please assign a valid target.");
        }
    }

    public void TurnToStone()
    {
        controller.enabled = false;
        _animator.enabled = false;
        _meshRenderer.material = _stoneMaterial;
        _turnedToStone = true;
        _turnedtoStoneAction?.Invoke(gameObject);
    }
    
    private void MoveCelorian()
    {
        // Perform a raycast to check for obstacles
        Vector3 origin = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        RaycastHit hit;
        Ray ray = new Ray(origin, moveDirection);
        Debug.DrawRay(origin, moveDirection, Color.green);
        if (Physics.Raycast(ray, out hit, 1.5f, avoidlayers))
        {
            // If an obstacle is hit, rotate by 180 degrees
            transform.Rotate(Vector3.up, rotationAngle);

            moveDirection = moveDirection == Vector3.back ? Vector3.forward : Vector3.back;
        }
        // Move the character using the Character Controller
        controller.SimpleMove(moveDirection * moveSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        #region Celorian
        if (_characterType == CharacterType.CelorianTypeTwo && other.gameObject.CompareTag(Tags.PLAYER_TAG) && localMonsterCount <= maxCamouflaged && !_turnedToStone)
        {
            _pause = true;
            GameObject enemy = Instantiate(_enemyPrefabs[UnityEngine.Random.Range(0, _enemyPrefabs.Length)], _enemySpts.transform.position, Quaternion.identity);
            enemy.GetComponent<MonsterMovementController>()._radiusRange = 25;
            Destroy(this.gameObject);
            localMonsterCount++;
        }else if (_characterType == CharacterType.CelorianTypeThree && other.gameObject.CompareTag(Tags.PLAYER_TAG))
        {
            _pause = true;
        }
        #endregion
    }

    private void OnTriggerExit(Collider other)
    {
        #region Celorian
        if (_characterType == CharacterType.CelorianTypeTwo && other.gameObject.CompareTag(Tags.PLAYER_TAG))
        {
            _pause = false;
        }
        else if (_characterType == CharacterType.CelorianTypeThree && other.gameObject.CompareTag(Tags.PLAYER_TAG))
        {
            _pause = false;
        }
        #endregion
    }

    private void Pause(bool val)
    {
        if(val)
        {
            _pause = val;
        }else if (!val)
        {
            _pause = val;
        }
    }

    public void KaireEffect()
    {
        _smokeRef = Instantiate(smokePrefab, smokeSpts.transform.position, Quaternion.identity);
    }

    public void RassignDestination(GameObject _target)
    {
        NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.isStopped = false;
        _state = State.Chase;
        _animator.SetBool("IsWalking", true);
        navMeshAgent.SetDestination(_target.transform.position);
    }
}
