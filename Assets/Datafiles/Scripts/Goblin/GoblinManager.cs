using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum GoblinState
{
    CHASE,
    Snatch
}
public class GoblinManager : MonoBehaviour
{

    public static GoblinManager _instance;
    public GameObject minimapPointer;
    private GoblinAnimationController _anim;
    public NavMeshAgent _agent;
    public SnatchManager _snatchManager;
    private Transform _playerTarget;
    public Transform _goblinTarget;
    
    [Header("Jump")]
    public float jumpForce = 5f;
    public float jumpTime = 0.5f;
    public float jumpCooldown = 1f;
    private bool isJumping = false;
    private float jumpTimer = 0f;
    private float cooldownTimer = 0f;


    public float move_Speed = 3.5f;
    public float attack_Distance = 1f;
    public float chase_Player_AfterAttack_Distance = 1f;
    private float wait_before_Attack_Time = 2f;
    private float attack_Timer;

    private GoblinState _state;

    public int tokenStealAmount = 1;
    public float healthReductionPercentage = 0.2f;

    [Header("Bools")]
    public int _runfactor = 10;

    [Header("Bools")]
    public bool _init = false;
    public bool _snatched = false;
    public bool _onNavmesh = false;
    public bool _headingToMole = false;

    private void Awake()
    {
        _instance = this;
        _anim = GetComponent<GoblinAnimationController>();
        _agent = GetComponent<NavMeshAgent>();
        _playerTarget = GameObject.FindGameObjectWithTag(Tags.PLAYER_TAG).transform;
        _snatchManager = GetComponentInChildren<SnatchManager>();
    }

    private void Start()
    {
        PerformJump();
        attack_Timer = wait_before_Attack_Time;
    }

    private void FixedUpdate()
    {
        if (_state == GoblinState.CHASE && _init && !_playerTarget.GetComponent<IntrantThirdPersonController>().pause)
        {
            ChasePayer();
        }
        if (_state == GoblinState.Snatch && _init && !_snatched && !_playerTarget.GetComponent<IntrantThirdPersonController>().pause)
        {
            Snatch();
        }

        //Check If Goblin snatched and is onnavmesh
        Debug.LogWarning("Agent is on navmesh "+ _agent.isOnNavMesh);
        if(_agent.isOnNavMesh && _snatched && !_headingToMole && !_playerTarget.GetComponent<IntrantThirdPersonController>().pause)
        {
            Run();
        }


        //Check If agent is on navmesh or not
        if(_agent.isOnNavMesh)
        {
            _onNavmesh = true;
        }else if (!_agent.isOnNavMesh)
        {
            _onNavmesh = false;
        }
    }

    private void Snatch()
    {
        if(_headingToMole)return;
        _agent.velocity = Vector3.zero;
        _agent.isStopped = true; // Stop moving

        _anim.Walk(false);

        attack_Timer += Time.deltaTime;

        Vector3 direction = (_playerTarget.position - transform.position).normalized;
        direction.y = 0;
        transform.LookAt(transform.position + direction);

        if (attack_Timer > wait_before_Attack_Time)
        {
            _anim.Snatch();
            //attackPoint.SetActive(true);
            attack_Timer = 0f;
        }

        if (Vector3.Distance(transform.position, _playerTarget.position) > attack_Distance + chase_Player_AfterAttack_Distance)
        {
            _agent.isStopped = false; // Move again
            //attackPoint.SetActive(false);
            _state = GoblinState.CHASE;
        }
    }

    private void ChasePayer()
    {
        if(_headingToMole)return;
        _agent.SetDestination(_playerTarget.position);

        _agent.speed = move_Speed;

        if (_agent.velocity.sqrMagnitude == 0)
        {
            _anim.Run(false);
        }
        else
        {
            _anim.Run(true);
        }

        Vector3 direction = (_playerTarget.position - transform.position).normalized;
        direction.y = 0;

        _agent.isStopped = false; // Move continuously

        if (Vector3.Distance(transform.position, _playerTarget.position) <= attack_Distance)
        {
            _state = GoblinState.Snatch;
        }
    }

    private void PerformJump()
    {
        _anim.PerformJump();
        StartCoroutine(Jump());
    }

    IEnumerator Jump()
    {
        GetComponent<NavMeshAgent>().enabled = false;
        yield return new WaitForSeconds(1f);
        _state = GoblinState.CHASE;
        GetComponent<NavMeshAgent>().enabled = true;
        _init = true;
    }


    public void Run( )
    {
        //_agent.destination = GoblinMole._instance._mole[0].transform.position;
        try
        {
            _agent.isStopped = true;
            Debug.LogError("New Target "+ GoblinMole._instance._mole[0].transform.position);
            List<GameObject> _goblinMole = GoblinMole._instance._mole;
            float minDist = Vector3.Distance(transform.position, _goblinMole[0].transform.position);
            GameObject nearestMole = _goblinMole[0];
            foreach (var mole in _goblinMole)
            {
                float dist = Vector3.Distance(transform.position, mole.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearestMole = mole;
                }
            }
            _agent.SetDestination(nearestMole.transform.position);
            Debug.LogError("Goblin target reset");
            _agent.isStopped = false;
            _state = GoblinState.CHASE;
            _headingToMole = true;
        }catch (Exception e) { 
            Debug.LogError(e+ " Failed to move goblin");
        }
    }

    private void OnDestroy()
    {
        Destroy(minimapPointer);
    }
}
