using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetSelector : ScriptableObject
{
    public abstract bool Test(int tankIndex);
}
