using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AccessBlockedUI : MonoBehaviour
{
    [SerializeField]
    private List<CanvasAccessSettings> accessSettings = new();

    [SerializeField]
    private Button okButton;

    [SerializeField]
    private Canvas canvas;

    public bool IsCanvasAccessible(UICanvasType type)
    {
        var setting = accessSettings.FirstOrDefault(s => s.CanvasType == type);
        return setting == null || DialogueDatabaseManager.GetLuaVariableAsBool(setting.DialogueVariableName);
    }

    public void ShowPopup()
    {
        canvas.gameObject.SetActive(true);
    }

    private void OnOk()
    {
        canvas.gameObject.SetActive(false);
        PlayerManager.ExitPausedState();
    }

    private void Awake()
    {
        okButton.AddOnClick(OnOk);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OnOk();
    }
}
