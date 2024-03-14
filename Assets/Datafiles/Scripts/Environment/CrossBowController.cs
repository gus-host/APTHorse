using UnityEngine;

public class CrossBowController : MonoBehaviour
{
    public GameObject _spawnPoint;
    public GameObject _arrow;

    public bool _initOnStart = false;
    public bool _firing = false;
    public bool _randomForce = false;
    private void OnEnable()
    {
        if (_initOnStart)
        {
            ActivateBows();
        }
    }
    
    public void ActivateBows()
    {
         InvokeRepeating("FireArrow", 1f, 7f);
    }

    private void FireArrow()
    {
        if(!_randomForce)
        {
            GameObject arrow = Instantiate(_arrow, _spawnPoint.transform.position, Quaternion.Euler(-25,90,0), _spawnPoint.transform);
            ExplosiveArrow _explosiveArrow = arrow.GetComponent<ExplosiveArrow>();
            _explosiveArrow.randomArrow = _randomForce;
            arrow.transform.LookAt(Vector3.forward);
        }
        else if(_randomForce) 
        {
            GameObject arrow = Instantiate(_arrow, _spawnPoint.transform.position, Quaternion.Euler(-25, 90, 0), _spawnPoint.transform);
            int randInt = Random.Range(0,4);
            if (randInt == 0)
            {
                arrow.transform.LookAt(Vector3.down);
            }
            else if (randInt == 1)
            {
                arrow.transform.LookAt(Vector3.up);
            }
            else if (randInt == 2)
            {
                arrow.transform.LookAt(Vector3.left);
            }
            else if (randInt == 3)
            {
                arrow.transform.LookAt(Vector3.right);
            }
        }
    }
    private void OnDisable()
    {
        CancelInvoke();
    }
}
