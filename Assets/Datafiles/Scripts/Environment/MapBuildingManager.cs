using UnityEngine;

public enum BuildingType
{
    Doors,
    MetalSpikeDoor
}

public class MapBuildingManager : MonoBehaviour
{
    public float openSpeed = 2.0f;
    public float openAngle = 90.0f;
    public float closeAngle = 0.0f;
    public BuildingType buildingType;
    public BoxCollider doorCollider; // Assign the collider in the Inspector.

    private Transform doorTransform;
    private float currentAngle;
    private bool isDoorMoving = false;

    public bool opened;
    public bool closed;
    public bool IsdoorMoving;

    private void Start()
    {
        doorTransform = transform;
        currentAngle = closeAngle; // Start with the door closed.
        doorCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (isDoorMoving)
        {
            doorCollider.enabled = false; // Disable the collider while the door is moving.
            RotateDoor();
        }
        else
        {
            doorCollider.enabled = true; // Enable the collider when the door is not moving.
        }
    }

    private void RotateDoor()
    {
        float targetAngle = opened ? openAngle : closeAngle; // Determine the target angle based on door state.
        currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, openSpeed * Time.deltaTime);
        doorTransform.localRotation = Quaternion.Euler(0, currentAngle, 0);

        // Check if the door has reached the target angle.
        if (Mathf.Approximately(currentAngle, targetAngle))
        {
            isDoorMoving = false; // Door has finished moving.
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (buildingType == BuildingType.MetalSpikeDoor)
        {
            if (other.gameObject.TryGetComponent<IntrantThirdPersonController>(out IntrantThirdPersonController _player))
            {
                if (_player._meronNftOne)
                {
                    // Handle the spike door movement.
                    Vector3 targetPosition = opened ? transform.position + Vector3.up * 6 : transform.position - Vector3.up * 6;
                    LeanTween.move(gameObject, targetPosition, 2f);
                }
            }
        }
        else if (buildingType == BuildingType.Doors)
        {
            if (!isDoorMoving)
            {
                if (!opened)
                {
                    isDoorMoving = true;
                    opened = true;
                }
                else
                {
                    isDoorMoving = true;
                    opened = false;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (buildingType == BuildingType.MetalSpikeDoor)
        {
            if (other.gameObject.TryGetComponent<IntrantThirdPersonController>(out IntrantThirdPersonController _player))
            {
                if (_player._meronNftOne)
                {
                    // Handle the spike door movement.
                    Vector3 targetPosition = transform.position + Vector3.up * 6;
                    LeanTween.move(gameObject, targetPosition, 2f);
                }
            }
        }
    }
}
