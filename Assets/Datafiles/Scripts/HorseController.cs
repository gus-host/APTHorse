using Photon.Pun;
using System;
using System.Collections;
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
}

public class HorseController : MonoBehaviourPunCallbacks
{
    public Player playerProperties = new Player();

    public RaceManager raceManager;

    public WayPointList waypoints;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    public Sprite _up;
    public Sprite _down;
    public Sprite _forward;
    public Sprite _backward;

    public int index = 0;
    public int horseId = 0;

    public int totalLap = 1;
    public int lapLeft = 1;

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
        horseId = playerProperties.id - 1;
        
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

        photonView.RPC("GetTheWaypoint", RpcTarget.AllBuffered);

        RunStraight = Animator.StringToHash("RunStraight");
        RunDownHash = Animator.StringToHash("RunDown");
        RunUpHash = Animator.StringToHash("RunUp");
        IdleForwardHash = Animator.StringToHash("IdleStraight");
        IdleUpHash = Animator.StringToHash("IdleUp");
        IdleBackwardHash = Animator.StringToHash("IdleBack");
        IdleDownHash = Animator.StringToHash("IdleDown");
        
        //Set to Idle
        if(playerProperties.color == HorseColor.Green)
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
        }

        // Disable start button if not masterclient
        if(PhotonNetwork.LocalPlayer == PhotonNetwork.MasterClient)
        {
            raceManager._start.gameObject.SetActive(true);
        }
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
            }
            else if (horseId == 1 && waypointList.horseSer == HorseSer.H2)
            {
                waypoints = waypointList;
            }
            else if (horseId == 2 && waypointList.horseSer == HorseSer.H3)
            {
                waypoints = waypointList;
            }
            else if (horseId == 3 && waypointList.horseSer == HorseSer.H4)
            {
                waypoints = waypointList;
            }
            else if (horseId == 4 && waypointList.horseSer == HorseSer.H5)
            {
                waypoints = waypointList;
            }
        }
    }

    public void StartRace()
    {
        StartCoroutine(Move());
        if (playerProperties.color == HorseColor.Green)
        {
            animator.SetBool(IdleDownHash, false);
            animator.SetBool(IdleForwardHash, false);
            animator.SetBool(IdleUpHash, false);
            animator.SetBool(IdleBackwardHash, false);
            animator.SetBool(RunStraight, false);
            animator.SetBool(RunDownHash, true);
            animator.SetBool(RunUpHash, false);
        }
        else{
            animator.SetBool(IdleDownHash, false);
            animator.SetBool(IdleForwardHash, false);
            animator.SetBool(IdleUpHash, false);
            animator.SetBool(IdleBackwardHash, false);
            animator.SetBool(RunStraight, true);
            animator.SetBool(RunDownHash, false);
            animator.SetBool(RunUpHash, false);
        }
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
            photonView.RPC("RPCReduceLapandUpdateCheckpoint", RpcTarget.AllBuffered);
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
    /*
        [PunRPC]
        public void RPCReduceLapandUpdateCheckpoint()
        {
            if (!lapReductionProcessed)
            {
                ReduceLapandCheckForLastLap();
                lapReductionProcessed = true;
            }
        }

        private void ReduceLapandCheckForLastLap()
        {
            lapLeft--;
            lapReductionProcessed = false;
            if (lapLeft == 1)
            {
                photonView.RPC("SetLastLap", RpcTarget.All, true);
            }
        }

        [PunRPC]
        private void SetLastLap(bool lastLap)
        {
            StartCoroutine(DelayLastLap(lastLap));
        }*/

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
        }
        photonView.RPC("SetLapReductionProcessed", RpcTarget.AllBuffered, true);
        photonView.RPC("RPCDelayLapReductionProcessed", RpcTarget.AllBuffered, true);
    }

    [PunRPC]
    private void SetLapReductionProcessed(bool processed)
    {
        if (processed && lapLeft == 1)
        {
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
        yield return new WaitForSeconds(1.5f);
        lapReductionProcessed = !lastLap;
    }
    private IEnumerator DelayLastLap(bool lastLap)
    {
        yield return new WaitForSeconds(1.5f);
        _lastLap = lastLap;
    }

    public void RPCAssign(int speed, float minSpeed, float acceleration, int totalLap, int lapLeft, bool lastLap)
    {
        photonView.RPC("AssignVal", RpcTarget.AllBufferedViaServer ,speed, minSpeed, acceleration, totalLap, lapLeft, lastLap);
    }

    [PunRPC]
    public void AssignVal(int speed, float minSpeed, float acceleration, int _totalLap, int _lapLeft, bool lastLap)
    {
        _maxSpeed = speed;
        _minSpeed = minSpeed;
        _acceleration = acceleration;
        totalLap = _totalLap;
        _lapLeft = lapLeft;
        _lastLap = lastLap;
    }
}