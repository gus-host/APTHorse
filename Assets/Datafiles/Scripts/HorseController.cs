using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HorseColor
{
    Green,
    White,
    Blue,
    Brown,
    Red
}

[Serializable]
public struct Player
{
    public HorseColor color;
    public int id;
    public string address;
}

public class HorseController : MonoBehaviourPunCallbacks
{
    public Player playerProperties = new Player();

    public RaceManager raceManager;

    public WayPointList waypoints;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public float _p1;
    public float _p2;
    public float _p3;
    public int obstacleIndex = 0;

    public Sprite _up;
    public Sprite _down;
    public Sprite _forward;
    public Sprite _backward;

    public int index = 0;
    public int horseId = 0;

    public int totalLap = 1;
    public int lapLeft = 1;

    public Transform[] OopsSpts; 

    //4-6
    public float _maxSpeed = 5f;
    public float _minSpeed = 0;
    //0.1-1 is normal
    public float _acceleration = 2f;
    public float currentSpeed = 0;

    public Transform _target;

    public bool _canMove;
    public bool _running;
    public bool _raceFinished;
    public bool _lastLap;
    public bool _gameEnded;
    public bool _InForwardRange;
    public bool _InBackwardRange;
    public bool _InUpRange;
    public bool _InDownRange;
    public bool lapReductionProcessed = false;
    public bool _gameStarted = false;

    float accelerationInput = 0f;

    private int  RunStraight;
    private int RunDownHash;
    private int RunUpHash;
    private int IdleForwardHash;
    private int IdleUpHash;
    private int IdleBackwardHash;
    private int IdleDownHash;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        raceManager = FindObjectOfType<RaceManager>();

        // Add horse to the Racemanager
        #region        
        if (raceManager.jockeys.Length <=5)
        {
            raceManager.AddHorse(this);
            if (raceManager.jockeys[4] != null)
            {
                raceManager.addedAllJockeys = true;
            }
        }
        else
        {
            Debug.LogError($"Invalid index: {horseId} and max index {raceManager.jockeys.Length}");
        }
        #endregion

        RunStraight = Animator.StringToHash("RunStraight");
        RunDownHash = Animator.StringToHash("RunDown");
        RunUpHash = Animator.StringToHash("RunUp");
        IdleForwardHash = Animator.StringToHash("IdleStraight");
        IdleUpHash = Animator.StringToHash("IdleUp");
        IdleBackwardHash = Animator.StringToHash("IdleBack");
        IdleDownHash = Animator.StringToHash("IdleDown");

        //Set to Idle
        animator.SetBool(IdleForwardHash, true);
        animator.SetBool(IdleUpHash, false);
        animator.SetBool(IdleBackwardHash, false);
        animator.SetBool(RunStraight, false);
        animator.SetBool(RunDownHash, false);
        animator.SetBool(RunUpHash, false);

