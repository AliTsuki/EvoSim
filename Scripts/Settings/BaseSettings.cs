﻿using UnityEngine;

//
[CreateAssetMenu(menuName = "Settings/Base Settings")]
public class BaseSettings : ScriptableObject
{
    public bool updateInEditor = true;
    [Range(1, 256)]
    public int worldSize = 40;
    [Range(0, 256)]
    public int maxAnimalCount = 50;
    [Range(0, 256)]
    public int maxPlantCount = 100;
}