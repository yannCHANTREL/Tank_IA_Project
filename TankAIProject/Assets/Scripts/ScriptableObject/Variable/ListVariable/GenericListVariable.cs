using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericListVariable<T> : ListVariable
{
    public List<T> m_Values;
    public override void Reset()
    {
        m_Values = new List<T>();
    }

    public override void IncrementTankSize()
    {
        m_Values.Add(default(T));
    }
    
    public override void IncrementTeamSize()
    {
    }
}
