using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnterEnableandDisable : MonoBehaviour
{
    public GameObject _obj;
    public bool _enabled;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _obj.SetActive(_enabled);
        }
    }
}
