using UnityEngine.SceneManagement;

public static class SceneExtensions
{
    public static SceneType GetSceneType(this Scene scene)
    {
        if (SceneLoadingManager.Instance == null)
            return default;

        return SceneLoadingManager.GetSceneTypeByName(scene.name);
    }
}