using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CustomisationType", menuName = "ScriptableObjects/CustomisationType", order = 1)]
public class CustomisationType : ScriptableObject
{
    public Image Image;
    public Sprite Sprite;
    public int Index;
    public Color Color;
}
