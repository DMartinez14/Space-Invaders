using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public void OnStartPressed()
    {
        SceneLoader loader = FindFirstObjectByType<SceneLoader>();
        if (loader != null)
            loader.LoadGame();
        else
            Debug.LogError("SceneLoader not found.");
    }
}
