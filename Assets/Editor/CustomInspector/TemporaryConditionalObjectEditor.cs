using UnityEditor;

[CustomEditor(typeof(TemporaryConditionalObject))]
public class TemporaryConditionalObjectEditor : Editor
{
    private SerializedProperty conditionProperty;
    private SerializedProperty activeDatesProperty;
    private SerializedProperty messageProperty;
    private SerializedProperty missionProperty;

    private void OnEnable()
    {
        conditionProperty = serializedObject.FindProperty("condition");
        activeDatesProperty = serializedObject.FindProperty("activeDates");
        messageProperty = serializedObject.FindProperty("message");
        missionProperty = serializedObject.FindProperty("mission");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(conditionProperty);

        TemporaryConditionalObject.Condition condition = (TemporaryConditionalObject.Condition)conditionProperty.enumValueIndex;

        switch (condition)
        {
            case TemporaryConditionalObject.Condition.Date:
                EditorGUILayout.PropertyField(activeDatesProperty, true);
                break;
            case TemporaryConditionalObject.Condition.Message:
                EditorGUILayout.PropertyField(messageProperty, true);
                break;
            case TemporaryConditionalObject.Condition.Mission:
                EditorGUILayout.PropertyField(missionProperty, true);
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