       /* if (playerProperties.color == HorseColor.Green)
        {
            animator.SetBool(IdleDownHash, true);
            animator.SetBool(IdleForwardHash, false);
            animator.SetBool(IdleUpHash, false);
            animator.SetBool(IdleBackwardHash, false);
            animator.SetBool(RunStraight, false);
            animator.SetBool(RunDownHash, false);
            animator.SetBool(RunUpHash, false);
        }
        else
        {
            animator.SetBool(IdleForwardHash, true);
            animator.SetBool(IdleUpHash, false);
            animator.SetBool(IdleBackwardHash, false);
            animator.SetBool(RunStraight, false);
            animator.SetBool(RunDownHash, false);
            animator.SetBool(RunUpHash, false);
        }*/
    }

    public void INIT()
    {
        photonView.RPC("GetTheWaypoint", RpcTarget.AllBuffered);
    }


    [PunRPC]
    public void GetTheWaypoint()
    {
        WayPointList[] wayPointLists = FindObjectsOfType<WayPointList>();

        foreach (var waypointList in wayPointLists)
        {
            if (horseId == 0 && waypointList.horseSer == HorseSer.H1)
            {
                waypoints = waypointList;
                transform.position = waypoints._wayPoint[0].position;
            }
            else if (horseId == 1 && waypointList.horseSer == HorseSer.H2)
            {
                waypoints = waypointList;
                transform.position = waypoints._wayPoint[0].position;
            }
            else if (horseId == 2 && waypointList.horseSer == HorseSer.H3)
            {
                waypoints = waypointList;
                transform.position = waypoints._wayPoint[0].position;
            }
            else if (horseId == 3 && waypointList.horseSer == HorseSer.H4)
            {
                waypoints = waypointList;
                transform.position = waypoints._wayPoint[0].position;
            }
            else if (horseId == 4 && waypointList.horseSer == HorseSer.H5)
            {
                waypoints = waypointList;
                transform.position = waypoints._wayPoint[0].position;
            }
        }
    }

    public void StartRace()
    {
        _gameStarted = true;
        StartCoroutine(Move());
        if (obstacleIndex == 0 && photonView.IsMine)
        {
            waypoints.SpawnObstacleAtPercentage(_p1, obstacleIndex++);
        }

/*        if (playerProperties.color == HorseColor.Green)
        {
            animator.SetBool(IdleDownHash, false);
            animator.SetBool(IdleForwardHash, false);
            animator.SetBool(IdleUpHash, false);
            animator.SetBool(IdleBackwardHash, false);
            animator.SetBool(RunStraight, false);
            animator.SetBool(RunDownHash, true);
            animator.SetBool(RunUpHash, false);
        }
        else{*/
            animator.SetBool(IdleDownHash, false);
            animator.SetBool(IdleForwardHash, false);
            animator.SetBool(IdleUpHash, false);
            animator.SetBool(IdleBackwardHash, false);
            animator.SetBool(RunStraight, true);
            animator.SetBool(RunDownHash, false);
            animator.SetBool(RunUpHash, false);
        /*}*/
    }

    IEnumerator Move()
    {
        while (!_raceFinished && _canMove && index < waypoints._wayPoint.Length)
        {
            if(_raceFinished)
            {
                yield break;
            }
            _target = waypoints._wayPoint[index];
            if (Vector3.Distance(transform.position, _target.position) > 0.1f)
            {
                Vector3 direction = _target.position - transform.position;

                // Calculate the angle in radians
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                // Gradually increase speed using lerp
                currentSpeed = Mathf.Lerp(currentSpeed, _maxSpeed, Time.deltaTime * _acceleration);

                // Move towards the target
                float step = currentSpeed * Time.deltaTime;

                //Debug.Log($"Running {step}");

                transform.position = Vector3.MoveTowards(transform.position, _target.position, step);
            }
            else if (Vector3.Distance(transform.position, _target.position) <= 0.1f)
            {
                //Check for next lap
                
                index++;

                //Last checkpoint and index gone out of bounds then 
                // 1. Decrease total lap
                // 2. Total lap is non negative then restart by index = 0
                if (index > waypoints._wayPoint.Length - 1)
                {
                    if(totalLap != 0)
                    {
                        totalLap--;
                    }
                    if(totalLap > 0)
                    {
                        index = 0;
                    }
                    Debug.Log($"Checking for last lap index {index}");
                }
                currentSpeed = _minSpeed; // Reset speed when reaching a new waypoint
            }
            _running = true;
            yield return null;
        }
    }

    private void Update()
    {
        if(_raceFinished && !_gameEnded)
        {
            _gameEnded = true;
            RaceFinished();
        }
    }

    private void RaceFinished()
    {
        raceManager.horses.Push(this);
        if (PhotonNetwork.IsMasterClient)
        {
            raceManager.RPCPrintStack();
        }
    }

    public void SetWaypoints(WayPointList wp)
    {
        waypoints = wp;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!_gameStarted) return;

        if (_gameEnded) {

            if (_InForwardRange)
            {
                animator.SetBool(IdleForwardHash, true);
                animator.SetBool(IdleUpHash, false);
                animator.SetBool(IdleDownHash, false);
                animator.SetBool(IdleBackwardHash, false);
                animator.SetBool(RunStraight, false);
                animator.SetBool(RunDownHash, false);
                animator.SetBool(RunUpHash, false);
                animator.SetBool(IdleForwardHash, false);
            }else if (_InDownRange)
            {
                animator.SetBool(IdleForwardHash, false);
                animator.SetBool(IdleUpHash, false);
                animator.SetBool(IdleDownHash, true);
                animator.SetBool(IdleBackwardHash, false);
                animator.SetBool(RunStraight, false);
                animator.SetBool(RunDownHash, false);
                animator.SetBool(RunUpHash, false);
                animator.SetBool(IdleForwardHash, false);
            }else if (_InBackwardRange)
            {
                animator.SetBool(IdleForwardHash, false);
                animator.SetBool(IdleUpHash, false);
                animator.SetBool(IdleDownHash, false);
                animator.SetBool(IdleBackwardHash, true);
                animator.SetBool(RunStraight, false);
                animator.SetBool(RunDownHash, false);
                animator.SetBool(RunUpHash, false);
                animator.SetBool(IdleForwardHash, false);
            }
            else if (_InUpRange)
            {
                animator.SetBool(IdleForwardHash, false);
                animator.SetBool(IdleUpHash, true);
                animator.SetBool(IdleDownHash, false);
                animator.SetBool(IdleBackwardHash, false);
                animator.SetBool(RunStraight, false);
                animator.SetBool(RunDownHash, false);
                animator.SetBool(RunUpHash, false);
                animator.SetBool(IdleForwardHash, false);
            }
            return;
        } 
        
        if (collision.gameObject.CompareTag("Start"))
        {
            animator.SetBool(IdleForwardHash, false);
            animator.SetBool(IdleUpHash, false);
            animator.SetBool(IdleDownHash, false);
            animator.SetBool(IdleBackwardHash, false);
            animator.SetBool(RunStraight, true);
            animator.SetBool(RunDownHash, false);
            animator.SetBool(RunUpHash, false);
            animator.SetBool(IdleForwardHash, false);
        }
        else if (collision.gameObject.CompareTag("First"))
        {
            animator.SetBool(IdleForwardHash, false);
            animator.SetBool(IdleUpHash, false);
            animator.SetBool(IdleDownHash, false);
            animator.SetBool(IdleBackwardHash, false);
            animator.SetBool(RunDownHash, true);
            animator.SetBool(RunStraight, false);
            animator.SetBool(RunUpHash, false);
            animator.SetBool(IdleForwardHash, false);

            _InDownRange = true;
            _InBackwardRange = false;
            _InForwardRange = false;
            _InUpRange = false;
        }
        else if (collision.gameObject.CompareTag("Second"))
        {
            animator.SetBool(IdleForwardHash, false);
            animator.SetBool(IdleUpHash, false);
            animator.SetBool(IdleDownHash, false);
            animator.SetBool(IdleBackwardHash, false);
            animator.SetBool(RunStraight, true);
            animator.SetBool(RunDownHash, false);
            animator.SetBool(RunUpHash, false);
            animator.SetBool(IdleForwardHash, false);
            spriteRenderer.flipX = true;

            _InDownRange = false;
            _InBackwardRange = true;
            _InForwardRange = false;
            _InUpRange = false;
        }
        else if (collision.gameObject.CompareTag("Third"))
        {
            animator.SetBool(IdleForwardHash, false);
            animator.SetBool(IdleUpHash, false);
            animator.SetBool(IdleDownHash, false);
            animator.SetBool(IdleBackwardHash, false);
            animator.SetBool(RunUpHash, true);
            animator.SetBool(RunDownHash, false);
            animator.SetBool(RunStraight, false);
            animator.SetBool(IdleForwardHash, false);

            _InDownRange = false;
            _InBackwardRange = false;
            _InForwardRange = false;
            _InUpRange = true;
        }
        else if (collision.gameObject.CompareTag("Fourth"))
        {
            animator.SetBool(IdleForwardHash, false);
            animator.SetBool(IdleUpHash, false);
            animator.SetBool(IdleDownHash, false);
            animator.SetBool(IdleBackwardHash, false);
            animator.SetBool(RunStraight, true);
            animator.SetBool(RunUpHash, false);
            animator.SetBool(RunDownHash, false);
            animator.SetBool(IdleForwardHash, false);
            spriteRenderer.flipX = false;

            _InDownRange = false;
            _InBackwardRange = false;
            _InForwardRange = true;
            _InUpRange = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        #region Change animation on trigger
        Debug.Log($"Collision {collision.gameObject.tag}");
        if (collision.gameObject.CompareTag("Start"))
        {
            animator.SetBool(IdleForwardHash, false);
            animator.SetBool(IdleUpHash, false);
            animator.SetBool(IdleDownHash, false);
            animator.SetBool(IdleBackwardHash, false);
            animator.SetBool(RunStraight, true);
            animator.SetBool(RunDownHash, false);
            animator.SetBool(RunUpHash, false);
            animator.SetBool(IdleForwardHash, false);
        }
        else if (collision.gameObject.CompareTag("First"))
        {
            animator.SetBool(IdleForwardHash, false);
            animator.SetBool(IdleUpHash, false);
            animator.SetBool(IdleDownHash, false);
            animator.SetBool(IdleBackwardHash, false);
            animator.SetBool(RunDownHash, true);
            animator.SetBool(RunStraight, false);
            animator.SetBool(RunUpHash, false);
            animator.SetBool(IdleForwardHash, false);

            _InDownRange = true;
            _InBackwardRange = false;
            _InForwardRange = false;
            _InUpRange = false;
        }
        else if (collision.gameObject.CompareTag("Second"))
        {
            animator.SetBool(IdleForwardHash, false);
            animator.SetBool(IdleUpHash, false);
            animator.SetBool(IdleDownHash, false);
            animator.SetBool(IdleBackwardHash, false);
            animator.SetBool(RunStraight, true);
            animator.SetBool(RunDownHash, false);
            animator.SetBool(RunUpHash, false);
            animator.SetBool(IdleForwardHash, false);
            spriteRenderer.flipX = true;

            _InDownRange = false;
            _InBackwardRange = true;
            _InForwardRange = false;
            _InUpRange = false;
        }
        else if (collision.gameObject.CompareTag("Third"))
        {
            animator.SetBool(IdleForwardHash, false);
            animator.SetBool(IdleUpHash, false);
            animator.SetBool(IdleDownHash, false);
            animator.SetBool(IdleBackwardHash, false);
            animator.SetBool(RunUpHash, true);
            animator.SetBool(RunDownHash, false);
            animator.SetBool(RunStraight, false);
            animator.SetBool(IdleForwardHash, false);

            _InDownRange = false;
            _InBackwardRange = false;
            _InForwardRange = false;
            _InUpRange = true;
        }
        else if (collision.gameObject.CompareTag("Fourth"))
        {
            animator.SetBool(IdleForwardHash, false);
            animator.SetBool(IdleUpHash, false);
            animator.SetBool(IdleDownHash, false);
            animator.SetBool(IdleBackwardHash, false);
            animator.SetBool(RunStraight, true);
            animator.SetBool(RunUpHash, false);
            animator.SetBool(RunDownHash, false);
            animator.SetBool(IdleForwardHash, false);
            spriteRenderer.flipX = false;

            _InDownRange = false;
            _InBackwardRange = false;
            _InForwardRange = true;
            _InUpRange = false;
        }
        else if (collision.gameObject.CompareTag("Puddle"))
        {
            currentSpeed = _minSpeed;
            if (photonView.IsMine)
            {
                var puddleReaction = PhotonNetwork.Instantiate("OOPS", OopsSpts[UnityEngine.Random.Range(0, OopsSpts.Length)].position, Quaternion.identity);
                Destroy( puddleReaction, 3f);
            }
            Destroy(collision.gameObject);
        }
        #endregion
        // Reduce lap and check for game finished
        else if (collision.gameObject.TryGetComponent<OnEnterDisableCollider>(out OnEnterDisableCollider onEnterDisableCollider)
            && collision.gameObject.CompareTag("Finished"))
        {
            ReduceLapandUpdateCheckpoint(onEnterDisableCollider);
        }
    }

    public void ReduceLapandUpdateCheckpoint(OnEnterDisableCollider onEnterDisableCollider)
    {
        if (!_lastLap)
        {
            photonView.RPC("RPCReduceLapandUpdateCheckpoint", RpcTarget.All);
            if (onEnterDisableCollider._nextCheckpoint.Length > 0)
            {
                onEnterDisableCollider.UpdateCheckPoint();
            }
        }
        else if (_lastLap)
        {
            Debug.LogError("Completed Race");
            spriteRenderer.flipX = false;
            _canMove = false;
            _running = false;
            _raceFinished = true;
            if (_InForwardRange)
            {
                animator.SetBool(IdleForwardHash, true);
                animator.SetBool(IdleUpHash, false);
                animator.SetBool(IdleDownHash, false);
                animator.SetBool(IdleBackwardHash, false);
                animator.SetBool(RunStraight, false);
                animator.SetBool(RunUpHash, false);
                animator.SetBool(RunDownHash, false);
            }
            else if (_InUpRange)
            {
                animator.SetBool(IdleUpHash, true);
                animator.SetBool(IdleForwardHash, false);
                animator.SetBool(IdleDownHash, false);
                animator.SetBool(IdleBackwardHash, false);
                animator.SetBool(RunStraight, false);
                animator.SetBool(RunUpHash, false);
                animator.SetBool(RunDownHash, false);
            }
            else if (_InDownRange)
            {
                animator.SetBool(IdleDownHash, true);
                animator.SetBool(IdleForwardHash, false);
                animator.SetBool(IdleUpHash, false);
                animator.SetBool(IdleBackwardHash, false);
                animator.SetBool(RunStraight, false);
                animator.SetBool(RunUpHash, false);
                animator.SetBool(RunDownHash, false);
            }
            else if (_InBackwardRange)
            {
                animator.SetBool(IdleBackwardHash, true);
                animator.SetBool(IdleForwardHash, false);
                animator.SetBool(IdleUpHash, false);
                animator.SetBool(IdleDownHash, false);
                animator.SetBool(RunStraight, false);
                animator.SetBool(RunUpHash, false);
                animator.SetBool(RunDownHash, false);
            }
        }
    }

    [PunRPC]
    public void RPCReduceLapandUpdateCheckpoint()
    {
        ReduceLapandCheckForLastLap();
    }

    private void ReduceLapandCheckForLastLap()
    {
        if(!lapReductionProcessed)
        {
            lapLeft--;
            lapReductionProcessed = true;
            if (obstacleIndex == 1 && photonView.IsMine)
            {
                waypoints.SpawnObstacleAtPercentage(_p2, obstacleIndex++);
            }else if(obstacleIndex == 2 && photonView.IsMine)
            {
                waypoints.SpawnObstacleAtPercentage(_p3, obstacleIndex++);
            }
        }
        photonView.RPC("SetLapReductionProcessed", RpcTarget.All, true);
        photonView.RPC("RPCDelayLapReductionProcessed", RpcTarget.All, true);
    }

    [PunRPC]
    private void SetLapReductionProcessed(bool processed)
    {
        Debug.LogError("Last lap");
        if (processed && lapLeft == 1)
        {
            Debug.LogError("Last lap");
            StartCoroutine(DelayLastLap(true));
        }
    }

    [PunRPC]
    private void RPCDelayLapReductionProcessed(bool processed)
    {
        StartCoroutine(DelayLapReductionProcessed(processed));
    }

    private IEnumerator DelayLapReductionProcessed(bool lastLap)
    {
        Debug.LogError("LapReductionprocessed");
        yield return new WaitForSecondsRealtime(1.5f);
        lapReductionProcessed = !lastLap;
    }
    private IEnumerator DelayLastLap(bool lastLap)
    {
        Debug.LogError("Last lap");
        yield return new WaitForSecondsRealtime(1.5f);
        _lastLap = lastLap;
    }

    public void RPCAssign(int horseId, int speed, float minSpeed, float acceleration,string address , int totalLap, int lapLeft, bool lastLap, float p1, float p2, float p3)
    {
        photonView.RPC("AssignVal", RpcTarget.AllBufferedViaServer,horseId ,speed, minSpeed, acceleration,address , totalLap, lapLeft, lastLap, p1, p2, p3);
    }

    [PunRPC]
    public void AssignVal(int _horseID ,int speed, float minSpeed, float acceleration,string address , int _totalLap, int _lapLeft, bool lastLap, float p1, float p2, float p3)
    {
        horseId = _horseID;
        _maxSpeed = speed;
        _minSpeed = minSpeed;
        _acceleration = acceleration;
        totalLap = _totalLap;
        lapLeft  = _lapLeft;
        if (totalLap == 1)
        {
            _lastLap = lastLap;
        }
        _p1 = p1;
        _p2 = p2;
        _p3 = p3;
        playerProperties.address = address;
        INIT();
    }

    internal void RPCAssignHorseID(int spawnAt)
    {
        photonView.RPC("AssignHorseID", RpcTarget.AllBufferedViaServer, spawnAt);
    }

    [PunRPC]
    public void AssignHorseID(int val)
    {
        horseId = val;
    }

    internal string GetName()
    {
        return photonView.Owner.NickName;
    }
}