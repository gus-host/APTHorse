using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum DragonState
{
    CHASE,
    ATTACK
}

public class DragonController : MonoBehaviour
{
    private DragonAnimation dragon_Anim;
    private NavMeshAgent navAgent;

    private Transform playerTarget;

    public float move_Speed = 3.5f;

    public float attack_Distance = 1f;
    public float chase_Player_AfterAttack_Distance = 1f;
    private float wait_before_Attack_Time = 2f;
    private float attack_Timer;

    private DragonState dragon_State;

    public GameObject attackPoint;

    

    void Awake()
    {
        dragon_Anim = GetComponent<DragonAnimation>();
        navAgent = GetComponent<NavMeshAgent>();

        playerTarget = GameObject.FindGameObjectWithTag(Tags.PLAYER_TAG).transform;
    }

    private void Start()
    {
        dragon_State = DragonState.CHASE;
        attack_Timer = wait_before_Attack_Time;
    }

    // Update is called once per frame
    void Update()
    {
        if (dragon_State == DragonState.CHASE)
        {
            chasePayer();
        }

        if (dragon_State == DragonState.ATTACK)
        {
            attackPlayer();
        }
    }

    void chasePayer()
    {
        print("ChasePlayer");

        navAgent.SetDestination(playerTarget.position);
        print("playerTarget.position" + playerTarget.position);
        navAgent.speed = move_Speed;

        if (navAgent.velocity.sqrMagnitude == 0)
        {
            dragon_Anim.Walk(false);
        }
        else
        {
            dragon_Anim.Walk(true);
        }

        Vector3 direction = (playerTarget.position - transform.position).normalized;
        direction.y = 0;
        transform.LookAt(transform.position + direction);

        navAgent.isStopped = false; // Move continuously

        if (Vector3.Distance(transform.position, playerTarget.position) <= attack_Distance)
        {
            dragon_State = DragonState.ATTACK;
        }
    }

    void attackPlayer()
    {
        navAgent.velocity = Vector3.zero;
        navAgent.isStopped = true; // Stop moving

        dragon_Anim.Walk(false);

        attack_Timer += Time.deltaTime;

        Vector3 direction = (playerTarget.position - transform.position).normalized;
        direction.y = 0;
        transform.LookAt(transform.position + direction);

        if (attack_Timer > wait_before_Attack_Time)
        {
            dragon_Anim.Attack1();
            attackPoint.SetActive(true);
            attack_Timer = 0f;
        }

        if (Vector3.Distance(transform.position, playerTarget.position) > attack_Distance + chase_Player_AfterAttack_Distance)
        {
            navAgent.isStopped = false; // Move again
            attackPoint.SetActive(false);
            dragon_State = DragonState.CHASE;
        }
    }




    void Activate_AttackPoint()
    {
        attackPoint.SetActive(true);
    }


    void Deactivate_AttackPoint()
    {
        if (attackPoint.activeInHierarchy)
        {
            attackPoint.SetActive(false);
        }
    }

  /*  private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, attack_Distance);
    }   */
}
