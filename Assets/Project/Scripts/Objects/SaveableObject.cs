using UnityEngine;

namespace Project.Scripts.Objects
{
    public class SaveableObject : MonoBehaviour
    {
        [SerializeField] private string uniqueID;
        public string UniqueID => uniqueID;

        // Runtime: ensure object always has a valid ID
        private void Awake()
        {
            if (string.IsNullOrEmpty(uniqueID))
                uniqueID = System.Guid.NewGuid().ToString();

            // Safety: ensure uniqueness in scene
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

#if UNITY_EDITOR
        // Allow editor script to regenerate ID
        public void ForceGenerateID()
        {
            uniqueID = System.Guid.NewGuid().ToString();
        }
#endif
    }
}