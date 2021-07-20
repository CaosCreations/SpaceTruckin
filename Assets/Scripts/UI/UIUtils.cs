using System.Text.RegularExpressions;
using UnityEngine;

public static class UIUtils
{
    /// <summary>
    /// Get the value to replace a template that represents some information that 
    /// changes depending on the situation.
    /// </summary>
    /// <param name="template">The template to replace.</param>
    /// <param name="dataModel">An optional data model used to get information about a specific object</param>
    public static string GetGameStateTemplateReplacement(string template, IDataModel dataModel = null)
    {
        string replacement = string.Empty;

        if (dataModel is null)
        {
            replacement = GetTemplateReplacement(template);
        }
        else if (dataModel is Ship ship)
        {
            replacement = GetTemplateReplacement(template, ship);
        }
        return replacement;
    }

    private static string GetTemplateReplacement(string template)
    {
        return template switch
        {
            UIConstants.PlayerNameTemplate => PlayerManager.Instance.PlayerName,
            _ => string.Empty,
        };
    }

    private static string GetTemplateReplacement(string template, Ship ship)
    {
        if (ship == null)
        {
            return string.Empty;
        }

        return template switch
        {
            UIConstants.ShipNameTemplate => ship.Name,
            _ => string.Empty,
        };
    }

    public static char ValidateCharInput(char addedChar, string validationPattern)
    {
        if (Regex.IsMatch(addedChar.ToString(), validationPattern))
        {
            return addedChar;
        }
        else
        {
            return '\0';
        }
    }

    public static Texture2D GetUITexture(int width, int height, Color colour)
    {
        Color32[] pixels = new Color32[width * height];

        for (int i = 0; i < pixels.Length; ++i)
        {
            pixels[i] = colour;
        }

        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels32(pixels);
        texture.Apply();

        return texture;
    }
}
