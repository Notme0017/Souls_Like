using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{

    [HideInInspector]public static PlayerInputManager instance;
    [HideInInspector]public PlayerManager player;

    [Header("Movement Input")]
    [SerializeField] private Vector2 movement;
    public float horizontalInput;
    public float verticalInput;
    public float moveAmount;

    [Header("Camera Movement Input")]
    [SerializeField] private Vector2 cameraInput;
    public float cameraVerticalInput;
    public float cameraHorizontalInput;

    PlayerControls playerControls;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        player = GetComponent<PlayerManager>();
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        instance.enabled = false;

        SceneManager.activeSceneChanged += OnSceneChange;
    }

    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movement = i.ReadValue<Vector2>();
            playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
        }

        playerControls.Enable();
    }

    private void OnDestroy()
    {
        //TO AVOID MEMORY LEAKS
        SceneManager.activeSceneChanged -= OnSceneChange;
    }
    
    private void Update()
    {
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
    }

    private void OnApplicationFocus(bool focus)
    {
        //IF THE GAME IS NOT FOCUSED THEN WE DO NOT READ THE INPUTS
        if (enabled)
        {
            if (focus)
            {
                playerControls.Enable();
            }
            else
            {
                playerControls.Disable();
            }
        }
    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        //ALLOWING TO CONTROL ONLY IN WORLD SCENE AND NOT IN TITLE MENU
        if(newScene.buildIndex == WorldDataSaveManager.instance.GetWorldSceneIndex())
        {
            instance.enabled = true;
        }
        //NOT ALLOWING TO CONTROL WHEN NOT IN WORLD
        else
        {
            instance.enabled = false;
        }
    }

    private void HandlePlayerMovementInput()
    {
        horizontalInput = movement.x;
        verticalInput = movement.y;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));

        // CLAMPING THE VALUE TO A FIXED VALUE
        if(moveAmount <0.5f && moveAmount >= 0.1f)
        {
            moveAmount = 0.5f;
        }
        else if(moveAmount >= 0.5f && moveAmount <= 1.0f)
        {
            moveAmount = 1.0f;
        }

        //THIS IS FOR NOT LOCKED IN TIME SO WE JUST RUN AND WALK
        if(player)
        player.playerAnimatorManager.UpdateAnimatorMovementParameter(0, moveAmount);

        //THIS IS FOR LOCKED ON TIME
    }

    private void HandleCameraMovementInput()
    {
        cameraHorizontalInput = cameraInput.x;
        cameraVerticalInput = cameraInput.y;

    }
}
