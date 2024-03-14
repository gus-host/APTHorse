using System;
using System.Collections;
using UnityEngine;

public enum MonsterState
{
    IDLE,
    CHASE,
    ATTACK,
    DEAD
}

public enum MonsterType
{
    NORMAL,
    FERRAPTOR,
    ShadowMonster,
    Arturo,
    FARRAPTORBOSS,
    DestructableObject
}

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class MonsterMovementController : MonoBehaviour
{
    public MonsterType _monsterType;
    public LayerMask shieldLayer;
    public FrontRadar _frontRadar;

    public UnityEngine.AI.NavMeshAgent navMeshAgent;
    public Transform target = null; // The player's transform
    public MonsterState _currentState;

    public GameObject _minimapPointer;
    public GameObject _arrow;
    public GameObject _arrowSpts;
    public GameObject _dieFx;
    public Fracture _fracture;


    public MonsterLimb[] _monsterLimbs;
    //Characterstics
    public float stoppingDistance = 2f;
    public int _radiusRange = 3;
    public float damageToDeal = 1f;
    public float arturoArrowDamage = 1f;
    public float arturoRange = 5f;
    public float ferraptorAttackingDist = 5f;
    private Vector3 _startingPoint;

    //Components.
    private MonsterAnimationController _animationController;
    private MonsterHealthController _monsterHealthController;

    //Animation cooldown
    private float _attackOneCooldown = 3.45f;
    private float _attackTwoCooldown = 3.20f;
    private float _attackThreeCooldown = 4.37f;
    private bool _isOnCooldown = false;
    public bool findEnemies = false;
    public bool _shooting = false;
    public bool _died = false;
    public bool obstacleInBetween = false;
    public bool enemyStopped = false;

    public Animator[] _dissolveAnimator;
    public GameObject[] _dissolve;

    private void Awake()
    {
        if (_monsterType == MonsterType.NORMAL)
        {
            _monsterLimbs = GetComponentsInChildren<MonsterLimb>();
        }
        if (findEnemies)
        {
            target = FindObjectOfType<IntrantThirdPersonController>().transform;
        }
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>(); //Set state to Idle in the beginning
        _currentState = MonsterState.IDLE;
        //Set reference for components
        _animationController = GetComponent<MonsterAnimationController>();
        _monsterHealthController = GetComponent<MonsterHealthController>();
    }

    private void Start()
    {
        FrontRadar._stopBroadcast += ObstacleHit;
        if (_monsterType == MonsterType.NORMAL)
        {
            #region Disable colliders
            foreach (MonsterLimb monsterLimb in _monsterLimbs)
            {
                monsterLimb.GetComponent<BoxCollider>().enabled = false;
            }
            #endregion
        }
        foreach (var anim in _dissolveAnimator)
        {
            anim.enabled = false;
        }
        //_dissolveAnimator.enabled = false;
        _startingPoint = transform.position;
        GetComponent<MonsterHealthController>().OnMonsterDie += MonsterDied;
    }

    private void ObstacleHit(bool obj)
    {
        obstacleInBetween = obj;
    }

    private void MonsterDied()
    {

        # region Case for destructible object
        if (_monsterType == MonsterType.DestructableObject)
        {
            _fracture.FractureObject();
            Destroy(gameObject);
            return;
        }
        #endregion


        _currentState = MonsterState.DEAD;
        _animationController.SetAnimationParameter("isDead");
        navMeshAgent.enabled = false;
        Rigidbody _rigidbody = gameObject.AddComponent<Rigidbody>();
        if (_rigidbody != null)
        {
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            _rigidbody.mass = 100;
        }
        navMeshAgent.enabled = false;
        foreach (var anim in _dissolveAnimator)
        {
            anim.enabled = true;
        }
        _died = true;
        GameObject _dieFxInstance = Instantiate(_dieFx, transform.position, Quaternion.Euler(-90, 0, 0));
        Destroy(_dieFxInstance, 10f);
        Destroy(_animationController);
        Destroy(gameObject, 1.11f);
    }

    private void FixedUpdate()
    {
        if(_monsterType == MonsterType.DestructableObject)
        {
            return;
        }
        if (target == null)
        {
            target = FindObjectOfType<IntrantThirdPersonController>().transform;
        }

        try
        {
            if (target.GetComponent<IntrantThirdPersonController>().pause)
            {
                _currentState = MonsterState.IDLE;
                _animationController.SetAnimationParameter("isWalk", false);
                _animationController.SetAnimationParameter("Idle", true);
                navMeshAgent.isStopped = true;
                return;
            }
            else if (target.GetComponent<IntrantThirdPersonController>().pause)
            {
                _currentState = MonsterState.IDLE;
                _animationController.SetAnimationParameter("isWalk", false);
                _animationController.SetAnimationParameter("Idle", true);
                navMeshAgent.isStopped = false;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        if (target != null && !obstacleInBetween)
        {
            Movement();
        }else if (obstacleInBetween)
        {
            StandStill();
        }

        #region Arturo

        if (_monsterType == MonsterType.Arturo && target != null)
        {
            float dist = Vector3.Distance(transform.position, target.position);
            if (dist < arturoRange)
            {
                _currentState = MonsterState.ATTACK;
            }
            else if (dist > arturoRange)
            {
                _currentState = MonsterState.IDLE;
            }
        }
        if (_monsterType == MonsterType.Arturo && _currentState == MonsterState.ATTACK && !_died)
        {
            transform.LookAt(target.position);
            _animationController.SetAnimationParameter("Shooting", true);
            if (!_shooting)
            {
                StartCoroutine(FireArrow());
            }
        }
        else if (_monsterType == MonsterType.Arturo && _currentState == MonsterState.IDLE && !_died)
        {
            _animationController.SetAnimationParameter("Shooting", false);
            CancelInvoke();
        }
        #endregion


        #region FARRAPTORBOSS
        if (_monsterType == MonsterType.FARRAPTORBOSS)
        {
            float distanceToPlayer = Vector3.Distance(target.transform.position, transform.position);
            if (distanceToPlayer <= ferraptorAttackingDist && !findEnemies)
            {
                _currentState = MonsterState.ATTACK;
            }
        }
        #endregion


        //Keep enemy stopped
        if(_currentState == MonsterState.ATTACK)
        {
            StopMovement();
        }else if (_currentState == MonsterState.IDLE)
        {
            StopMovement();
        }

        enemyStopped = navMeshAgent.isStopped;
    }
    private IEnumerator FireArrow()
    {
        if (_shooting) yield return null;
        _shooting = true;
        yield return new WaitForSeconds(1f);
        GameObject arrow = Instantiate(_arrow, _arrowSpts.transform.position, Quaternion.Euler(-95, 85, -115), _arrowSpts.transform);
        arrow.GetComponent<ExplosiveArrow>().arrowDamage = arturoArrowDamage;
        arrow.transform.SetParent(null);
        _shooting = false;
        //arrow.transform.LookAt(Vector3.forward);
    }
    private void Movement()
    {
        //Handle normatl monster
        if (_currentState == MonsterState.IDLE && !findEnemies)
        {
            StandStill();
        }
        else if (_currentState == MonsterState.CHASE && !obstacleInBetween)
        {
            Chase();
        }
        else if (_currentState == MonsterState.ATTACK && !obstacleInBetween)
        {
            Attack();
        }

        //Handle when kaire passed
        /*        if (findEnemies && Vector3.Distance(target.transform.position, transform.position) > stoppingDistance)
                {
                    _currentState = MonsterState.CHASE;
                }
                else if (findEnemies && Vector3.Distance(target.transform.position, transform.position) < stoppingDistance)
                {
                    _currentState = MonsterState.ATTACK;
                }*/

        float distanceToPlayer = Vector3.Distance(target.transform.position, transform.position);
        if (!findEnemies)
        {
            StateManager(distanceToPlayer, findEnemies);
        }
        else if (findEnemies)
        {
            StateManager(distanceToPlayer, findEnemies);
        }

        if (distanceToPlayer <= stoppingDistance && !findEnemies)
        {
            _currentState = MonsterState.ATTACK;
        }


        /*if (distanceToPlayer <= stoppingDistance && findEnemies)
        {
            _currentState = MonsterState.ATTACK;
        }*/
        //Check for radius 

    }

    private void StateManager(float distanceToPlayer, bool val)
    {
        if(!val)
        {
            if (Vector3.Distance(_startingPoint, target.position) < _radiusRange * 2)
            {
                if (distanceToPlayer > stoppingDistance && !findEnemies)
                {
                    if (navMeshAgent.hasPath)
                    {
                        _currentState = MonsterState.CHASE;
                    }
                }
            }
            else if (Vector3.Distance(_startingPoint, target.position) > _radiusRange * 2)
            {
                _currentState = MonsterState.IDLE;
            }
        }
        else if (val)
        {
            if (Vector3.Distance(target.transform.position, transform.position) > stoppingDistance)
            {
                if (distanceToPlayer > stoppingDistance)
                {
                    if (navMeshAgent.hasPath)
                    {
                        _currentState = MonsterState.CHASE;
                    }
                    else
                    {
                        _currentState = MonsterState.CHASE;
                        //navMeshAgent.SetDestination(target.transform.position);
                    }
                }
            }
            else if (Vector3.Distance(target.transform.position, transform.position) < stoppingDistance)
            {
                //_currentState = MonsterState.IDLE;
                _currentState = MonsterState.ATTACK;
            }
        }
        
      
    }

    private void StandStill()
    {
        //StopMovement();
        _animationController.SetAnimationParameter("isWalk", false);
        _animationController.SetAnimationParameter("Idle", true);
        
    }

    private void Attack()
    {
        if (_monsterType == MonsterType.NORMAL || _monsterType == MonsterType.ShadowMonster)
        {
            _animationController.SetAnimationParameter("isWalk", false);
            //StopMovement();
            if (_isOnCooldown) return;

            _isOnCooldown = true;

            int randomAttackIndex = UnityEngine.Random.Range(1, 4);
            string _animationString = "Attack" + randomAttackIndex;
            _animationController.SetAnimationParameter(_animationString);

            //Cooldown
            #region cooldopwn
            if (_animationString == "Attack1")
            {
                StartCoroutine(StartCooldown(_attackOneCooldown));
                StartCoroutine(EnableLimbs(1.12f, 1));
            }
            else if (_animationString == "Attack2")
            {
                StartCoroutine(StartCooldown(_attackTwoCooldown));
                StartCoroutine(EnableLimbs(0.45f, 0));
            }
            else if (_animationString == "Attack3")
            {
                StartCoroutine(StartCooldown(_attackThreeCooldown));
                StartCoroutine(EnableLimbs(1.20f, 1));
            }
            #endregion
        }
        else if (_monsterType == MonsterType.FERRAPTOR || _monsterType == MonsterType.FARRAPTORBOSS)
        {
            if (_isOnCooldown) return;
            if (_died) return;
            _isOnCooldown = true;

            StartCoroutine(ApplyDamage());
        }

        //StopMovement();
        transform.LookAt(target);
    }


    // 0 for leg 1 for hands
    IEnumerator EnableLimbs(float val, int attackType)
    {
        yield return new WaitForSeconds(val - val * 0.1f);
        if (attackType == 0)
        {
            foreach (MonsterLimb monsterLimb in _monsterLimbs)
            {
                if (!monsterLimb._ishands)
                {
                    monsterLimb.damageToDeal = damageToDeal;
                    monsterLimb.GetComponent<BoxCollider>().enabled = true;
                }
            }
        }
        else
        {
            foreach (MonsterLimb monsterLimb in _monsterLimbs)
            {
                if (monsterLimb._ishands)
                {
                    monsterLimb.damageToDeal = damageToDeal;
                    monsterLimb.GetComponent<BoxCollider>().enabled = true;
                }
            }
        }

        yield return new WaitForSeconds(0.7f);
        foreach (MonsterLimb monsterLimb in _monsterLimbs)
        {
            monsterLimb.GetComponent<BoxCollider>().enabled = false;
        }

        if (_monsterType == MonsterType.ShadowMonster)
        {
            //Debug.LogError("KnockoutProcess 1");
            if (target != null && target.TryGetComponent<IntrantThirdPersonController>(out IntrantThirdPersonController _player))
            {

                //Debug.LogError("KnockoutProcess 2");
                _player.Knockout();
                Destroy(gameObject, .5f);
            }
        }
    }

    private IEnumerator StartCooldown(float cooldownDuration)
    {
        float cooldownTimer = cooldownDuration + 2f;
        while (cooldownTimer > 0f)
        {
            // Update the cooldown timer
            cooldownTimer -= Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Cooldown is over
        _isOnCooldown = false;
    }

    IEnumerator ApplyDamage()
    {
        target.GetComponent<IntrantPlayerHealthManager>().DealDamage(damageToDeal);
        yield return new WaitForSeconds(0.5f);
        _isOnCooldown = false;
    }
    private void Chase()
    {
        if (!findEnemies)
        {
            if (Vector3.Distance(_startingPoint, target.position) > _radiusRange * 2)
            {
                _currentState = MonsterState.IDLE;
                return;
            }
        }

        _animationController.SetAnimationParameter("isWalk", true);
        _animationController.SetAnimationParameter("Idle", false);
        ResumeMovement();
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void StopMovement()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.velocity = Vector3.zero;
    }

    public void ResumeMovement()
    {
        navMeshAgent.isStopped = false;
        if (target != null)
        {
            navMeshAgent.SetDestination(target.position);
            //Debug.LogError("Resuming Movement");
        }
    }

    public void SetState(MonsterState _state)
    {
        _currentState = _state;
    }

    private void OnDestroy()
    {
        GetComponent<MonsterHealthController>().OnMonsterDie -= MonsterDied;
        FrontRadar._stopBroadcast -= ObstacleHit;
        Destroy(_minimapPointer);
    }

    private void OnTriggerStay(Collider other)
    {
        if (_minimapPointer == null) return;
        if (other.gameObject.CompareTag(Tags.PlayerRange_TAG) && !_minimapPointer.activeSelf)
        {
            _minimapPointer.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == shieldLayer)
        {
            navMeshAgent.isStopped = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == shieldLayer)
        {
            navMeshAgent.isStopped = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == shieldLayer)
        {
            navMeshAgent.isStopped = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.PlayerRange_TAG) && _minimapPointer != null)
        {
            _minimapPointer.SetActive(false);
        }
    }
}