using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Map
{
    public class Checkpoint : MonoBehaviour
    {
        public Action<Checkpoint> OnPlayerTouched;
        public Material m_FinalCheckpointMaterial;

        public void SetFinalCheckpoint()
        {
            GetComponent<MeshRenderer>().material = m_FinalCheckpointMaterial;
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
