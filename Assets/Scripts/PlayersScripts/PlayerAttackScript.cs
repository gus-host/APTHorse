using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerAttackScript : MonoBehaviour
{
    private CharacterAnimation playerAnimation;
    public GameObject attackPoint;

    private PlayerShield shield;

    private bool isMobile = false;
    private bool _animatingVFX = false;

    private GameObject nearestEnemy;
    public GameObject SliceVFX;
    public GameObject SliceUpVFX;
    public GameObject _swordTrail;

    [Header("......TrailandParticleEffect......")]
    public TrailRenderer P_SlashTrail;

    private void Awake()
    {
        playerAnimation = GetComponent<CharacterAnimation>();
        shield = GetComponent<PlayerShield>();

#if UNITY_ANDROID
        isMobile = true;
#endif
    }

    // Start is called before the first frame update
    void Start()
    {
        init();
       
    }

    // Update is called once per frame
    void Update()
    {
        
        KeyboardInput();
    }

    #region PcInput
    void KeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !GetComponent<IntrantThirdPersonController>()._paused)
        {
            onSwordAttack_click();
            
        }
        if (Input.GetKeyDown(KeyCode.E) && !GetComponent<IntrantThirdPersonController>()._paused)
        {
            OnSwordHorizontalAttack_click();
        }
    }

    IEnumerator Slice()
    {
        _animatingVFX = true;
        _swordTrail.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        _swordTrail.SetActive(false);
        SliceVFX.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        SliceVFX.SetActive(false);
        _animatingVFX = false;
    }

    IEnumerator SliceUp()
    {
        _animatingVFX = true;
        yield return new WaitForSeconds(0.5f);
        SliceUpVFX.SetActive(true);
        _swordTrail.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        SliceUpVFX.SetActive(false);
        _swordTrail.SetActive(false);
        _animatingVFX = false;
    }
    #endregion
    private void init()
    {
        playerAnimation = GetComponent<CharacterAnimation>();
        PlayerUI.instance._swordAttack_btn.onClick.AddListener(onSwordAttack_click);
        PlayerUI.instance._sworkdHorizontalAttack_btn.onClick.AddListener(OnSwordHorizontalAttack_click);
        
    }

    void Activate_AttackPoint()                                        //Reference PlayerAttackanimation Event
    {
        //  attackPoint.GetComponent<AttackDamage>().enabled = true;
        // Find the nearest enemy and rotate towards it
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float minDistance = Mathf.Infinity;
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = enemy;
            }
        }
        if (nearestEnemy != null)
        {

            Vector3 direction = nearestEnemy.transform.position - transform.position;
            direction.y = 0f;
            transform.rotation = Quaternion.LookRotation(direction);
        }

        Invoke("Deactivate_AttackPoint", 1f);
    }                 

    void Deactivate_AttackPoint()                              //Reference in ActivateAttackPoint
    {
        if (attackPoint.activeInHierarchy)
        {
           // attackPoint.GetComponent<AttackDamage>().enabled = false;
            attackPoint.SetActive(false);
           
        }
    }

    public void onSwordAttack_click()
    {
        if (!GetComponent<IntrantThirdPersonController>()._paused)
        {
            playerAnimation.Attack1();
            Activate_AttackPoint();
            if (!_animatingVFX)
            {
                StartCoroutine(Slice());
            }
        }
/*        else
        {
            playerAnimation.Attack1();
            Activate_AttackPoint();
        }*/
        // Debug.Log("Attack");
    }
    public void OnSwordHorizontalAttack_click()
    {
        if (!GetComponent<IntrantThirdPersonController>()._paused)
        {
            // Debug.Log("Attack1");
            playerAnimation.Attack2();
            Activate_AttackPoint();
            if (!_animatingVFX)
            {
                StartCoroutine(SliceUp());
            }
        }
    }

    public void ActivateAttackPoint()
    {
        attackPoint.SetActive(true);
    }

    #region Effect

    public void P_SwordSlashEffectOn()
    {
        P_SlashTrail.gameObject.SetActive(true);
    }
    public void P_SwordSlashEffectOff()
    {
        P_SlashTrail.gameObject.SetActive(false);
       
    }

    #endregion

}
