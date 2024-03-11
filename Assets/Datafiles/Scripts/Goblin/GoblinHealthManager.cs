using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GoblinHealthManager : MonoBehaviour
{
    public static GoblinHealthManager instance;
    public TextMeshProUGUI _healthTxt;
    public float _health = 100f;
    public GameObject HealthUI_Canvas;

    public GameObject BloodSplatter_Hit;

    private Animator _anim;
    private Animator BloodSplatter_anim;

    public float currentHealth;
    public Image health_UI;
    [Header("Bools")]
    [SerializeField] public bool IsAlive = true;

    private void Start()
    {
        _anim = gameObject.GetComponent<Animator>();
    }

    private void Awake()
    {
        instance = this;
        IsAlive = true;
    }

    void Update()
    {
        if (IsAlive)
        {
            if (_health <= 0)
            {
                IsAlive = false;
            }
        }
        if (!IsAlive)
        {
            StartCoroutine(DisableBehaviour());
        }
        ShowHealthLvl();
    }

    private void ShowHealthLvl()
    {
        _healthTxt.text = "HP " + _health.ToString();
    }

    IEnumerator DisableBehaviour()
    {
        GetComponent<NavMeshAgent>().enabled = false;
        yield return new WaitForSeconds(1);
        GetComponent<GoblinManager>().enabled = false;
        yield return new WaitForSeconds(1f);
        GetComponent<Animator>().enabled = false;
        GetComponent<CapsuleCollider>().isTrigger = true;
        Destroy(gameObject,0.2f);
        this.enabled = false;
        
        
    }

    public void ReduceHealth(int val)
    {
        Debug.LogError("Reducing health by " + val);
        _health = _health - val;
        BloodSplatter_anim.SetBool("isBloodSplatter", true);
    }

    public void ApplyDamage(int  damage)
    {
        try
        {

            BloodSplatter_anim.SetBool("isBloodSplatter", true);
        }
        catch (Exception e)
        {

        }
        Invoke("BloodSplatterOnHit", 0.2f);

        _health = _health - damage;
        StartCoroutine(ShowBlood());
        if (health_UI != null)
        {
            health_UI.fillAmount = _health / 100f;
        }

        if (_health <= 0)
        {
            _anim.SetTrigger("Dead");
            HealthUI_Canvas.SetActive(false);
            Debug.LogWarning("isEnemy....");
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<EnemyController>().enabled = false;
        }
    }

    IEnumerator ShowBlood()
    {
        BloodSplatter_Hit.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        BloodSplatter_Hit.SetActive(false);
    }
}
