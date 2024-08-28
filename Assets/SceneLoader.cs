using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class SceneLoader : TruongSingleton<SceneLoader>
{
    public void LoadScene(string sceneAddress)
    {
        // Tải scene  
        Addressables.LoadSceneAsync(sceneAddress).Completed += OnSceneLoaded;
    }

    private void OnSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Scene loaded successfully!");
        }
        else
        {
            Debug.Log("Failed to load scene.");
        }
    }
}