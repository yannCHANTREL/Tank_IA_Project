using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ListVariable : ScriptableObject
{
    public abstract void Reset();
    public abstract void IncrementSize();
}
