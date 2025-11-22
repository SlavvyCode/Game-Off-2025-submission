#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(Project.Scripts.Objects.SaveableObject))]
public class SaveableObjectEditor : Editor
{
    private void OnEnable()
    {
        // Run the same logic as OnValidate on load
        ValidateID();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ValidateID();
    }

    private void ValidateID()
    {
        var saveable = (Project.Scripts.Objects.SaveableObject)target;
        if (saveable == null) return;

        var go = saveable.gameObject;

        // Don't generate IDs on the prefab asset
        if (PrefabUtility.IsPartOfPrefabAsset(go))
            return;

        // Generate new ID if missing
        if (string.IsNullOrEmpty(saveable.UniqueID))
        {
            Undo.RecordObject(saveable, "Generate SaveableObject ID");
            saveable.ForceGenerateID();
            EditorUtility.SetDirty(saveable);
            EditorSceneManager.MarkSceneDirty(go.scene);
        }
    }
}

#endif