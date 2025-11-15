using UnityEngine;
    public class Util 
    {
        
        public static void SetActiveWithParents(GameObject titleScreenCanvas, bool b)
        {
            titleScreenCanvas.SetActive(b);
            Transform parent = titleScreenCanvas.transform.parent;
            while (parent != null)
            {
                parent.gameObject.SetActive(b);
                parent = parent.parent;
            }
        }

        public static Camera FindMainCamera()
        {
            return Camera.main;
        }

        public static string GenerateID(GameObject gameObject)
        {
            var scene = gameObject.scene.name;
            var name = gameObject.name;
            var pos = gameObject.transform.position;

            return $"{scene}_{name}_{pos.x:0.00}_{pos.y:0.00}_{pos.z:0.00}";
        }
    }
