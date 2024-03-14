using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortressOfEventualitiesTriggerManagerEarthandWater : MonoBehaviour
{

    public bool _kairfightPoint = false;

    public EnemySpawner _enemySpawner;
    public EnemyController _kaireEnemyController;

    public GameObject []_spts;

    public GameObject _cave;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.PLAYER_TAG) && _enemySpawner != null && _cave == null)
        {
            if (other.GetComponent<IntrantThirdPersonController>()._collectedOrbs > 2)
            {
                _enemySpawner.spawnRate = 1;
            }
            _enemySpawner.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag(Tags.PLAYER_TAG) && !_kairfightPoint && _cave == null)
        {
            gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag(Tags.Kaire_TAG) && _kairfightPoint)
        {
            Debug.LogError("Kair Reached fight point");
            GetComponent<EnemySpawner>().enabled = true;
            other.gameObject.GetComponent<EnemyController>().Stop();
            other.gameObject.GetComponent<EnemyController>().ResetTargetToPlayer();
            other.gameObject.GetComponent<EnemyController>()._fightPlayer = true;
            _kaireEnemyController = other.gameObject.GetComponent<EnemyController>();
            //gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag(Tags.PLAYER_TAG) && _kairfightPoint && _kaireEnemyController != null)
        {
            _kaireEnemyController.Resume();
        }
        if(other.gameObject.TryGetComponent<IntrantThirdPersonController>(out IntrantThirdPersonController _player) && _cave != null)
        {
            if (_player._collectedOrbs>2)
            {
                Debug.LogError("Destroying Enemies");
                _cave.SetActive(false);
                _enemySpawner.DestroyEnemies();
            }
        }
    }

    public void FightFinished()
    {
        _kaireEnemyController.FollowGate();
        _kaireEnemyController.Resume();
    }
}
