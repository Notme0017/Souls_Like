using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;
    public PlayerManager player;
    public Camera cameraObject;
    public Transform cameraVerticalPivot;

    [Header("Camera Settings")]
    [SerializeField] float cameraSmoothTime = 1;//THE BIGGER THE NUMBER THE LONGER IT TAKES FOR THE CAMERA TO REACH THE PLAYER
    [SerializeField] float leftAndRightRotationSpeed = 220;
    [SerializeField] float upAndDownRotationSpeed = 220;
    [SerializeField] float minPivot = -30;
    [SerializeField] float maxPivot = 60;
    [SerializeField] float cameraCollisionRadius = 0.2f;
    [SerializeField] LayerMask collidedWithLayers;


    [Header("Camera Values")]//Not player preference
    private Vector3 cameraSmoothVelocity;
    private Vector3 cameraObjectPosition;
    [SerializeField] float leftAndRightLookAngle;
    [SerializeField] float upAndDownLookAngle;
    private float cameraZPosition;
    private float targetCameraZPosition;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        cameraZPosition = cameraObject.transform.localPosition.z;
    }

    public void HandleCameraMovements()
    {
        if(player != null)
        {
            HandleFollowTarget();
            HandleRotation();
            HandleCollisions();
        }
    }

    private void HandleFollowTarget()
    {
        Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraSmoothVelocity, cameraSmoothTime * Time.deltaTime);
        transform.position = targetCameraPosition;
    }

    private void HandleRotation()
    {
        //THERE WILL BE TWO TYPES OF ROTATION
        //1 IS OF FREE TYPE WHERE WE NORMALLY MOVE CAMERA
        //2 IS LOCK ON

        //NORMAL

        //Calculating how much to move the camera based on input
        leftAndRightLookAngle += (PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;

        upAndDownLookAngle -= (PlayerInputManager.instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;
        upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minPivot, maxPivot);

        //moving the camera based on the calcs
        Vector3 cameraRotation = Vector3.zero;
        Quaternion targetRotation;

        //left and right rotation which is based on this gameObject
        cameraRotation.y = leftAndRightLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        transform.rotation = targetRotation;

        //up and down rotation which is based on a child gameObject
        cameraRotation.x = upAndDownLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        cameraVerticalPivot.localRotation = targetRotation;
    }

    private void HandleCollisions()
    {
        targetCameraZPosition = cameraZPosition;

        RaycastHit hit;
        Vector3 direction = cameraObject.transform.position - cameraVerticalPivot.position;
        direction.Normalize();

        if(Physics.SphereCast(cameraVerticalPivot.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collidedWithLayers))
        {
            float distanceFromHitObject = Vector3.Distance(cameraVerticalPivot.position, hit.point);
            targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
        }

        if(Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
        {
            targetCameraZPosition = -cameraCollisionRadius;
        }

        cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
        cameraObject.transform.localPosition = cameraObjectPosition;
    }
}
