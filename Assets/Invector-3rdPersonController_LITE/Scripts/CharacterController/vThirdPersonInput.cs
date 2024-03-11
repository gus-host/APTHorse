using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;


public enum PlatformType
{
    PC,
    Mobile,
    Console
}


namespace Invector.vCharacterController
{
    public class vThirdPersonInput : MonoBehaviourPunCallbacks
    {
        #region Variables       
        
        public PlatformType currentPlatform;
        
        [Header("Controller Input")]
        public string horizontalInput = "Horizontal";
        public string verticallInput = "Vertical";
        public KeyCode jumpInput = KeyCode.Space;
        public KeyCode strafeInput = KeyCode.Tab;
        public KeyCode sprintInput = KeyCode.LeftShift;
        public KeyCode attackOne = KeyCode.Q;
        public KeyCode attackTwo = KeyCode.E;
        public KeyCode shield = KeyCode.R;

        [Header("Camera Input")]
        public string rotateCameraXInput = "Mouse X";
        public string rotateCameraYInput = "Mouse Y";

        [HideInInspector] public vThirdPersonController cc;
        [HideInInspector] public vThirdPersonCamera tpCamera;
        [HideInInspector] public Camera cameraMain;

        [Header("JoyStick")]
        public FixedJoystick _fixedJoystick;

        [Header("Mobile Input")] 
        private Vector3 mobileMovementInput;
        
        #region Bool

        [Header("Bool")]
        public bool _localPlayer = false;

        public bool _isAttackingOne;
        public bool _isAttackingTwo;
        
        public bool localPlayer
        {
            get
            {
                return _localPlayer;
            }
            set
            {
                _localPlayer = value;
            }
        }

        #endregion

        
        #endregion


        #region UI
        
        public PvPPlayerUI _playerUI;

        #endregion

        protected virtual void Start()
        {
            InitilizeController();
            InitializeTpCamera();
            localPlayer = photonView.IsMine;
            /*// Detect the current platform and set the currentPlatform variable accordingly
            #if UNITY_STANDALONE
                currentPlatform = PlatformType.PC;
            #elif UNITY_ANDROID || UNITY_IOS
                currentPlatform = PlatformType.Mobile;
            #elif UNITY_XBOXONE || UNITY_PS4 || UNITY_SWITCH
                currentPlatform = PlatformType.Console;
            #else
                currentPlatform = PlatformType.PC; // Default to PC if the platform is not recognized
            #endif*/

            // Use the currentPlatform variable to perform platform-specific actions or checks
            switch (currentPlatform)
            {
                case PlatformType.PC:
                    Debug.Log("Current platform: PC");
                    // Add PC-specific behavior here
                    break;
                case PlatformType.Mobile:
                    Debug.Log("Current platform: Mobile");
                    // Add Mobile-specific behavior here
                    break;
                case PlatformType.Console:
                    Debug.Log("Current platform: Console");
                    // Add Console-specific behavior here
                    break;
            }
            #region SetListeners

            if (currentPlatform == PlatformType.Mobile)
            {
                _playerUI._firstAttack.onClick.AddListener(GetFirstAttackInput);
                _playerUI._secondAttack.onClick.AddListener(GetSecondAttackInput);
                _playerUI._jump.onClick.AddListener(JumpInput);
                _playerUI._shield.onClick.AddListener(ShieldUP);
                Debug.Log("Listeners has been set correctly");
            }

            #endregion
            
        }

        /*[ServerCallback]*/
        protected virtual void FixedUpdate()
        {
            cc.UpdateMotor();               // updates the ThirdPersonMotor methods
            cc.ControlLocomotionType();     // handle the controller locomotion type and movespeed
            cc.ControlRotationType();       // handle the controller rotation type
        }

        //GetMobile Input
        private void GetInputValue()
        {
            mobileMovementInput = new Vector3(_fixedJoystick.Horizontal, 0, _fixedJoystick.Vertical);
        }

        /*[ServerCallback]*/
        protected virtual void Update()
        {
            InputHandle();                  // update the input methods
            cc.UpdateAnimator();            // updates the Animator Parameters
        }

        
        public virtual void OnAnimatorMove()
        {
            cc.ControlAnimatorRootMotion(); // handle root motion animations 
        }

        #region Basic Locomotion Inputs

        protected virtual void InitilizeController()
        {
            cc = GetComponent<vThirdPersonController>();
            _localPlayer = cc.localPlayer;
            if (cc != null)
                cc.Init();
        }

