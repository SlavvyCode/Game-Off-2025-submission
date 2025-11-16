using UnityEngine;

namespace Project.Scripts.Util
{
    public class Cooldown
    {
        private float cooldownTime;
        private float lastUsedTime;

        public Cooldown(float cooldownTime)
        {
            this.cooldownTime = cooldownTime;
            lastUsedTime = -cooldownTime; // So it's ready immediately
        }

        public bool IsReady()
        {
            return (Time.time - lastUsedTime) >= cooldownTime;
        }

        public void Use()
        {
            lastUsedTime = UnityEngine.Time.time;
        }

        public float TimeRemaining()
        {
            float timePassed = Time.time - lastUsedTime;
            return Mathf.Max(0, cooldownTime - timePassed);
        }
        
    }
}