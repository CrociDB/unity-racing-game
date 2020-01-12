using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Map
{
    public class Checkpoint : MonoBehaviour
    {
        public Action<Checkpoint> OnPlayerTouched;

        private void Update() 
        {
            
        }

        private void OnTriggerEnter(Collider other) 
        {
            if (other.tag == "Player")
            {
                if (OnPlayerTouched != null) OnPlayerTouched.Invoke(this);
            }    
        }
    }
}
