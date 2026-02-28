using UnityEngine;

public class PlayerLocomotionManager: CharacterLocomotionManager
{
    PlayerManager player;

    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private Vector3 targetRotationDirection;
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float walkingSpeed = 2f;
    [SerializeField] private float runningSpeed = 5f;

    public float verticalMovement;
    public float horizontalMovement;
    public float moveAmount;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    protected override void Update()
    {
        base.Update();

        if (player.IsOwner)
        {
            player.characterNetworkManager.verticalMovement.Value = verticalMovement;
            player.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
            player.characterNetworkManager.moveAmount.Value = moveAmount;
        }
        else
        {
            verticalMovement = player.characterNetworkManager.verticalMovement.Value;
            horizontalMovement = player.characterNetworkManager.horizontalMovement.Value;
            moveAmount = player.characterNetworkManager.moveAmount.Value;

            player.playerAnimatorManager.UpdateAnimatorMovementParameter(0, moveAmount);
        }
    }

    public void HandleAllMovements()
    {
        HandleGroundedMovements();
        HandleRotation();
        //ARIEL MOVEMENTS
    }

    private void GetMovement()
    {
        verticalMovement = PlayerInputManager.instance.verticalInput;
        horizontalMovement = PlayerInputManager.instance.horizontalInput;
        moveAmount = PlayerInputManager.instance.moveAmount;
    }

    private void HandleGroundedMovements()
    {
        GetMovement();
        //MOVEDIRECTION IS BASED ON CAMERA FACING DIRECTION
        moveDirection = (PlayerCamera.instance.cameraObject.transform.forward * verticalMovement) + (PlayerCamera.instance.cameraObject.transform.right * horizontalMovement);
        moveDirection.Normalize();
        moveDirection.y = 0;

        if(PlayerInputManager.instance.moveAmount > 0.5f)
        {
            player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
        }
        else if(PlayerInputManager.instance.moveAmount <= 0.5f)
        {
            player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
        }
    }

    private void HandleRotation()
    {
        targetRotationDirection = Vector3.zero;
        targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement + (PlayerCamera.instance.cameraObject.transform.right * horizontalMovement);
        targetRotationDirection.y = 0;
        targetRotationDirection.Normalize();

        if(targetRotationDirection == Vector3.zero)
        {
            targetRotationDirection = transform.forward;
        }

        Quaternion turnRotation = Quaternion.LookRotation(targetRotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, turnRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = targetRotation;
    }
}
