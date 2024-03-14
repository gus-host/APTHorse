using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TheraHealthManager : MonoBehaviour
{
    public static TheraHealthManager Instance;
    public float Health = 100f;
    [SerializeField]
    public Image health_UI;
    public GameObject HealthUI_Canvas;


    public bool playerDied = false;
    public bool playerRevived = false;

    private void Awake()
    {
        Instance = this;
    }
    private void LateUpdate()
    {

        health_UI.fillAmount = Health/100f;
        if (Health < 0)
        {
            playerDied = true;
            EnemyController[] _enemies = FindObjectsOfType<EnemyController>();
            foreach (EnemyController _enemy in _enemies)
            {
                if (_enemy._isKaire)
                {
                    StartCoroutine(_enemy.FollowGateWithDelay());
                }
            }
        }
        if(playerDied)
        {
            if(
                Health > 0)
            {
                playerRevived = true;
            }
        }

        if(playerRevived)
        {
            StartCoroutine(DisableReviveVFX());
        }

    }

    IEnumerator DisableReviveVFX()
    {
        yield return new WaitForSeconds(2f);
        GetComponent<Thera>()._therareviveVFX.gameObject.SetActive(false);
    }

    public void ApplyDamage(float Damage)
    {
        if (IntrantThirdPersonController.instance._shieldUp)
        {
            return;
        }

        Health = Health - Damage;

        if (health_UI != null)
        {
            health_UI.fillAmount = Health / 100f;
        }

        if (Health <= 0)
        {
            if (HealthUI_Canvas != null)
            {
                HealthUI_Canvas.SetActive(false);
                playerDied = true;

                GetComponent<NavMeshAgent>().enabled = false;
                GetComponent<Thera>().enabled = false;
            }
        }
    }
}
