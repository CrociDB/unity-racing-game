using System;
using System.Collections;
using System.Collections.Generic;

namespace Game.GameManagement
{
    [System.Serializable]
    public class UserData
    {
        [System.Serializable]
        public class UserLevelData
        {
            public int m_Stars;
            public float m_Time;
        };

        public Dictionary<string, UserLevelData> m_LevelData;

        public UserData()
        {
            m_LevelData = new Dictionary<string, UserLevelData>();
        }

        public UserLevelData GetUserLevelData(string levelName)
        {
            if (m_LevelData.ContainsKey(levelName))
                return m_LevelData[levelName];

            return null;
        }

        public void SetUserLevelData(string levelName, UserLevelData levelData)
        {
            m_LevelData[levelName] = levelData;
        }
    }
}