        protected virtual void InitializeTpCamera()
        {
            if(!photonView.IsMine){return;}
            if (tpCamera == null)
            {
                tpCamera = FindObjectOfType<vThirdPersonCamera>();
                if (tpCamera == null)
                    return;
                if (tpCamera)
                {
                    tpCamera.SetMainTarget(this.transform);
                    tpCamera.Init();
                }
            }
        }
    
        /*[Server]*/
        protected virtual void InputHandle()
        {
            MoveInput();
            CameraInput();
            SprintInput();
            StrafeInput();
            GetInputValue();
            
            if (currentPlatform != PlatformType.Mobile)
            {
                GetFirstAttackInput();
                GetSecondAttackInput();
                JumpInput();
                ShieldUP();
            }
        }

        private void ShieldUP()
        {
            if (Input.GetKeyDown(shield))
            {
                cc.ShieldUp();
            }
            else if(currentPlatform == PlatformType.Mobile)
            {
                cc.ShieldUp();
            }
        }

        private void GetFirstAttackInput()
        {
            if(Input.GetKeyDown(attackOne) && !_isAttackingOne)
            {
                _isAttackingOne = true;
                StartCoroutine(AttackingOneCooldown());
                cc.AttackOne();
                Debug.Log("Attacking through Pc");
            }else if (currentPlatform == PlatformType.Mobile && !_isAttackingOne)
            {
                _isAttackingOne = true;
                StartCoroutine(AttackingOneCooldown());
                cc.AttackOne();
                Debug.Log("Attacking through mobile");
            }
        }

        IEnumerator AttackingOneCooldown()
        {
            yield return new WaitForSeconds(4f);
            _isAttackingOne = false;
        }
        IEnumerator AttackingTwoCooldown()
        {
            yield return new WaitForSeconds(4f);
            _isAttackingOne = false;
        }
        private void GetSecondAttackInput()
        {
            if (Input.GetKeyDown(attackTwo) && !_isAttackingTwo)
            {
                _isAttackingTwo = true;
                StartCoroutine(AttackingTwoCooldown());
                cc.AttackTwo();
                Debug.Log("Attacking through Pc");
                return;
            }else if (currentPlatform == PlatformType.Mobile && !_isAttackingTwo)
            {
                _isAttackingTwo = true;
                StartCoroutine(AttackingTwoCooldown());
                cc.AttackTwo();
                Debug.Log("Attacking through mobile");
            }
        }

        public virtual void MoveInput()
        {
            if (currentPlatform == PlatformType.PC)
            {
                cc.input.x = Input.GetAxis(horizontalInput);
                cc.input.z = Input.GetAxis(verticallInput);
            }else if (currentPlatform == PlatformType.Mobile)
            {
                cc.input.x = mobileMovementInput.x;
                cc.input.z = mobileMovementInput.z;
            }
         
        }

        protected virtual void CameraInput()
        {
            if (!cameraMain)
            {
                if (!Camera.main) Debug.Log("Missing a Camera with the tag MainCamera, please add one.");
                else
                {
                    cameraMain = Camera.main;
                    cc.rotateTarget = cameraMain.transform;
                }
            }

            if (cameraMain)
            {
                cc.UpdateMoveDirection(cameraMain.transform);
            }

            if (tpCamera == null)
                return;

            var Y = Input.GetAxis(rotateCameraYInput);
            var X = Input.GetAxis(rotateCameraXInput);

            tpCamera.RotateCamera(X, Y);
        }

        protected virtual void StrafeInput()
        {
            if (Input.GetKeyDown(strafeInput))
                cc.Strafe();
        }

        protected virtual void SprintInput()
        {
            if (Input.GetKeyDown(sprintInput))
                cc.Sprint(true);
            else if (Input.GetKeyUp(sprintInput))
                cc.Sprint(false);
        }

        /// <summary>
        /// Conditions to trigger the Jump animation & behavior
        /// </summary>
        /// <returns></returns>
        protected virtual bool JumpConditions()
        {
            return cc.isGrounded && cc.GroundAngle() < cc.slopeLimit && !cc.isJumping && !cc.stopMove;
        }

        /// <summary>
        /// Input to trigger the Jump 
        /// </summary>
        protected virtual void JumpInput()
        {
            if (Input.GetKeyDown(jumpInput) && JumpConditions())
                cc.Jump();
            else if(currentPlatform == PlatformType.Mobile && JumpConditions())
                cc.Jump();
        }

        #endregion       
    }
}