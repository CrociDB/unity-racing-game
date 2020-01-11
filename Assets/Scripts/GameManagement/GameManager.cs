using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GameManagement
{
    public class GameManager : MonoBehaviour
    {
        private void Awake() 
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
