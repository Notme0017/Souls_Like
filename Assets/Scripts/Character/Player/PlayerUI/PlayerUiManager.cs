using UnityEngine;
using Unity.Netcode;

public class PlayerUiManager : MonoBehaviour
{
    public static PlayerUiManager instance;

    [Header("Network Join")]
    [SerializeField]
    private bool startAsClient;

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
    }
    private void Update()
    {
        if(startAsClient)
        {
            startAsClient = false;

            //stopping the host to start as the client 
            NetworkManager.Singleton.Shutdown();

            NetworkManager.Singleton.StartClient();
        }
    }
}
