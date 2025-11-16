using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
namespace Project.Scripts.Objects
{

    public class SaveableObject : MonoBehaviour
    {
        [SerializeField] private string uniqueID;
        public string UniqueID => uniqueID;

#if UNITY_EDITOR
    // Editor-time: try to ensure prefab instances get their own ID when placed/edited.
    private void OnValidate()
    {
        // If this is the prefab asset itself, DONT generate or change an ID here.
        // We only want IDs on scene instances.
        #if UNITY_2020_1_OR_NEWER
        bool isPrefabAsset = PrefabUtility.IsPartOfPrefabAsset(gameObject);
        #else
        bool isPrefabAsset = PrefabUtility.GetCorrespondingObjectFromSource(gameObject) == null && PrefabUtility.GetPrefabInstanceHandle(gameObject) == null;
        #endif

        if (isPrefabAsset)
            return;

        // If this is a prefab instance and equals source, generate new ID
        var prefabSource = PrefabUtility.GetCorrespondingObjectFromSource(gameObject) as GameObject;
        if (prefabSource != null)
        {
            var srcSave = prefabSource.GetComponent<SaveableObject>();
            if (srcSave != null && !string.IsNullOrEmpty(srcSave.uniqueID))
            {
                // generate new one if already exists.
                if (uniqueID == srcSave.uniqueID)
                {
                    uniqueID = System.Guid.NewGuid().ToString();
                    EditorUtility.SetDirty(this);
                    // mark scene dirty so change persists
                    if (!Application.isPlaying)
                        EditorSceneManager.MarkSceneDirty(gameObject.scene);
                }
            }
        }

        // If no ID at all, create one 
        if (string.IsNullOrEmpty(uniqueID))
        {
            uniqueID = System.Guid.NewGuid().ToString();
            EditorUtility.SetDirty(this);
            if (!Application.isPlaying)
                EditorSceneManager.MarkSceneDirty(gameObject.scene);
        }
    }
#endif

    // Runtime safety
    private void Awake()
    {
        if (string.IsNullOrEmpty(uniqueID))
            uniqueID = System.Guid.NewGuid().ToString();

        // final safety: if another object in scene already uses this ID, regenerate
        var all = FindObjectsOfType<SaveableObject>();
        foreach (var s in all)
        {
            if (s != this && s.uniqueID == uniqueID)
            {
                uniqueID = System.Guid.NewGuid().ToString();
                break;
            }
        }
    }
    }

}