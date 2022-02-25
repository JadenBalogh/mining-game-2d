using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Upgrade : ScriptableObject
{
    [SerializeField] private int maxStacks = 1;
    public int MaxStacks { get => maxStacks; }

    public abstract void Apply();
}
