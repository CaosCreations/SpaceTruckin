using PixelCrushers.DialogueSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class FacialExpressionPreviewWindow : EditorWindow
{
    private const int PortraitCount = 9;
    private const int PortraitSize = 128;
    private const int PortraitsPerRow = 3;

    private DialogueDatabase database;
    private Actor selectedActor;
    private int selectedActorIndex;
    private string[] actorNames = new string[0];
    private List<Actor> actorsWithPortraits = new List<Actor>();
    private Vector2 scrollPosition;
    private bool editMode = false;
    private bool showAllActors = false;

    [MenuItem("Space Truckin/Dialogue/Facial Expression Preview")]
    public static void ShowWindow()
    {
        var window = GetWindow<FacialExpressionPreviewWindow>("Facial Expressions");
        window.minSize = new Vector2(450, 500);
    }

    private void OnEnable()
    {
        RefreshDatabase();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space(10);

        DrawDatabaseSelector();

        if (database == null)
        {
            EditorGUILayout.HelpBox("Select a Dialogue Database to preview actor facial expressions.", MessageType.Info);
            return;
        }

        EditorGUILayout.Space(10);
        DrawActorSelector();

        if (selectedActor == null)
        {
            EditorGUILayout.HelpBox("Select an actor to preview their facial expressions.", MessageType.Info);
            return;
        }

        EditorGUILayout.Space(10);
        DrawPortraitGrid();
    }

    private void DrawDatabaseSelector()
    {
        EditorGUI.BeginChangeCheck();
        database = EditorGUILayout.ObjectField("Dialogue Database", database, typeof(DialogueDatabase), false) as DialogueDatabase;
        if (EditorGUI.EndChangeCheck())
        {
            RefreshActorList();
        }
    }

    private void DrawActorSelector()
    {
        if (actorNames.Length == 0)
        {
            EditorGUILayout.HelpBox(
                showAllActors ? "No actors found in database." : "No actors with portraits found. Enable 'Show All' to see all actors.",
                MessageType.Warning);
            return;
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginChangeCheck();
        selectedActorIndex = EditorGUILayout.Popup("Actor", selectedActorIndex, actorNames);
        if (EditorGUI.EndChangeCheck() || selectedActor == null)
        {
            if (selectedActorIndex >= 0 && selectedActorIndex < actorsWithPortraits.Count)
            {
                selectedActor = actorsWithPortraits[selectedActorIndex];
            }
        }

        EditorGUI.BeginChangeCheck();
        showAllActors = GUILayout.Toggle(showAllActors, "Show All", EditorStyles.miniButton, GUILayout.Width(70));
        if (EditorGUI.EndChangeCheck())
        {
            RefreshActorList();
        }

        editMode = GUILayout.Toggle(editMode, "Edit Mode", EditorStyles.miniButton, GUILayout.Width(80));
        EditorGUILayout.EndHorizontal();
    }

    private void DrawPortraitGrid()
    {
        EditorGUILayout.LabelField("Facial Expression Portraits", EditorStyles.boldLabel);

        if (editMode)
        {
            EditorGUILayout.HelpBox(
                "EDIT MODE: Drag sprites into the slots to assign portraits.\n" +
                "Changes are saved to the database automatically.",
                MessageType.Warning);
        }
        else
        {
            EditorGUILayout.HelpBox(
                "Index 0 = Default portrait\n" +
                "Index 1-8 = Alternate portraits\n" +
                "Set the 'FacialExpression' field on dialogue nodes to these indices.\n" +
                "Click 'Edit Mode' to assign portraits.",
                MessageType.Info);
        }

        EditorGUILayout.Space(5);

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        var portraits = GetActorPortraits(selectedActor);

        for (int row = 0; row < (PortraitCount + PortraitsPerRow - 1) / PortraitsPerRow; row++)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            for (int col = 0; col < PortraitsPerRow; col++)
            {
                int index = row * PortraitsPerRow + col;
                if (index >= PortraitCount) break;

                DrawPortraitCell(index, portraits[index]);
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(5);
        }

        EditorGUILayout.EndScrollView();
    }

    private void DrawPortraitCell(int index, Texture2D portrait)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(PortraitSize + 10));

        // Index label
        var labelStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            alignment = TextAnchor.MiddleCenter
        };
        EditorGUILayout.LabelField($"Index: {index}", labelStyle);

        if (editMode)
        {
            // Editable sprite field
            var currentSprite = GetActorSpriteAtIndex(selectedActor, index);
            EditorGUI.BeginChangeCheck();
            var newSprite = (Sprite)EditorGUILayout.ObjectField(currentSprite, typeof(Sprite), false,
                GUILayout.Width(PortraitSize), GUILayout.Height(PortraitSize));
            if (EditorGUI.EndChangeCheck())
            {
                SetActorSpriteAtIndex(selectedActor, index, newSprite);
            }
        }
        else
        {
            // Portrait preview (read-only)
            var rect = GUILayoutUtility.GetRect(PortraitSize, PortraitSize);
            if (portrait != null)
            {
                GUI.DrawTexture(rect, portrait, ScaleMode.ScaleToFit);
            }
            else
            {
                EditorGUI.DrawRect(rect, new Color(0.2f, 0.2f, 0.2f));
                var noPortraitStyle = new GUIStyle(EditorStyles.miniLabel)
                {
                    alignment = TextAnchor.MiddleCenter,
                    normal = { textColor = Color.gray }
                };
                GUI.Label(rect, "No Portrait", noPortraitStyle);
            }
        }

        // Description label
        string description = index == 0 ? "(Default)" : $"(Alt {index})";
        var descStyle = new GUIStyle(EditorStyles.miniLabel)
        {
            alignment = TextAnchor.MiddleCenter
        };
        EditorGUILayout.LabelField(description, descStyle);

        EditorGUILayout.EndVertical();
    }

    private Sprite GetActorSpriteAtIndex(Actor actor, int index)
    {
        if (actor == null) return null;

        if (index == 0)
        {
            return actor.spritePortrait;
        }

        int altIndex = index - 1;
        if (actor.spritePortraits != null && altIndex < actor.spritePortraits.Count)
        {
            return actor.spritePortraits[altIndex];
        }

        return null;
    }

    private void SetActorSpriteAtIndex(Actor actor, int index, Sprite sprite)
    {
        if (actor == null || database == null) return;

        Undo.RecordObject(database, "Set Actor Portrait");

        if (index == 0)
        {
            actor.spritePortrait = sprite;
        }
        else
        {
            int altIndex = index - 1;

            // Ensure spritePortraits list exists and is large enough
            if (actor.spritePortraits == null)
            {
                actor.spritePortraits = new List<Sprite>();
            }

            while (actor.spritePortraits.Count <= altIndex)
            {
                actor.spritePortraits.Add(null);
            }

            actor.spritePortraits[altIndex] = sprite;
        }

        EditorUtility.SetDirty(database);
    }

    private Texture2D[] GetActorPortraits(Actor actor)
    {
        var portraits = new Texture2D[PortraitCount];

        if (actor == null) return portraits;

        // Index 0: Default portrait (check both Texture2D and Sprite variants)
        if (actor.portrait != null)
        {
            portraits[0] = actor.portrait;
        }
        else if (actor.spritePortrait != null)
        {
            portraits[0] = GetTextureFromSprite(actor.spritePortrait);
        }

        // Index 1-8: Alternate portraits
        for (int i = 0; i < 8; i++)
        {
            if (actor.alternatePortraits != null && i < actor.alternatePortraits.Count)
            {
                portraits[i + 1] = actor.alternatePortraits[i];
            }
            else if (actor.spritePortraits != null && i < actor.spritePortraits.Count)
            {
                portraits[i + 1] = GetTextureFromSprite(actor.spritePortraits[i]);
            }
        }

        return portraits;
    }

    private Texture2D GetTextureFromSprite(Sprite sprite)
    {
        if (sprite == null) return null;
        return sprite.texture;
    }

    private void RefreshDatabase()
    {
        if (database == null)
        {
            // Try to find a dialogue database in the project
            var guids = AssetDatabase.FindAssets("t:DialogueDatabase");
            if (guids.Length > 0)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                database = AssetDatabase.LoadAssetAtPath<DialogueDatabase>(path);
            }
        }

        RefreshActorList();
    }

    private void RefreshActorList()
    {
        actorsWithPortraits.Clear();
        selectedActor = null;
        selectedActorIndex = 0;

        if (database == null)
        {
            actorNames = new string[0];
            return;
        }

        // Include actors based on filter setting
        if (showAllActors)
        {
            actorsWithPortraits = database.actors
                .OrderBy(a => a.Name)
                .ToList();
        }
        else
        {
            actorsWithPortraits = database.actors
                .Where(a => a.portrait != null ||
                           a.spritePortrait != null ||
                           (a.alternatePortraits != null && a.alternatePortraits.Count > 0) ||
                           (a.spritePortraits != null && a.spritePortraits.Count > 0))
                .OrderBy(a => a.Name)
                .ToList();
        }

        actorNames = actorsWithPortraits.Select(a => a.Name).ToArray();

        if (actorsWithPortraits.Count > 0)
        {
            selectedActor = actorsWithPortraits[0];
        }
    }
}
