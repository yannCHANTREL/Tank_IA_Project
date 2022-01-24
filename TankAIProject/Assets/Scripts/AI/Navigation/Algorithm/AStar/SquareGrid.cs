using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareGrid : WeightedGraph<Node>
{
    private int m_NumberLocations;

    public SquareGrid(int numberLocations)
    {
        m_NumberLocations = numberLocations;
    }

    public double Cost(Node a, Node b)
    {
        if (b.stateNode == 0)
        {
            return Vector2.Distance ( a.position, b.position );
        }
        return Vector2.Distance ( a.position, b.position ) * m_NumberLocations;
    }
}
