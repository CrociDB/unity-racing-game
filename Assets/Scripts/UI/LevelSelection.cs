using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameManagement;
using UnityEngine;

namespace Game.UI
{
    public class LevelSelection : MonoBehaviour
    {
        public LevelSelectionItem m_ItemBase;

        public void Awake() 
        {
            Populate();
        }

        private void Populate()
        {
            var g = GameManager.Instance;
            foreach (var level in g.m_GameDescriptor.m_Levels)
            {
                var item = Instantiate(m_ItemBase);
                item.transform.SetParent(m_ItemBase.transform.parent);
                item.transform.localScale = Vector3.one;

                item.Build(level);
            }

            m_ItemBase.gameObject.SetActive(false);
        }
    }
}
