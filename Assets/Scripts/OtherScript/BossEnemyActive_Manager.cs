using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyActive_Manager : MonoBehaviour
{
    private HealthScript _healthScript;

    // Start is called before the first frame update
    void Start()
    {
        _healthScript = GetComponent<HealthScript>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }


}
