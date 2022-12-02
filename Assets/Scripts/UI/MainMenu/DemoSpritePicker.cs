using UnityEngine;

public class DemoSpritePicker : MonoBehaviour
{
    [SerializeField]
    private GameObject charSprite1;
    [SerializeField]
    private GameObject charSprite2;

    public string SelectedSpriteName;

    private void Start()
    {
        charSprite1.SetActive(true);
        charSprite2.SetActive(false);
    }

    public void PickSprite()
    {
        charSprite1.SetActive(!charSprite1.activeSelf);
        charSprite2.SetActive(!charSprite2.activeSelf);

        if (charSprite1.activeSelf)
            SelectedSpriteName = charSprite1.name;
        else if (charSprite2.activeSelf)
            SelectedSpriteName = charSprite2.name;
        else
            Debug.LogError("No sprites are active.");
    }
}
