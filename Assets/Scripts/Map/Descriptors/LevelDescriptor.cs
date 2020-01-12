using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDescriptor", menuName = "ScriptableObjects/LevelDescriptor", order = 2)]
public class LevelDescriptor : ScriptableObject
{
    public enum Difficulty 
    {
        Easy,
        Medium,
        Hard
    };

    [Header("Description")]
    public string m_Title;
    public Difficulty m_Difficulty;

    [Header("Internal")]
    public string m_Scene;

    [Header("Gameplay Parameters")]
    public int m_Boosts;
    public float m_TimeLimit;
    public Vector2 m_TimeByStar;
}
