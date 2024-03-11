using Cinemachine;
using Invector.vCharacterController;
using UnityEngine;

public class CameraFollowingPlayer  : MonoBehaviour
{
    public static CameraFollowingPlayer Instance;
    public GameObject Player;
    public CinemachineVirtualCamera VirtualCamera;

    [Header("Mouse zoom")]
    public bool enableMouseMovement = false;
    public float sensitivity = 2f;
    public float zoomSpeed = 5f;
    public float minDistance = 1f;
    public float maxDistance = 10f;

    private void Awake()
    {
        Instance = this;

    }

    private void LateUpdate()
    {
        if(Player != null)
        {
            VirtualCamera.Follow = Player.transform;
            VirtualCamera.LookAt = Player.transform;
        }
    }
}
