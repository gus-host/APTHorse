using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    CHASE,
    ATTACK
}

public class EnemyController : MonoBehaviour
{
    private CharacterAnimation enemy_Anim;
    private NavMeshAgent navAgent;

    public Transform playerTarget;
    public IntrantThirdPersonController player;
    public Transform playerRef;
    public GameObject _theraRef;

    public GameObject _playerStationVFX;

    public float move_Speed = 3.5f;

    public float attack_Distance = 1f;
    public float chase_Player_AfterAttack_Distance = 1f;
    private float wait_before_Attack_Time = 2f;
    private float attack_Timer;

    private EnemyState enemy_State;

    public GameObject attackPoint;

    [Header("Bools")]
    public bool _paused;
    public bool _isKaire = false;
    public bool _canAttack = false;
    public bool _playerTriggered = false;
    public bool _headingToTerathsGate = false;
    public bool _headingToPortal = false;
    public bool _fightPlayer = false;
    
    void Awake()
    {
        enemy_Anim = GetComponent<CharacterAnimation>();
        navAgent = GetComponent<NavMeshAgent>();
        playerRef = IntrantThirdPersonController.instance.transform;
        player = IntrantThirdPersonController.instance;
    }

    private void Start()
    {
        enemy_State = EnemyState.CHASE;
        attack_Timer = wait_before_Attack_Time;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isKaire && playerTarget != null && !player.pause)
        {
            if (enemy_State == EnemyState.CHASE)
            {
                chasePayer();
            }

            if (enemy_State == EnemyState.ATTACK && playerTarget.GetComponent<IntrantPlayerHealthManager>().currentHealth > 0)
            {
                attackPlayer();
            }
            if (IntrantThirdPersonController.instance._paused)
            {
                _paused = true;
                PauseandResumeEnemy(_paused);
            }
            else if (!IntrantThirdPersonController.instance._paused)
            {
                _paused = false;
                PauseandResumeEnemy(_paused);
            }
        }
        if (_isKaire && !player.pause)
        {
            if (enemy_State == EnemyState.CHASE)
            {
                chasePayer();
            }

            if (enemy_State == EnemyState.ATTACK && playerTarget.GetComponent<IntrantPlayerHealthManager>().currentHealth > 0)
            {
                attackPlayer();
            }
        }
        if (_isKaire && _theraRef.GetComponent<TheraHealthManager>().playerDied)
        {
            StartCoroutine(FollowGateWithDelay());
        }

        if (_headingToPortal)
        {
            _headingToPortal = false;

            StartCoroutine(FollowGateWithDelay(null));
        }

        if(_headingToTerathsGate && playerTarget.GetComponent<IntrantThirdPersonController>()._map ==MAP.WEEK2)
        {
            if(Vector3.Distance(transform.position, playerTarget.transform.position) > 8f)
            {
                navAgent.isStopped = true;
            }else if (Vector3.Distance(transform.position, playerTarget.transform.position) < 8f)
            {
                navAgent.isStopped = false;
            }
        }
    }


    private void PauseandResumeEnemy(bool _pause)
    {
        navAgent.isStopped = _pause;
    }

    void chasePayer()
    {
        // print("ChasePlayer");
        if (_isKaire && !_playerTriggered && !_headingToTerathsGate)
        {
            navAgent.SetDestination(playerTarget.position);
        }
        if (!_isKaire)
        {
            navAgent.SetDestination(playerTarget.position);
        }

        navAgent.speed = move_Speed;

        if (navAgent.velocity.sqrMagnitude == 0)
        {
            enemy_Anim.Walk(false);
        }
        else
        {
            enemy_Anim.Walk(true);
        }

        try
        {
            Vector3 direction = (playerTarget.position - transform.position).normalized;
            direction.y = 0;
        }
        catch (Exception e)
        {

        }

        if (Vector3.Distance(transform.position, playerTarget.position) <= attack_Distance && !_paused && !_headingToTerathsGate)
        {
            enemy_State = EnemyState.ATTACK;
        }
    }

    void attackPlayer()
    {
        navAgent.velocity = Vector3.zero;
        navAgent.isStopped = true; // Stop moving

        enemy_Anim.Walk(false);

        attack_Timer += Time.deltaTime;

        Vector3 direction = (playerTarget.position - transform.position).normalized;
        direction.y = 0;
        transform.LookAt(transform.position + direction);

        if (attack_Timer > wait_before_Attack_Time && !_paused)
        {
            enemy_Anim.Attack1();
            try
            {
                if(playerRef.GetComponent<IntrantThirdPersonController>()._map == MAP.WEEK1)
                {
                    _theraRef.GetComponent<TheraHealthManager>().ApplyDamage(50f);
                }
            }
            catch (Exception e)
            {

            }
            attackPoint.SetActive(true);
            attack_Timer = 0f;
        }
        else if(attack_Timer < wait_before_Attack_Time)
        {
            Deactivate_AttackPoint();
        }

        if (Vector3.Distance(transform.position, playerTarget.position) > attack_Distance + chase_Player_AfterAttack_Distance)
        {
            navAgent.isStopped = false; // Move again
            attackPoint.SetActive(false);
            enemy_State = EnemyState.CHASE;
        }
    }

    void Deactivate_AttackPoint()
    {
        if (attackPoint.activeInHierarchy)
        {
            attackPoint.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isKaire && other.gameObject.CompareTag(Tags.PLAYER_TAG) && !_playerTriggered)
        {
            _playerTriggered = true;
            //StartCoroutine(FollowGateWithDelay(other));
            _theraRef = other.gameObject.GetComponent<PlayerTheraConnectionManager>()._theraRef;
        }

        if (other.gameObject.CompareTag(Tags.PlayerRange_TAG) && !_headingToTerathsGate)
        {
            playerTarget = GameObject.FindGameObjectWithTag(Tags.PLAYER_TAG).transform;
            if (playerTarget == null)
            {
                //Debug.Log("Cant find player");
            }else if (playerTarget != null)
            {
                //Debug.Log("Found Player" + playerTarget);
            }
            navAgent.SetDestination(playerTarget.transform.position);
            if (_playerStationVFX != null)
            {
                _playerStationVFX.SetActive(false);
            }
        }

        if (other.gameObject.CompareTag(Tags.Finish_TAG) && _isKaire)
        {
            IntrantThirdPersonController.instance.gameObject.GetComponent<PlayerTimelineManager>()._kairePassed = true;
        }
    }
    public void Stop()
    {
        navAgent.isStopped = true;
    }

    public void Resume()
    {
        navAgent.isStopped = false;
    }

    public void FollowGate()
    {
        StartCoroutine(FollowGateWithDelay());
    }

    public IEnumerator FollowGateWithDelay(Collider other = null)
    {
        yield return new WaitForSeconds(2f);
        _headingToTerathsGate = true;
        try
        {
            navAgent.SetDestination(FinishGate.instance.gameObject.transform.position);
            //Debug.LogError("setting Destination ...");
        }
        catch (Exception e)
        {
            //Debug.LogError("Failed to set dest..");
        }
    }

    internal void ResetTargetToPlayer()
    {
        navAgent.SetDestination(playerRef.position);
    }
}
