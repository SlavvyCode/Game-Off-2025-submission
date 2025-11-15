using System;
using System.Collections.Generic;

namespace General_and_Helpers
{
    public class LevelSaveData
    {
        public string SceneName { get; set; }
        public int TotalCollectibles { get; private set; } = 0;
        public string LastCheckpointId { get; set; } = null;
        

        public void SetTotalCollectibles(int total)
        {
            TotalCollectibles = total;
        }
    }
}