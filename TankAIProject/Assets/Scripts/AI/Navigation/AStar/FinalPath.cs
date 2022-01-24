using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPath
{
    private List<Location> m_Location = new List<Location>();
    private float m_Length = 0f;
    
    public List<Location> locations
    {
        get
        {
            return m_Location;
        }
    }
    
    public float length
    {
        get
        {
            return m_Length;
        }
    }
}
