using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    
    void Awake()
    {
        // If an instance already exists and it's not this one, destroy this
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        // Set this as the instance and make it persist
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
