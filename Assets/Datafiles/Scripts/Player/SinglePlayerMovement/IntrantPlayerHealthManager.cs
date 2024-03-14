using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntrantPlayerHealthManager : MonoBehaviour
{
    public static IntrantPlayerHealthManager instance;
    [SerializeField] public int maxHealth;
    [SerializeField] public float currentHealth;

    [SerializeField] public Image _healthFill;
    [SerializeField] public TMP_Text _healthTxt;

    [SerializeField] public GameObject GameOver_Panel;
    [SerializeField] public CanvasGroup _damageEffect;
    [SerializeField] public Sprite []_damageEffects;

    [SerializeField] public GameObject _bloodFx;
    
    public event Action PlayerOnDie;
    public static event Action OnHealthUpdated;
    

    private void Awake()
    {
        instance = this;
        
        currentHealth = maxHealth;    
        
        if (GameOver_Panel != null && this.gameObject.CompareTag("Player"))
        {
            GameOver_Panel.SetActive(false);
        }

        OnHealthUpdated += UpdateHealthStatus;
        PlayerOnDie += DisablePlayeBehaviour;
        OnHealthUpdated?.Invoke();
    }

    private void OnDestroy()
    {
        OnHealthUpdated -= UpdateHealthStatus; 
        
        PlayerOnDie -= DisablePlayeBehaviour;
    }

    public void DealDamage(float damageAmount)
    {
        if(currentHealth == 0 ){return;}

        currentHealth = (Mathf.Max(currentHealth - damageAmount, 0));

        StartCoroutine(ShowBloodtoast());
        
        OnHealthUpdated?.Invoke();
        
        if(currentHealth != 0){return;}

        ActivateGameOver_Panel(true);

        Debug.Log("We Died");
    }

    IEnumerator ShowBloodtoast()
    {
        if (_bloodFx.activeSelf) yield return null;
        
        _bloodFx.SetActive(true);
        _damageEffect.alpha = 1;
        _damageEffect.GetComponent<Image>().sprite = _damageEffects[UnityEngine.Random.Range(0, _damageEffects.Length)];
        LeanTween.value(1, 0, 0.7f).setOnUpdate((float val) => _damageEffect.alpha = val);
        yield return new WaitForSeconds(0.5f);
        _bloodFx.SetActive(false);
    }

    private void UpdateHealthStatus()
    {
        float fillAmount = ((float)currentHealth / maxHealth);
        Debug.Log($"Fillamount {fillAmount}");
        _healthFill.fillAmount = fillAmount;
        _healthTxt.text = "HP ";
    }
    
    public void Increasehealth(int val)
    {
        if(val == maxHealth)
            ActivateGameOver_Panel(false);

        if(currentHealth == maxHealth)
        {
            return;
        }
        
        Debug.LogError("Increasing health....");
        currentHealth = currentHealth>maxHealth? maxHealth : Mathf.Max(currentHealth + val, currentHealth);
        OnHealthUpdated?.Invoke();
    }


    private void DisablePlayeBehaviour()
    {
        StartCoroutine(DisableBehaviour());
    }

    IEnumerator DisableBehaviour()
    {
        Debug.LogWarning("Disabling behaviour");
        //Dead_BloodSplatter.SetActive(true);
        GetComponent<IntrantThirdPersonController>().animator.SetTrigger("Death");
        GetComponent<IntrantThirdPersonController>().enabled = false;
        this.gameObject.layer = LayerMask.NameToLayer("Default");
        yield return new WaitForSeconds(1);
        GameManager.instance._gameEnded = true;
        GetComponent<Animator>().enabled = false;
        if (GameOver_Panel != null)
            ActivateGameOver_Panel(true);
        Destroy(this.gameObject, 2f);
        this.enabled = false;
        Debug.LogWarning("Disabled behaviour");
    }
    void ActivateGameOver_Panel(bool val)
    {
        Debug.LogWarning("inside ActivateGameOver_Panel");
        GameOver_Panel.SetActive(val);
    }

    internal void ReduceHealth(int val)
    {
        if (currentHealth > val/2)
        {
            DealDamage(val/2);
        }
        else
        {
            GetComponent<PlayerTimelineManager>().NotEnoughEnergyToSpare();
        }
    }
}
