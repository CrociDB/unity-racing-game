using System;
using System.Linq;
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
            public string m_Name;
            public int m_Stars;
            public float m_Time;
        };

        // Cannot use Dictionary because it's not serializable by default
        public List<UserLevelData> m_LevelData;

        public UserData()
        {
            m_LevelData = new List<UserLevelData>();
        }

        public UserLevelData GetUserLevelData(string levelName)
        {
            return m_LevelData.Where(l => l.m_Name == levelName).FirstOrDefault();
        }

        public void SetUserLevelData(UserLevelData levelData)
        {
            var ld = m_LevelData.Where(l => l.m_Name == levelData.m_Name).FirstOrDefault();
            if (ld == null)
            {
                m_LevelData.Add(levelData);
            }
            else
            {
                ld.m_Stars = levelData.m_Stars;
                ld.m_Time = levelData.m_Time;
            }
        }
    }
}
