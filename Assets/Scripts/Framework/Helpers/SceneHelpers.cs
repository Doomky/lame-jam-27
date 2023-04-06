using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneHelpers
{
    public static void LoadScene<TSceneName>(TSceneName sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        SceneManager.LoadScene(sceneName.ToString(), loadSceneMode);
    }

    public static AsyncOperation LoadSceneAsync<TSceneName>(TSceneName sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        return SceneManager.LoadSceneAsync(sceneName.ToString(), loadSceneMode);
    }
}
