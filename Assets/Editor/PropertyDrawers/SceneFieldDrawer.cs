
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SceneFieldAttribute))]
public class SceneFieldDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        if (property.propertyType == SerializedPropertyType.String)
        {
            var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(property.stringValue);

            EditorGUI.BeginChangeCheck();
            var newSceneAsset = EditorGUI.ObjectField(position, label, sceneAsset, typeof(SceneAsset), false) as SceneAsset;
            if (EditorGUI.EndChangeCheck())
            {
                var newPath = AssetDatabase.GetAssetPath(newSceneAsset);
                property.stringValue = newPath;
            }
        }
        else
        {
            EditorGUI.LabelField(position, label.text, "Use SceneField with string.");
        }

        EditorGUI.EndProperty();
    }
}

public class SceneFieldAttribute : PropertyAttribute { }
