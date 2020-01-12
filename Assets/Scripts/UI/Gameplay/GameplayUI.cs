using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class GameplayUI : MonoBehaviour
    {
        public Text m_Time;
        public Speedometer m_Speedometer;

        public void SetSpeedNormalized(float speed)
        {
            m_Speedometer.SetSpeedNormalized(speed);
        }

        public void SetTime(float time)
        {
            int minutes = (int)Mathf.Floor(time / 60f);
            int seconds = (int)Mathf.Floor(time % 60f);
            int milliseconds = (int)Mathf.Floor((time % 1.0f) * 1000.0f);
            m_Time.text = string.Format("{0}'\t{1:00}\"\t{2:000}", minutes, seconds, milliseconds);
        }
    }
}
