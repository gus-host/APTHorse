using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Invector.vCharacterController
{
    public class vThirdPersonController : vThirdPersonAnimator
    {
        [Header("VFX")] 
        public GameObject _shieldFx;
        public GameObject _spawnFx;
        public Collider _hammerCollider;
        public Transform _shieldFxParent;
        private GameObject _shieldFxInstance = null;
        
        private bool isShieldActive = false;
        private bool isAttacking = false;
        private float shieldCooldown = 2.5f;
        private static readonly int IsAttackingOneHash = Animator.StringToHash("AttackOne");
        private static readonly int IsAttackingTwoHash = Animator.StringToHash("AttackTwo");

        [Header("Bool")]
        public bool _localPlayer = false;
        public bool _paused = true;
        public bool _isPuzzleMode = false;
        public bool _playerDied = false;
        public bool _playerWon = false;
        


        [Header("Screen")] 
        public GameObject _gameOver;
        public GameObject _winScreen;
        public GameObject _preparingForgame;
        public TMP_Text _name;

        [Header("Screen")]
        public GameObject _sliceOnef;
        public GameObject _sliceOneS;
        public GameObject _sliceOneT;
        public GameObject _sliceTwof;
        public GameObject _sliceTwoT;
        public GameObject _attackAura;


        #region Actions

        public event Action InitiatedAttackOne;
        public event Action InitiatedAttackTwo;

        #endregion

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
        
        public bool isPuzzleMode
        {
            get
            {
                return _isPuzzleMode;
            }
            set
            {
                _isPuzzleMode = value;
            }
        }
        
        public bool pause
        {
            get
            {
                return _paused;
            }
            set
            {
                _paused = value;
            }
        }


        private void Awake()
        {
            var platform = PlayerPrefs.GetInt("Platform");
            Debug.LogError($"platform {platform}");
            if(_name.gameObject !=null)
            {
                _name.text = photonView.Owner.NickName;
            }

            if (platform == 0)
                GetComponent<vThirdPersonInput>().currentPlatform = PlatformType.Mobile;
            else if (platform == 1) GetComponent<vThirdPersonInput>().currentPlatform = PlatformType.PC;
        }
 
        private void Start()
        {
            GetComponent<PvPPlayerHealth>().ServerOnDie += PlayerDied;
            localPlayer = photonView.IsMine;
            if (photonView.IsMine)
            {
                _gameOver =  FindObjectOfType<PvPGameOver>().gameObject;
                //_winScreen = FindObjectOfType<PvPWinScreen>().gameObject;
                _preparingForgame = FindObjectOfType<Preparinggame>().gameObject;
                FindObjectOfType<CameraFollowingPlayer>().Player = gameObject;
                StartCoroutine(DisableScreens());
            }

        }

        private IEnumerator DisableScreens()
        {
            yield return new WaitForSeconds(1.5f);
            _gameOver.SetActive(false);
            //_winScreen.SetActive(false);
            _preparingForgame.SetActive(false);
        }

        private void PlayerDied()
        {
            // Update the player's state on the server
            ServerPlayerDied();

            // Notify all clients that the player has died
            photonView.RPC("RpcPlayerDied",RpcTarget.All);
            photonView.RPC("RpcNotifyPlayerWon",RpcTarget.All);
            PvPPlayerHealth[] _playerHealths = FindObjectsOfType<PvPPlayerHealth>();
            foreach (var playerHealth in _playerHealths)
            {
                if(playerHealth.currentHealth>0)
                {
                  //playerHealth.GetComponent<vThirdPersonController>().HandlePlayerWon(false, true);
                }
            }
        }

// On all clients
        [PunRPC]
        private void RpcPlayerDied()
        {
            // This method runs on all clients when the player dies
            Debug.LogError($"Handle Player Death {photonView.ViewID}");
            _playerDied = true;
            animator.SetBool("Died", true);
            //HandlePlayerDeath(false, true);
        }
        
        private void ServerPlayerDied()
        {
            _playerDied = true;
        }
        [PunRPC]
        private void RpcNotifyPlayerWon()
        {
            // This method runs on all clients when the player dies
            Debug.LogError($"Rpc notify won {photonView.ViewID}");
            if (!_playerDied)
            {
                //HandlePlayerWon(false, true);                
            }
        }

/*        private void HandlePlayerDeath(bool oldval, bool newbool)
        {
            if (newbool && photonView.IsMine)
            {
                _gameOver.SetActive(true);
                PhotonNetwork.LeaveRoom();
                SceneManager.instance.LoadScene("Lobby");
            }    
        }*/
        

/*        private void HandlePlayerWon(bool oldval, bool newbool)
        {
            Debug.LogError("Player Won");
            if (newbool && photonView.IsMine)
            {
                _playerWon = true;
                //_winScreen.SetActive(true);
                pause = false;
                //GetComponent<PvPPlayerPuzzleManager>()._puzzleParent.gameObject.SetActive(false);
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PvPSceneManager.instance._surrender.GetComponentInChildren<TMP_Text>().text = "RETURN TO LOBBY";
                Debug.LogError("Player Won");
            }
        }*/
        private void OnDestroy()
        {
            GetComponent<PvPPlayerHealth>().ServerOnDie -= PlayerDied;
        }
        
        public virtual void ControlAnimatorRootMotion()
        {
            if(!localPlayer || _playerDied || pause){return;}
            
            if (!this.enabled) return;

            if (inputSmooth == Vector3.zero)
            {
                transform.position = animator.rootPosition;
                transform.rotation = animator.rootRotation;
            }

            if (useRootMotion)
            {
                MoveCharacter(moveDirection);
            }
        }

        public virtual void ControlLocomotionType()
        {
            if(!localPlayer || _playerDied || pause){return;}
            
            if (lockMovement) return;

            if (locomotionType.Equals(LocomotionType.FreeWithStrafe) && !isStrafing || locomotionType.Equals(LocomotionType.OnlyFree))
            {
                SetControllerMoveSpeed(freeSpeed);
                SetAnimatorMoveSpeed(freeSpeed);
            }
            else if (locomotionType.Equals(LocomotionType.OnlyStrafe) || locomotionType.Equals(LocomotionType.FreeWithStrafe) && isStrafing)
            {
                isStrafing = true;
                SetControllerMoveSpeed(strafeSpeed);
                SetAnimatorMoveSpeed(strafeSpeed);
            }

            if (!useRootMotion)
            {
                MoveCharacter(moveDirection);
            }
        }

        public virtual void ControlRotationType()
        {
            if(!localPlayer || _playerDied || pause){return;}
            
            if (lockRotation) return;

            bool validInput = input != Vector3.zero || (isStrafing ? strafeSpeed.rotateWithCamera : freeSpeed.rotateWithCamera);

            if (validInput)
            {
                // calculate input smooth
                inputSmooth = Vector3.Lerp(inputSmooth, input, (isStrafing ? strafeSpeed.movementSmooth : freeSpeed.movementSmooth) * Time.deltaTime);

                Vector3 dir = (isStrafing && (!isSprinting || sprintOnlyFree == false) || (freeSpeed.rotateWithCamera && input == Vector3.zero)) && rotateTarget ? rotateTarget.forward : moveDirection;
                RotateToDirection(dir);
            }
        }

        public virtual void UpdateMoveDirection(Transform referenceTransform = null)
        {
            if(!localPlayer || _playerDied || pause){return;}
            if(pause)return;
            if (input.magnitude <= 0.01)
            {
                moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, (isStrafing ? strafeSpeed.movementSmooth : freeSpeed.movementSmooth) * Time.deltaTime);
                return;
            }

            if (referenceTransform && !rotateByWorld)
            {
                //get the right-facing direction of the referenceTransform
                var right = referenceTransform.right;
                right.y = 0;
                //get the forward direction relative to referenceTransform Right
                var forward = Quaternion.AngleAxis(-90, Vector3.up) * right;
                // determine the direction the player will face based on input and the referenceTransform's right and forward directions
                moveDirection = (inputSmooth.x * right) + (inputSmooth.z * forward);
            }
            else
            {
                moveDirection = new Vector3(inputSmooth.x, 0, inputSmooth.z);
            }
        }

        public virtual void Sprint(bool value)
        {
            if(!localPlayer || _playerDied || pause){return;}

            var sprintConditions = (input.sqrMagnitude > 0.1f && isGrounded &&
                !(isStrafing && !strafeSpeed.walkByDefault && (horizontalSpeed >= 0.5 || horizontalSpeed <= -0.5 || verticalSpeed <= 0.1f)));

            if (value && sprintConditions)
            {
                if (input.sqrMagnitude > 0.1f)
                {
                    if (isGrounded && useContinuousSprint)
                    {
                        isSprinting = !isSprinting;
                    }
                    else if (!isSprinting)
                    {
                        isSprinting = true;
                    }
                }
                else if (!useContinuousSprint && isSprinting)
                {
                    isSprinting = false;
                }
            }
            else if (isSprinting)
            {
                isSprinting = false;
            }
        }

        public virtual void Strafe()
        {
            if(!localPlayer || _playerDied || pause){return;}

            isStrafing = !isStrafing;
        }

        public virtual void Jump()
        {
            if(!localPlayer || _playerDied || pause){return;}

            // trigger jump behaviour
            jumpCounter = jumpTimer;
            isJumping = true;

            // trigger jump animations
            if (input.sqrMagnitude < 0.1f)
                animator.CrossFadeInFixedTime("Jump", 0.1f);
            else
                animator.CrossFadeInFixedTime("JumpMove", .2f);
        }

        public virtual void AttackOne()
        {
            // Set the animator parameter for the local player
/*            if(pause || _playerDied)return;
            if (photonView.IsMine)
            {
                if (isPuzzleMode)
                {
                    InitiatedAttackOne?.Invoke();
                }
                animator.SetBool(IsAttackingOneHash, true);
                isAttacking = true;
                StartCoroutine(DisableAnimation(IsAttackingOneHash));
                CmdAttackOne();
                StartCoroutine(WeaponCollider(1));
                photonView.RPC("TriggerVfxSignal", RpcTarget.All,1);
                Debug.Log("AttackOne");
            }*/
        }

        [PunRPC]
        public void TriggerVfxSignal(int val)
        {
            StartCoroutine(TriggerFx(val));
        }


        private IEnumerator TriggerFx(int val)
        {
            Debug.LogError("TriggerFx");
            if (val == 1)
            {
                _attackAura.SetActive(true);
                yield return new WaitForSeconds(0.25f);
                _sliceOnef.SetActive(true);
                yield return new WaitForSeconds(0.9f);
                _sliceOnef.SetActive(false);
                _sliceOneS.SetActive(true);
                yield return new WaitForSeconds(1.05f);
                _sliceOneS.SetActive(false);
                _sliceOneT.SetActive(true);
                yield return new WaitForSeconds(1f);
                _sliceOneT.SetActive(false);
                _attackAura.SetActive(false);

            }else if (val == 2)
            {
                _attackAura.SetActive(true);
                yield return new WaitForSeconds(0.15f);
                _sliceTwof.SetActive(true);
                yield return new WaitForSeconds(1.05f);
                _sliceTwof.SetActive(false);
                _sliceTwoT.SetActive(true);
                yield return new WaitForSeconds(1f);
                _sliceTwoT.SetActive(false);
                _attackAura.SetActive(false);
            }
            yield return null;
        }

        public virtual void AttackTwo()
        {
           /* if(pause || _playerDied)return;
            // Set the animator parameter for the local player
            if (photonView.IsMine)
            {
                if (isPuzzleMode)
                {
                    InitiatedAttackTwo?.Invoke();
                }
                animator.SetBool(IsAttackingTwoHash, true);
                isAttacking = true;
                StartCoroutine(DisableAnimation(IsAttackingTwoHash));
                CmdAttackTwo();
                StartCoroutine(WeaponCollider(2));
                photonView.RPC("TriggerVfxSignal", RpcTarget.All, 2);
                Debug.Log("AttackTwo");
            }*/

        }

        [PunRPC]
        private IEnumerator WeaponCollider(int val)
        {
            if(val == 1)
            {
                Debug.Log($"Toggling collider with val {val}");
                yield return new WaitForSeconds(0.15f);
                _hammerCollider.GetComponent<PvPHammer>().EnableCollider(true);
                yield return new WaitForSeconds(2.2f);
                _hammerCollider.GetComponent<PvPHammer>().EnableCollider(false);
            }
            else if(val == 2)
            {
                Debug.Log($"Toggling collider with val {val}");
                yield return new WaitForSeconds(0.15f);
                _hammerCollider.GetComponent<PvPHammer>().EnableCollider(true);
                yield return new WaitForSeconds(1.2f);
                _hammerCollider.GetComponent<PvPHammer>().EnableCollider(false);
            }

            yield return null;
        }

        IEnumerator DisableAnimation(int AttackOneHash)
        {
            yield return new WaitForSeconds(1f);
            animator.SetBool(AttackOneHash, false);
            isAttacking = false;
        }
        
        #region Server
        
        private void FixedUpdate()
        {
            if (isAttacking)
            {
                RpcSetHammerColliderState(true);
            }else if (!isAttacking)
            {
                RpcSetHammerColliderState(false);
            }

        }
        
        //Command for attack one animation
        private void CmdAttackOne()
        {
            Debug.Log("Command attack one");
            RpcTriggerAttackOne();
            StartCoroutine(DisableAttackOneAfterDelay());
        }

        //Command for attack two animation
        private void CmdAttackTwo()
        {
            RpcTriggerAttackTwo();
            StartCoroutine(DisableAttackTwoAfterDelay());
        }
        
        //Shield Activate On Server

        public virtual void ShieldUp()
        {
            if(pause || _playerDied || !localPlayer)return;
            
            if (!isShieldActive)
            {
                RpcEnableShield();
            }
        }

        #endregion
        
        #region Client
        
        private void RpcSetHammerColliderState(bool state)
        {
            // This method runs on all clients
            if (photonView.IsMine)
            {
                //_hammerCollider.enabled = state;
            }
        }
        
        private void RpcTriggerAttackOne()
        {
            Debug.Log($"Setting animation for attack one {photonView.IsMine}");
            try
            {
                // Set the animator parameter for other clients (not the local player)
                if (photonView.IsMine && animator != null)
                {
                    isAttacking = true;
                    Debug.Log("Setting animation for attack one");
                    animator.SetBool(IsAttackingOneHash, true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        private void RpcTriggerAttackTwo()
        {
            try
            {
                // Set the animator parameter for other clients (not the local player)
                if (photonView.IsMine && animator != null)
                {
                    isAttacking = true;
                    Debug.Log("Setting animation for attack two");
                    animator.SetBool(IsAttackingTwoHash, true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        private void RpcDisableAttackOneAnimation()
        {
            try
            {
                // Set the animator parameter for other clients (not the local player)
                if (photonView.IsMine && animator != null)
                {
                    animator.SetBool(IsAttackingOneHash, false);
                    isAttacking = false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void RpcDisableAttackTwoAnimation()
        {
            try
            {
                // Set the animator parameter for other clients (not the local player)
                if (photonView.IsMine && animator != null)
                {
                    animator.SetBool(IsAttackingTwoHash, false);
                    isAttacking = false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        private IEnumerator DisableAttackOneAfterDelay()
        {
            yield return new WaitForSeconds(2.15f);
            RpcDisableAttackOneAnimation();
        }

        private IEnumerator DisableAttackTwoAfterDelay()
        {
            yield return new WaitForSeconds(1f);
            RpcDisableAttackTwoAnimation();
        }
        
        private void RpcEnableShield()
        {
            if (_shieldFxInstance == null)
            {
                _shieldFxInstance = Instantiate(_shieldFx, transform.position, transform.rotation, _shieldFxParent);
            }
                
            StartCoroutine(EnableShield());
        }

        IEnumerator EnableShield()
        {
            // Set the shield active flag and enable the VFX
            isShieldActive = true;
            _shieldFxInstance.SetActive(true);

            // Wait for the cooldown duration
            yield return new WaitForSeconds(shieldCooldown);

            // Reset the shield active flag and disable the VFX
            isShieldActive = false;
            _shieldFxInstance.SetActive(false);
        }
        #endregion
    }
    
    
}