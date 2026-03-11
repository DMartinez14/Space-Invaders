using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "Game";
    [SerializeField] private string creditsSceneName = "Credits";
    [SerializeField] private string menuSceneName = "Menu";
    [SerializeField] private float creditsDuration = 5f;

    void Awake()
    {
        SceneLoader[] loaders = FindObjectsByType<SceneLoader>(FindObjectsSortMode.None);
        if (loaders.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void LoadGame()
    {
        StartCoroutine(_LoadGame());
    }

    public void LoadCredits()
    {
        StartCoroutine(_LoadCredits());
    }

    public void LoadMenu()
    {
        StartCoroutine(_LoadMenu());
    }

    private IEnumerator _LoadGame()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(gameSceneName);
        while (!loadOperation.isDone)
        {
            yield return null;
        }

        GameObject playerObj = GameObject.Find("Player");
        Debug.Log(playerObj != null ? playerObj.name : "Player not found");
    }

    private IEnumerator _LoadCredits()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(creditsSceneName);
        while (!loadOperation.isDone)
        {
            yield return null;
        }

        yield return new WaitForSecondsRealtime(creditsDuration);
        yield return _LoadMenu();
    }

    private IEnumerator _LoadMenu()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(menuSceneName);
        while (!loadOperation.isDone)
        {
            yield return null;
        }
    }
}
