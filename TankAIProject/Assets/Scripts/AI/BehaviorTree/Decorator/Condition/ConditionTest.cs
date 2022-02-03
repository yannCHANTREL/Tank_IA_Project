using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConditionTest : ScriptableObject
{
    public abstract bool Test(int teamIndex, int tankIndex = -1);
}
