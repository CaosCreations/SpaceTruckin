using UnityEngine;

public class UICanvasBase : MonoBehaviour
{
    [SerializeField] private GameObject canvasTutorialPrefab;
    
    public void ShowCanvasTutorial()
    {
        Instantiate(canvasTutorialPrefab, transform);
    }
}