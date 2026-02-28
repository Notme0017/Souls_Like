using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldDataSaveManager : MonoBehaviour
{
    [SerializeField]
    private int worldSceneIndex = 1;

    public static WorldDataSaveManager instance;

    private void Awake()
    {
        //THERE CAN ONLY BE ONE INSTANCE
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

    public IEnumerator LoadNewGame()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
        yield return null;
    }

    public int GetWorldSceneIndex()
    {
        return worldSceneIndex;
    }
}
