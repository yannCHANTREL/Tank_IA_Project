using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location
{
    // Implementation notes: I am using the default Equals but it can
    // be slow. You'll probably want to override both Equals and
    // GetHashCode in a real project.
    
    private List<Location> m_neighbors;
    private int m_stateLocation; // -1 Wall ; 0 Empty ; 1 Forest
    private Vector2 m_Position;

    public Location(int number, Vector2 Position)
    {
        m_Position = Position;
        m_neighbors = new List<Location>();
        m_stateLocation = number;
    }

    public void AddNeighbors(Location location)
    {
        m_neighbors.Add(location);
    }

    public void ChangeStateLocation(int stateLocation)
    {
        m_stateLocation = stateLocation;
    }
    
    public virtual List<Location> neighbors
    {
        get
        {
            return m_neighbors;
        }
    }
    
    public int stateLocation
    {
        get
        {
            return m_stateLocation;
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
