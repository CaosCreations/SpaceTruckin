using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public static class StringExtensions
{
    public static string InsertNewLines(this string self)
    {
        if (!string.IsNullOrWhiteSpace(self))
        {
            return self.Replace("<br>", Environment.NewLine);
        }
        return string.Empty;
    }

    public static string RemoveCarriageReturns(this string self)
    {
        return self.Replace("\r", string.Empty);
    }

    public static string ToItalics(this string self)
    {
        if (!string.IsNullOrWhiteSpace(self))
        {
            return self.Insert(self.Length - 1, "</i>").Insert(0, "<i>");
        }
        return string.Empty;
    }

    public static string RemoveAllWhitespace(this string self)
    {
        if (!string.IsNullOrEmpty(self))
        {
            return self.Replace(" ", string.Empty);
        }
        return string.Empty;
    }

    public static string RemoveCharacter(this string self, char character)
    {
        if (self != null && !char.IsWhiteSpace(character))
        {
            return self.Replace(character.ToString(), string.Empty);
        }
        return string.Empty;
    }

    public static bool IsNullOrEmpty(this (string, string) self)
    {
        return self == (null, null)
            || self == (string.Empty, null)
            || self == (null, string.Empty)
            || self == (string.Empty, string.Empty);
    }

    public static bool IsAlphabetical(this string self, bool includeAccents = false)
    {
        string regexPattern = includeAccents ?
            UIConstants.AlphabeticalIncludingAccentsPattern : UIConstants.AlphabeticalPattern;

        return !string.IsNullOrWhiteSpace(self) && Regex.IsMatch(self, regexPattern);
    }

    public static string ReplaceGameStateTemplates(this string self, IDataModel dataModel = null)
    {
        MatchCollection matches = new Regex(UIConstants.GameStateTemplatePattern).Matches(self);

        if (matches.Count <= 0)
        {
            return self;
        }

        foreach (Match match in matches.Cast<Match>().Reverse())
        {
            string replacement = UIUtils.GetGameStateTemplateReplacement(match.Value
                .RemoveTemplateBoundaries()
                .RemoveAllWhitespace()
                .ToUpper(),
                dataModel);

            self = self.Remove(match.Index, match.Length).Insert(match.Index, replacement);
        }

        return self;
    }

    public static string ReplaceLuaVariableTemplates(this string self)
    {
        MatchCollection matches = new Regex(UIConstants.LuaVariablePattern).Matches(self);

        if (matches.Count <= 0)
        {
            return self;
        }

        foreach (Match match in matches.Cast<Match>().Reverse())
        {
            if (match.Groups.Count < 2)
            {
                Debug.LogError("Insufficient Lua variable regex match groups.");
                continue;
            }

            // The 3rd match group contains the Lua variable name 
            string replacement = DialogueDatabaseManager.GetLuaVariableAsString(match.Groups[2].Value);

            self = self.Remove(match.Index, match.Length).Insert(match.Index, replacement);
        }

        return self;
    }

    public static string ReplaceLuaActorFieldTemplates(this string self)
    {
        MatchCollection matches = new Regex(UIConstants.LuaActorFieldPattern).Matches(self);

        if (matches.Count <= 0)
        {
            return self;
        }

        foreach (Match match in matches.Cast<Match>().Reverse())
        {
            if (match.Groups.Count < 3)
            {
                Debug.LogError("Insufficient Lua variable regex match groups.");
                continue;
            }

            // The 2nd group contains the actor name
            // The 3rd group contains the field name 
            string replacement = DialogueDatabaseManager
                .GetActorFieldAsString(actorName: match.Groups[2].Value, fieldName: match.Groups[3].Value);

            self = self.Remove(match.Index, match.Length).Insert(match.Index, replacement);
        }

        return self;
    }

    private static string RemoveTemplateBoundaries(this string self)
    {
        return self
            .TrimStart(UIConstants.TemplateBoundaryLeftChar)
            .TrimEnd(UIConstants.TemplateBoundaryRightChar);
    }

    public static string ReplaceAllTemplates(this string self)
    {
        return self
            .ReplaceGameStateTemplates()
            .ReplaceLuaVariableTemplates()
            .ReplaceLuaActorFieldTemplates();
    }

    public static string RemoveConsecutiveSpaces(this string self)
    {
        if (!string.IsNullOrEmpty(self))
        {
            return new Regex(UIConstants.ConsecutiveSpacesPattern).Replace(self, " ");
        }
        return self;
    }

    public static string EnforceCharacterLimit(this string self, int limit)
    {
        if (self != null && self.Length > limit)
        {
            return self.Substring(0, limit);
        }
        return self;
    }
}
