using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetSorter : ScriptableObject
{
    public abstract float ScoreCalculation(int tankIndex);
}
