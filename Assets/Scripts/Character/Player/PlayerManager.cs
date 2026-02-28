using UnityEngine;

public class PlayerManager: CharacterManager
{
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    protected override void Awake()
    {
        base.Awake();

        //NOW WE CAN ADD MORE FUNCITONALITES WITHOUT CHANGING THE BASE CLASS
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
    }

    protected override void Update()
    {
        if (!IsOwner) return;

        base.Update();

        //HANDLES ALL THE MOVEMENTS
        playerLocomotionManager.HandleAllMovements();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            PlayerCamera.instance.player = this;
            PlayerInputManager.instance.player = this;
            //IF THIS IS THE OWNER OR SAY THE PERSON WHOSE WORLD THIS IS, THEY GET THE CAMERA ACCESS
        }
    }

    protected override void LateUpdate()
    {
        if (!IsOwner) return;

        base.LateUpdate();

        PlayerCamera.instance.HandleCameraMovements();
    }
}
