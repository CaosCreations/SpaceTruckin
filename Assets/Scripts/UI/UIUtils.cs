public static class UIUtils
{
    public static string GetTemplateReplacement(string template) 
    {
        string replacement = string.Empty;
        switch (template)
        {
            case UIConstants.PlayerNameTemplate:
                replacement = PlayerManager.Instance.PlayerName;
                break;

            // Other templates here 
        }
        return replacement; 
    }

}