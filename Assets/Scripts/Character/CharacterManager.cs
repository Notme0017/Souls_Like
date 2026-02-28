using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterManager : NetworkBehaviour
{
    [HideInInspector]public CharacterController characterController;
    [HideInInspector]public Animator animator;

    [HideInInspector]public CharacterNetworkManager characterNetworkManager;
    protected virtual void Awake()
    {
        //SINCE PROTECTED SO CAN BE USED IN PLAYER MANAGER WITH ALL FUNCITONALITES 
        //VIRTUAL ALLOWS US TO OVERWRITE WHENEVER NEEDED
        DontDestroyOnLoad(this);

        characterController = GetComponent<CharacterController>();
        characterNetworkManager = GetComponent<CharacterNetworkManager>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        //IF BEING CONTROLLED FROM OUR SIDE UPDATE THE POSITION ON NETWORK
        if (IsOwner)
        {
            characterNetworkManager.networkPosition.Value = transform.position;
            characterNetworkManager.networkRotation.Value = transform.rotation;
        }
        //IF NOT FORM OUR SIDE TAKE THE VALUE FROM NETWORK AND SMOOTH DAMP THE CURRENT POSITITON TO WHAT NETWORK POSITION HAS
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, characterNetworkManager.networkPosition.Value, ref characterNetworkManager.networkPositionVelocity, characterNetworkManager.networkPositionSmooth);
            transform.rotation = Quaternion.Slerp(transform.rotation, characterNetworkManager.networkRotation.Value, characterNetworkManager.networkRotationSmooth);
        }
    }

    protected virtual void LateUpdate()
    {

    }
}
