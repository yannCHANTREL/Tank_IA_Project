using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location
{
    // Implementation notes: I am using the default Equals but it can
    // be slow. You'll probably want to override both Equals and
    // GetHashCode in a real project.
    
    private int m_X, m_Y;
    
    public Location(int x, int y)
    {
        this.m_X = x;
        this.m_Y = y;
    }
    
    public int x
    {
        get
        {
            return m_X;
        }
    }
    
    public int y
    {
        get
        {
            return m_Y;
        }
    }
}
