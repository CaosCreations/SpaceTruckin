using UnityEngine;
using UnityEngine.UI;

public class UnloadSceneButton : MonoBehaviour
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.AddOnClick(UnloadScene);
    }

    private void UnloadScene()
    {
        SceneLoadingManager.Instance.UnloadSceneAsync(gameObject.scene.name);
    }
}