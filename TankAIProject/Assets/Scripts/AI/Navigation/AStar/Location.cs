using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location
{
    // Implementation notes: I am using the default Equals but it can
    // be slow. You'll probably want to override both Equals and
    // GetHashCode in a real project.
    
    private List<Location> m_Neighbors;
    private int m_StateLocation; // -1 Wall ; 0 Empty ; 1 Forest
    private Vector2 m_Position;

    public Location(int number, Vector2 Position)
    {
        m_Position = Position;
        m_Neighbors = new List<Location>();
        m_StateLocation = number;
    }

    public void AddNeighbors(Location location)
    {
        m_Neighbors.Add(location);
    }

    public void ChangeStateLocation(int stateLocation)
    {
        m_StateLocation = stateLocation;
    }
    
    public virtual List<Location> neighbors
    {
        get
        {
            return m_Neighbors;
        }
    }
    
    public int stateLocation
    {
        get
        {
            return m_StateLocation;
        }
    }
    
    public Vector2 position
    {
        get
        {
            return m_Position;
        }
    }
}
