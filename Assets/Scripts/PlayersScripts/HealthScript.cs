using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    public static HealthScript instance;
    public static event Action<EnemyController> OnPlayerExit;
    public float Health = 100f;
    public float MaxHealth = 100f;
    public float _fillAmount = 0;
    public float x_Death = -90f;
    private float death_smooth = 0.9f;
    private float rotate_Time = 0.23f;
    public TextMeshProUGUI _playerHealthtxt;

    public GameObject _Player;
    public GameObject GameOver_Panel;
    public GameObject Replay_btn;
    public GameObject Quit_Btn;

    public GameObject DungeonEnvironment;
    public GameObject BossMonster;
    public GameObject _joystick;
    public GameObject attack_btn;

    public GameObject HealthUI_Canvas;
    public GameObject Dead_BloodSplatter;
    public GameObject BloodSplatter_Hit;
    public GameObject BloodVFX;
    public GameObject AttackPoint;
    public GameObject Player;
    public GameObject Enemies;

    public GameObject UI_Canvas;

    private Animator _anim;
    private Animator BloodSplatter_anim;

    public float currentHealth;


    public bool isPlayer;
    private bool playerDied;

    [SerializeField]
    public Image health_UI;


    public CharacterSoundFX soundFX;

    private int enemyDead_Count;

    [Header("Bools")]
    [SerializeField] public bool shieldActivated;
    [SerializeField] public bool IsAlive = true;
    [SerializeField] public bool _isPlayer = false;

    private void Start()
    {
        _anim = gameObject.GetComponent<Animator>();
        BloodSplatter_anim = BloodSplatter_Hit.GetComponent<Animator>();
        IntrantPlayerHealthManager.instance.PlayerOnDie += DisableEnemyBehaviour;
        MaxHealth = Health;
    }

    private void OnDestroy()
    {
        IntrantPlayerHealthManager.instance.PlayerOnDie -= DisableEnemyBehaviour;
    }

    private void Awake()
    {
        GameOver_Panel = GameObject.Find("GameOver_Canvas");
        if (GameOver_Panel != null && this.gameObject.CompareTag("Player"))
        {
            GameOver_Panel.SetActive(false);
        }
        soundFX = GetComponentInChildren<CharacterSoundFX>();
        instance = this;
        IsAlive = true;
    }

    void Update()
    {
        if (IsAlive)
        {
            if (Health <= 0)
            {
                IsAlive = false;
            }
        }
        if (IsAlive != true)
        {
            Debug.LogWarning("Player dead");
            StartCoroutine(DisableBehaviour());
        }
        ShowPlayerStats();
    }

    private void ShowPlayerStats()
    {
        Debug.LogWarning("Current health " + Health.ToString());
        if (Health <= 0)
        {
            Health = 0;
        }
        _playerHealthtxt.text = "HP " + Health.ToString();
    }
    private void DisableEnemyBehaviour()
    {
        StartCoroutine(DisableBehaviour());
    }
    IEnumerator DisableBehaviour()
    {
        OnPlayerExit?.Invoke(GetComponent<EnemyController>());
        if (!_isPlayer)
        {
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<EnemyController>().enabled = false;
        }
        _anim.SetTrigger("Death");
        if (_isPlayer)
        {
            GetComponent<IntrantThirdPersonController>().enabled = false;
        }
        this.gameObject.layer = LayerMask.NameToLayer("Default");
        yield return new WaitForSeconds(1);
        if (_isPlayer)
        {
            GameManager.instance._gameEnded = true;
        }
        GetComponent<Animator>().enabled = false;
        if (GameOver_Panel != null && isPlayer)
            ActivateGameOver_Panel();
        Destroy(this.gameObject, 2f);
        this.enabled = false;
        Debug.LogWarning("Disabled behaviour");
    }

    public void ApplyDamage(float Damage)
    {
        Debug.LogWarning("Apply Damage" + Damage + " ,.....");

        if (isPlayer)
        {
            BloodSplatter_anim.SetBool("isBloodSplatter", true);
            BloodSplatter_anim.SetTrigger("isBloodSplatterr");

            Invoke("BloodSplatterOnHit", 0.2f);
        }
        else
        {
            BloodSplatter_anim.SetBool("isBloodSplatter", true);
            BloodSplatter_anim.SetTrigger("isBloodSplatterr");
            Invoke("BloodSplatterOnHit", 0.2f);
        }

        if (IntrantThirdPersonController.instance._shieldUp)
        {
            return;
        }

        Health = Health - Damage;
        StartCoroutine(BloodAnimation());

        if (health_UI != null)
        {
            _fillAmount = (float)Health / MaxHealth;
            Debug.LogError($"Kaire health {_fillAmount}");
            health_UI.fillAmount = _fillAmount;
        }

        if (Health <= 0)
        {
            _anim.SetTrigger("isDead");
            if (HealthUI_Canvas != null)
                HealthUI_Canvas.SetActive(false);

            if (isPlayer && this.gameObject.CompareTag(Tags.PLAYER_TAG))
            {
                Debug.LogWarning("isPlayer....");

                GetComponent<IntrantThirdPersonController>().enabled = false;


                GetComponent<PlayerAttackScript>().enabled = false;

                GameObject.FindGameObjectWithTag(Tags.ENEMY_TAG).GetComponent<EnemyController>().enabled = false;
                //Dead_BloodSplatter.SetActive(true);
                soundFX.Die();
            }

            else
            {
                Debug.LogWarning("isEnemy....");

                playerDied = true;
                soundFX.Die();
                afterEnemy_Death();
                GetComponent<NavMeshAgent>().enabled = false;
                GetComponent<EnemyController>().enabled = false;

            }
        }
    }

    void ActivateGameOver_Panel()
    {
        Debug.LogWarning("inside ActivateGameOver_Panel");
        GameOver_Panel.SetActive(true);
    }


    public void afterEnemy_Death()
    {
        if (playerDied == true)
        {
            GameManager.enemyDead_Count = GameManager.enemyDead_Count + 1;
            Debug.LogWarning("The Character Died......." + GameManager.enemyDead_Count);

            playerDied = true;
        }

        /*if (GameManager.enemyDead_Count >= 4)
        {
            Debug.LogWarning("Activate Monster");
            AttackPoint.GetComponent<AttackDamage>().enabled = false;

            GameManager.instance.activate_BossMonster();

            _Player.GetComponent<DragonHealth_Script>().enabled = true;
        }*/
    }

    public void ReduceHealth(float percentage)
    {
        int healthReduction = Mathf.FloorToInt(Health * percentage);
        Health -= healthReduction;
        if (Health < 0)
        {
            Health = 0;
        }
    }

    public void Increasehealth(int val)
    {
        Health += val;
        health_UI.fillAmount = Health / 100f;
    }

    IEnumerator BloodAnimation()
    {
        GameObject bloodvfxinstance = Instantiate(BloodVFX,transform.position,quaternion.identity);
        yield return new WaitForSeconds(0.3f);
        Destroy(bloodvfxinstance);
    }
}
