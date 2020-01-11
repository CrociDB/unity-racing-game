using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDescriptor", menuName = "ScriptableObjects/GameDescriptor", order = 1)]
public class GameDescriptor : ScriptableObject
{
    [Header("Levels")]
    public LevelDescriptor[] m_Levels;
}
