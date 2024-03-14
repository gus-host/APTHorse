using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum ObjectType
{
    Rocks
}

public class DestructableObjects : MonoBehaviour
{
    public float _health = 100f;
    public float _Maxhealth = 100f;

    public Image fill;
    public GameObject healthCanvas;
    public GameObject bloodSpatter;

    public Fracture _fracture;

    private event Action OnTookDamage;
    public event Action OnMonsterDie;


    private void Start()
    {
        _health = _Maxhealth;
        OnTookDamage += InitiateToastEffect;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null && collision.gameObject.TryGetComponent<Hammer>(out Hammer _hammer))
        {
            DealDamage(_hammer);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.gameObject.TryGetComponent<Hammer>(out Hammer _hammer))
        {
            DealDamage(_hammer);
        }
    }

    private void DealDamage(Hammer _hammer)
    {
        _health -= _hammer.damageToDeal;

        if(_health < 1)
        {
           
        }
        if (_health > 0)
        {
            _health -= _hammer.damageToDeal;
            OnTookDamage?.Invoke();
            fill.fillAmount = (float)_health / _Maxhealth;
            if (_health <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        _fracture.FractureObject();
        Destroy(gameObject);
    }

    private void InitiateToastEffect()
    {
        if (healthCanvas.gameObject.activeSelf)
        {
            return;
        }
        StartCoroutine(ToastEffect());
    }
    IEnumerator ToastEffect()
    {
        healthCanvas.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.75f);
        healthCanvas.gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        OnTookDamage -= InitiateToastEffect;
    }
}
