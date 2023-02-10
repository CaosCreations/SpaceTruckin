using UnityEngine;
using UnityEngine.UI;

public class UnloadSceneButton : MonoBehaviour
{
    private Button button;

    private void Start()
    {
        if (!TryGetComponent(out button))
        {
            throw new System.Exception("Couldn't get button component in UnloadSceneButton");
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(UnloadScene);
    }

    protected virtual void UnloadScene()
    {
        SceneLoadingManager.Instance.UnloadSceneAsync(gameObject.scene.name);
    }
}