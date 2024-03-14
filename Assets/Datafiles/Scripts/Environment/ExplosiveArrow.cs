using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ArrowType
{
    BlastArrow,
    HommingArrow
}

public class ExplosiveArrow : MonoBehaviour
{
   public ArrowType arrowType;
    public IntrantThirdPersonController _playerRef;
    public int _minThrustIntensity = 3;
   public int _maxThrustIntensity = 3;
   public int _thrustIntensity = 3;
   public float speed = 10f;
   public float rotationSpeed = 5f;
   public float arrowDamage = 1f;
   public Rigidbody _rb;
   public GameObject _explosive;
   public bool randomArrow = false;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerRef = IntrantThirdPersonController.instance;
        if (arrowType == ArrowType.HommingArrow)
        {
            _rb.isKinematic = true;
        }
    }

    private void Start()
   {
        if (arrowType == ArrowType.BlastArrow)
        {
            _thrustIntensity = Random.Range(_minThrustIntensity, _maxThrustIntensity);
            _rb.velocity = transform.forward * _thrustIntensity;
        }
        
        if(arrowType == ArrowType.HommingArrow)
        {
            Invoke("Shoot",1f);
        }
    }

    private void Shoot()
    {
        _rb.isKinematic = false;
        _thrustIntensity = Random.Range(_minThrustIntensity, _maxThrustIntensity);
        _rb.velocity = transform.up * _thrustIntensity;
        Destroy(gameObject, 2f);
    }

    private void FixedUpdate()
   {
        if (arrowType == ArrowType.BlastArrow)
        {
            if (!randomArrow)
            {
                Quaternion rotation = Quaternion.LookRotation(_rb.velocity.normalized);
                _rb.MoveRotation(rotation);
            }else if(randomArrow)
            {
                Quaternion rotation = Quaternion.LookRotation(_rb.velocity.normalized);
                _rb.MoveRotation(rotation);
            }
        }else if(arrowType == ArrowType.HommingArrow)
        {
            HomingMissile();
        }
    }

    private void HomingMissile()
    {
        if (_playerRef.gameObject == null)
        {
            return;
        }

        // Calculate the direction to the target.
        Vector3 direction = (_playerRef.transform.position - transform.position).normalized;

        // Rotate the arrow to face the target.
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        // Move the arrow forward.
        _rb.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision other)
   {
        if (arrowType == ArrowType.BlastArrow)
        {
            GameObject _gameObject = Instantiate(_explosive, transform.position, quaternion.identity);
            Destroy(_gameObject, 3f);
            Destroy(gameObject, 0.01f);
        }else if (arrowType == ArrowType.HommingArrow)
        {
            if(other.gameObject.TryGetComponent<IntrantPlayerHealthManager>(out IntrantPlayerHealthManager _playerHealth)){
                _playerHealth.DealDamage(arrowDamage);
                Destroy(gameObject);
            }
        }
   }
}
