using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHealthController : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Image fill;
    public GameObject healthCanvas;
    public GameObject bloodSpatter;
    public GameObject _damagePoint;

    public static event Action<MonsterMovementController> OnPlayerExit;
    private event Action OnTookDamage; 
    public event Action OnMonsterDie; 
    private void Start()
    {
        currentHealth = maxHealth;
        fill.fillAmount = currentHealth / maxHealth;
        OnTookDamage += InitiateToastEffect;
        healthCanvas.gameObject.SetActive(false);
    }

    public void GiveDamage(int damageAmount)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damageAmount;
            Vector3 _pos = new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z);

            if (_damagePoint != null)
            {
                GameObject _damagePointInstance = Instantiate(_damagePoint, _pos, Quaternion.identity, transform);
                _damagePointInstance.GetComponent<TextMeshPro>().text = "-" + damageAmount.ToString();
                Destroy(_damagePointInstance, 1f);
            }
            OnTookDamage?.Invoke();
            fill.fillAmount = (float)currentHealth / maxHealth;
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        // Implement death logic here (e.g., play death animation, destroy the object, etc.)
        
        OnMonsterDie?.Invoke();
        OnPlayerExit?.Invoke(GetComponent<MonsterMovementController>());
    }

    private void InitiateToastEffect()
    {
        if(healthCanvas.gameObject.activeSelf){
            return;
        }
        StartCoroutine(ToastEffect());
    }
    
    IEnumerator ToastEffect()
    {
        if(bloodSpatter!= null)
        {
            bloodSpatter.SetActive(true);
        }
        healthCanvas.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.75f);
        if (bloodSpatter != null)
        {
            bloodSpatter.SetActive(false);
        }
        healthCanvas.gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        OnTookDamage -= InitiateToastEffect;
    }
}
