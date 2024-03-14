using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangeHandler : MonoBehaviour
{

  public GameObject _playerRef;
  public static event Action<MonsterMovementController> OnPlayerEntered;

  private void FixedUpdate()
  {
    transform.position = _playerRef.transform.position;
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.TryGetComponent<MonsterMovementController>(out MonsterMovementController _monster))
    {
      _monster.SetState(MonsterState.CHASE);
      OnPlayerEntered?.Invoke(_monster);
    }

    if(other.TryGetComponent<DestructableObjects>(out DestructableObjects _destructable))
    {
            OnPlayerEntered?.Invoke(_monster);
        }
  }
}
