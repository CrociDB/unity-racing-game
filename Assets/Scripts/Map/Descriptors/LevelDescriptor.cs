﻿using System.Collections;
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
    public string m_Name;
    public Difficulty m_Difficulty;

    [Header("Internal")]
    public string m_Scene;

    [Header("Gameplay Parameters")]
    public float[] m_TimeByStar;
}