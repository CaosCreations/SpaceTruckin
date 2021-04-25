using UnityEngine;

public class UICanvasBase : MonoBehaviour
{
    [SerializeField] private GameObject canvasTutorialPrefab;
    
    public void ShowTutorial()
    {
        if (canvasTutorialPrefab != null)
        {
            Instantiate(canvasTutorialPrefab, transform);
        }
    }
}