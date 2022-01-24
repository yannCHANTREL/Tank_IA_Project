using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareGrid : WeightedGraph<Location>
{
    private int m_NumberLocations;

    public SquareGrid(int numberLocations)
    {
        m_NumberLocations = numberLocations;
    }

    public double Cost(Location a, Location b)
    {
        if (b.stateLocation == 0)
        {
            return Vector2.Distance ( a.position, b.position );
        }
        return Vector2.Distance ( a.position, b.position ) * m_NumberLocations;
    }
}
