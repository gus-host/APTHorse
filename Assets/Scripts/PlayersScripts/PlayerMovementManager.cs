using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementManager : MonoBehaviour
{
    public static PlayerMovementManager instance;
    [Header(".....ScriptReference.......")]
    #region .....ScriptReference.......
    public InputType inputType;
    #endregion


    private CharacterController _charController;
    private CharacterAnimation _playerAnimation;
    public Animator _charAnimation;
    public Rigidbody rb;

    [Header("........Playermovement.......")]
    #region "........Playermovement......."
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 10f;
    public float movement_Speed = 3f;
    public float gravity = 9.8f;
    public float rotation_Speed = 0.15f;
    public float rotateDegreePerSecond = 180f;
    private Vector3 playerVelocity;
    private Vector3 moveDirection;
    private float verticalVelocity;
    public bool isJumping = false;
    private float currentSpeed;
    public float jumpHeight = 2.0f;
    public float gravityValue = -9.81f;
    [HideInInspector]
    public Vector3 P_movedir;
    [HideInInspector]
    public Vector3 I_movementinput;
    //Jump



    #endregion


    [Header("........MobileJoyStick and UI...........")]
    public FixedJoystick _fixedJoystick;
    public Collider _PlayerShield;


    [Header("........VFX...........")]
    public GameObject _shieldfx;
    public GameObject _healingfx;
    public GameObject _buff;
    public GameObject _debuff;

    [Header("Materials")]
    public Material _alkemmanaSwitchonMat;

    [Header("Count")]
    public int _switchedOnCount = 0;
    public int _collectedTherCode = 0;


    [Header("Panel")]
    public GameObject _grimoirPanel;

    public GameObject _canvas;
    
    
    #region Bool
    
    [Header("Bools")]
    public bool _shieldUp;
    public bool isMobilePlatform;
    public bool _paused = false;
    public bool _gameFinished = false;
    public bool _isAnimating = false;
    public bool _receivedTheraBox = false;
    public bool _receivedTheraCode = false;
    public bool _receivedAsimanaCodex = false;
    public bool _placedTheraCode = false;
    public bool _jumpButtonPressed = false;
    public bool _mainFrameEnabled = false;
    public bool _beenInAlkemanaChamber = false;
    public bool localPlayer = false;

    public bool _localPlayer
    {
        get
        {
            return localPlayer;
        }
        set
        {
            localPlayer = value;
            Debug.Log("Local player Set");
        }
    }
    
    #endregion

    #region Actions

    public static event Action<PlayerMovementManager> ServerOnPlayerSpawned;
    public static event Action<PlayerMovementManager> ServerOnPlayerDespawned;
    public static event Action<PlayerMovementManager> AuthorityOnPlayerSpawned;
    public static event Action<PlayerMovementManager> AuthorityOnPlayerDespawned;

    #endregion
    
    #region monobehaviour
    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();


        switch (inputType)
        {
            case InputType.Mobile:
                isMobilePlatform = true;
                break;
            case InputType.Pc:
                isMobilePlatform = false;
                break;
        }
        if(isMobilePlatform)
        {
            jumpHeight = jumpHeight / 2;
            Debug.LogError("JumpHieght "+ jumpHeight);
        }
    }

    private void Disable()
    {
        //Turnoff break button in start
        PlayerUI.instance._wallBreakbtn.gameObject.SetActive(false);
    }

    void Start()
    {
        #region Listeners

        PlayerUI.instance._shield_btn.onClick.AddListener(Shield);
        PlayerUI.instance._jump_btn.onClick.AddListener(JumpHandlerAndroid);
        PlayerUI.instance._breakPotionCollect.onClick.AddListener(AtivateBreakWallUI);
        PlayerUI.instance._wallBreakbtn.onClick.AddListener(BreakWall);

        #endregion

        Init();
        Disable();
    }


    void Update()
    {
        PlayerMovementInput();
        if (!isMobilePlatform && !_paused)
        {
            JumpHandler();
        }
        if (isMobilePlatform && !_paused) {

            JumpHandlerAndroid();
        }

        if (!_jumpButtonPressed)
        {
            _jumpButtonPressed = false;
        }
        if(_paused)
        {
            //_charAnimation.SetBool("isWalk", false);
            I_movementinput = Vector3.zero;
            _playerAnimation.WalkAnimation(I_movementinput);
            _isAnimating = true;
        }else if(!_paused)
        {
            _isAnimating = false;
        }

        //Handle Exception for Theracode
        if (_collectedTherCode >= 6)
        {
            _receivedTheraCode = true;
            _collectedTherCode = GameManager.instance._maxTheraCode + 1;
        }
    }

    #endregion

    #region Jump Handler
    private void JumpHandler()
    {

        try
        {
            if (_charController.isGrounded)
            {
                // Check for jump input
                if (Input.GetButtonDown("Jump") || isMobilePlatform)
                {
                    _playerAnimation.PerformJump();
                    // Calculate the jump velocity
                    StartCoroutine(Jump());
                }
            }

            // Apply gravity to the character
            playerVelocity.y += gravityValue * Time.deltaTime;

            // Move the character controller
            _charController.Move(playerVelocity * Time.deltaTime);

            // Check if the character has landed after jumping
            if (_charController.isGrounded && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
                isJumping = false;
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex + " Jump Failed");
        }
        // Check for jump input
    }

    IEnumerator Jump()
    {
        Debug.LogWarning("Jumping...");
        yield return new WaitForSeconds(1f);
        playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        isJumping = true;
        Debug.LogWarning("Jumped");
    }
    IEnumerator JumpAndroid()
    {
        Debug.LogWarning("Jumping...");
        yield return new WaitForSeconds(1f);
        playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        isJumping = false;
        _jumpButtonPressed = false;
        Debug.LogWarning("Jumped");
    }

    private void JumpHandlerAndroid()
    {
/*        if(isJumping)
        {
            return;
        }*/
        try
        {
            if (_charController.isGrounded)
            {
                // Check for jump input
                if (_jumpButtonPressed)
                {
                    if(!isJumping)
                    {
                        _playerAnimation.PerformJump();
                    }
                    isJumping = true;
                    // Calculate the jump velocity
                    StartCoroutine(JumpAndroid());
                }
            }

            // Apply gravity to the character
            playerVelocity.y += gravityValue * Time.deltaTime;

            // Move the character controller
            _charController.Move(playerVelocity * Time.deltaTime);

            // Check if the character has landed after jumping
            if (_charController.isGrounded && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex + " Jump Failed");
        }
        // Check for jump input

    }

    #endregion

    void Init()
    {
        _charController = GetComponent<CharacterController>();
        _playerAnimation = GetComponent<CharacterAnimation>();
        PlayerUI.instance._grimoire.onClick.AddListener(() => { Pause(); });
        PlayerUI.instance._grimoirBackBtn.onClick.AddListener(() => { Resume(); });
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        _grimoirPanel.SetActive(false);
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        _grimoirPanel.SetActive(true);
    }

    #region Mobile and Pc Inputs
    void PlayerMovementInput()
    {
        if (!_paused)
        {
            switch (inputType)
            {
                case InputType.Mobile:
                    isMobilePlatform = true;
                    break;
                case InputType.Pc:
                    isMobilePlatform = false;
                    break;
            }
            if (isMobilePlatform)
            {
                //joystick input
                I_movementinput = new Vector3(_fixedJoystick.Horizontal, 0, _fixedJoystick.Vertical);
                PlayerMovement();

            }
            else if (!isMobilePlatform)
            {
                //keyboard input
                I_movementinput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                PlayerMovement();
            }
            if (!isMobilePlatform)
            {
                if (Input.GetKeyDown(KeyCode.R) && !_shieldUp && _paused)
                {
                    StartCoroutine(ActivateShield());
                }
            }
        }
    }
    #endregion


    #region Shield ability
    public void Shield()
    {
        if(!_paused)
        {
            StartCoroutine(ActivateShield());
        }
    }

    IEnumerator ActivateShield()
    {
        if (_shieldUp)
        {
            yield return null;
        }
        Debug.LogWarning("Shield Activated");
        _shieldUp = true;
        PlayerUI.instance._shield_btn.interactable = false;
        _shieldfx.SetActive(true);
        _playerAnimation.Shield();
        _PlayerShield.enabled = true;
        this.gameObject.layer = LayerMask.NameToLayer("Default");
        yield return new WaitForSeconds(2f);
        this.gameObject.layer = LayerMask.NameToLayer("Player");
        _shieldfx.SetActive(false);
        _PlayerShield.enabled = false;
        _shieldUp = false;
        PlayerUI.instance._shield_btn.interactable = true;
    }
    #endregion


    #region PlayerMovement and Animation
    void PlayerMovement()
    {
        if (I_movementinput != Vector3.zero && !_paused)
        {

            P_movedir = Vector3.MoveTowards(transform.forward, I_movementinput, 100);
            Quaternion targetRotation = Quaternion.LookRotation(P_movedir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotation_Speed * Time.deltaTime);


            P_movedir *= movement_Speed;
            P_movedir.y -= gravity * Time.deltaTime;
            _charController.Move(P_movedir * Time.deltaTime);
        }
        _playerAnimation.WalkAnimation(I_movementinput);
    }
    #endregion


    #region Reload Game
    public void Reload(string _sceneName, GameObject _loadingScreen, Image _fill)
    {
        StartCoroutine(LoadSceneAsync(_sceneName, _loadingScreen, _fill));
    }
    private IEnumerator LoadSceneAsync(string sceneName, GameObject _loadingScreen, Image _fill)
    {
        yield return new WaitForSeconds(2.5f);
        if (_loadingScreen)
        {
            _loadingScreen.SetActive(true);
        }
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            // You can display a loading progress bar or perform other tasks here if needed
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            if (_fill != null)
            {
                _fill.fillAmount = progress;
            }
            Debug.LogError("Loading progress: " + progress * 100 + "%");

            yield return null;
        }
    }
    #endregion

    #region Break Wall

    public void AtivateBreakWallUI()
    {
        PlayerUI.instance._wallBreakbtn.gameObject.SetActive(true);
        PlayerUI.instance._breakPotionCollect.gameObject.SetActive(false);
    }

    private void BreakWall()
    {
        Sword.instance._canBreak = true;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag(Tags.ASIMANAS_CODEX))
        {
            _receivedAsimanaCodex = true;
            LeanTween.scale(other.gameObject,Vector3.zero,1f);
            Destroy(other.gameObject,1f);
        }

        if (other.CompareTag("PointC"))
        {
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            _beenInAlkemanaChamber = true;
        }
       
        if (other.gameObject.CompareTag("WallBreakSpell"))
        {
            PlayerUI.instance._breakPotionCollect.gameObject.SetActive(true);
            Destroy(other.gameObject,0.1f);
        }

        if (other.gameObject.CompareTag(MapPoints.AlkemannaHotspotSwitch) && _receivedTheraCode && _collectedTherCode == GameManager.instance._maxTheraCode+1 &&_beenInAlkemanaChamber)
        {
            PlayerUI.instance._switchOnAlkemmana.gameObject.SetActive(true);
            PlayerUI.instance._switchOnAlkemmana.onClick.RemoveAllListeners();
            PlayerUI.instance._switchOnAlkemmana.onClick.AddListener(() => ChangeMat(other.gameObject.GetComponent<MeshRenderer>(), _alkemmanaSwitchonMat));
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
        }

        if (other.gameObject.CompareTag(Tags.THERABOX_TAG))
        {
            Destroy(other.gameObject,0.3f);
        }

        if (other.gameObject.CompareTag(Tags.TheraCode_TAG))
        {
            PlayerUI.instance._collect.gameObject.SetActive(true);
            PlayerUI.instance._collect.onClick.RemoveAllListeners();
            PlayerUI.instance._collect.onClick.AddListener(() => { CollectCode(other.gameObject); });
        }

        if (other.gameObject.CompareTag(Tags.HEALTH_TAG))
        {
            StartCoroutine(IncreaseHealth(other.gameObject));
        }
    }

    IEnumerator IncreaseHealth(GameObject _obj)
    {
        LeanTween.scale(_obj, new Vector3(0, 0, 0), 2f);
        Destroy(_obj, 2f);
        while (GetComponent<HealthScript>().Health<100)
        {
            yield return new WaitForSeconds(0.1f);
            GetComponent<HealthScript>().Increasehealth(1);
        }
    }

    private void CollectCode(GameObject _code)
    {
        //Collect thera stray code 

        Destroy(_code);
        _collectedTherCode++;
        PlayerUI.instance._collect.gameObject.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
/*        if (other.gameObject.CompareTag(Tags.ENEMY_TAG))
        {
            _paused = true;
        }*/
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("WallBreakSpell"))
        {
            PlayerUI.instance.gameObject.SetActive(true);
        }

        if (other.gameObject.CompareTag(Tags.ENEMY_TAG))
        {
            _paused = false;
        }

        if (other.gameObject.CompareTag(Tags.TheraCode_TAG))
        {
            PlayerUI.instance._collect.gameObject.SetActive(false);
        }

    }
    #endregion

    public void ChangeMat(MeshRenderer _mesh, Material _mat)
    {
        _mesh.material = _mat;
        PlayerUI.instance._switchOnAlkemmana.gameObject.SetActive(false);
        _switchedOnCount++;
    }
}