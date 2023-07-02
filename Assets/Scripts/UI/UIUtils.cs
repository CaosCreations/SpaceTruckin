using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;

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

        return '\0';
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

    /// <summary>
    /// Gets all event system raycast results for the current mouse position.
    /// </summary>
    public static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        return raycastResults;
    }

    public static bool IsPointerOverLayer(int layerNumber)
    {
        List<RaycastResult> raycastResults = GetEventSystemRaycastResults();

        for (int i = 0; i < raycastResults.Count; i++)
        {
            if (raycastResults[i].gameObject.layer == layerNumber)
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsPointerOverTag(string tagName)
    {
        List<RaycastResult> raycastResults = GetEventSystemRaycastResults();

        for (int i = 0; i < raycastResults.Count; i++)
        {
            if (raycastResults[i].gameObject.CompareTag(tagName))
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsPointerOverTag(string[] tagNames)
    {
        List<RaycastResult> raycastResults = GetEventSystemRaycastResults();

        for (int i = 0; i < raycastResults.Count; i++)
        {
            if (tagNames.Any(x => raycastResults[i].gameObject.CompareTag(x)))
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsPointerOverObjectType(Type objectType)
    {
        List<RaycastResult> raycastResults = GetEventSystemRaycastResults();

        for (int i = 0; i < raycastResults.Count; i++)
        {
            if (raycastResults[i].gameObject.GetType() == objectType
                || raycastResults[i].gameObject.GetComponent(objectType))
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsPointerOverObjectType(Type[] objectTypes)
    {
        List<RaycastResult> raycastResults = GetEventSystemRaycastResults();

        for (int i = 0; i < raycastResults.Count; i++)
        {
            var type = raycastResults[i].gameObject.GetType();

            if (objectTypes.Contains(type) 
                || objectTypes.Any(t => raycastResults[i].gameObject.TryGetComponent(t, out _)))
            {
                return true;
            }
        }

        return false;
    }

    public static bool AreAnyCanvasesActive()
    {
        return UnityEngine.Object.FindObjectsOfType<UICanvasBase>().Any();
    }

    public static Color HexStringToColor(string hexString)
    {
        if (string.IsNullOrWhiteSpace(hexString) || hexString.Length < 6)
            return Color.white;

        var r = byte.Parse(hexString.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
        var g = byte.Parse(hexString.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
        var b = byte.Parse(hexString.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);

        var color = new Color(r, g, b, 255);
        return color;
    }

    public static bool IsFuelButtonInteractable(Ship ship, long fuelCostPerUnit)
    {
        return ship.CurrentFuel < ship.MaxFuel
            && PlayerManager.Instance.CanSpendMoney(fuelCostPerUnit);
    }
}
