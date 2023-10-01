using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public abstract class DataModelEditorWindow<T> : EditorWindow where T : IEditableDataModel
{
    protected Vector2 scrollPosition;
    protected string searchBarFilter = string.Empty;
    protected List<T> dataModels;

    private void OnGUI()
    {
        if (dataModels == null)
        {
            return;
        }
        EditorGUILayout.BeginHorizontal("HelpBox");
        EditorGUILayout.LabelField("Filter:", GUILayout.MaxWidth(50));
        searchBarFilter = EditorGUILayout.TextField(searchBarFilter);
        EditorGUILayout.EndHorizontal();
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        if (dataModels.Any())
        {
            foreach (var model in dataModels.Where(x => x.DisplayTitle.Contains(searchBarFilter, StringComparison.InvariantCultureIgnoreCase)))
            {
                EditorGUILayout.BeginHorizontal();

                var newToggleValue = EditorGUILayout.ToggleLeft(model.DisplayTitle, model.ToggleValue);
                if (newToggleValue != model.ToggleValue)
                {
                    OnToggleChanged(model, newToggleValue);
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        else
        {
            GUILayout.Label("No data available.");
        }
        EditorGUILayout.EndScrollView();
    }

    protected virtual void OnToggleChanged(IEditableDataModel model, bool newValue)
    {
        model.ToggleValue = newValue;
        HandleCustomToggleBehavior(model);
    }

    protected virtual void HandleCustomToggleBehavior(IEditableDataModel model)
    {
    }

    protected virtual void RefreshData(List<T> models)
    {
        dataModels = models;
        Repaint();
    }
}
