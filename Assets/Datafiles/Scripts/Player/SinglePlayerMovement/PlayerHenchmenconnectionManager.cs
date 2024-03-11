using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHenchmenconnectionManager : MonoBehaviour
{
    public GameObject milut;
    public GameObject milutRef;
    public GameObject []milutSpts;

    public void BringMilut()
    {
        milutRef = Instantiate(milut, milutSpts[Random.Range(0, milutSpts.Length)].transform.position, Quaternion.identity);
        GetComponent<PlayerTimelineManager>()._milutArrived = true;
    }
}
