using Project.Scripts.Player;
using UnityEngine;

namespace Project.Scripts.Util
{
    public class SaveUtil : MonoBehaviour
    {

        
        public static void ClearUserData()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
        
        
        
        
        public static Vector2 GetCheckpoint()
        {
            float x = PlayerPrefs.GetFloat("respawn_x", 0f);
            float y = PlayerPrefs.GetFloat("respawn_y", 0f);
            return new Vector2(x, y);
        }
        
        
    }
}