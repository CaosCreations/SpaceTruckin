using UnityEngine;

public enum FontType
{
    Title, Subtitle, ListItem, Paragraph, Button 
}

public class FontManager : MonoBehaviour
{
    public static FontManager Instance { get; private set; }

    [SerializeField] private Font titleFont;
    [SerializeField] private Font subtitleFont;
    [SerializeField] private Font listItemFont;
    [SerializeField] private Font paragraphFont;
    [SerializeField] private Font buttonFont;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public Font GetFontByType(FontType fontType)
    {
        switch (fontType)
        {
            case FontType.Title:
                return titleFont;
            case FontType.Subtitle:
                return subtitleFont;
            case FontType.ListItem:
                return listItemFont;
            case FontType.Paragraph:
                return paragraphFont;
            case FontType.Button:
                return buttonFont;
            default:
                return paragraphFont;
        }
    }
}
