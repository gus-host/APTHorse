using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveChest : MonoBehaviour
{
    public Rigidbody _rb;
    public GameObject _explosive;

    private void OnCollisionEnter(Collision other)
    {
        GameObject _gameObject = Instantiate(_explosive, transform.position, Quaternion.identity);
        Destroy(_gameObject, 3f);
        Destroy(gameObject, 0.01f);
    }
}
