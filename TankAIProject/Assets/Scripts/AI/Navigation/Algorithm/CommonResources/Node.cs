using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The Node.
/// </summary>
public class Node
{

    /// <summary>
    /// The connections (neighbors).
    /// </summary>
    protected List<Node> m_Neighbors = new List<Node>();

    private int m_StateNode;
    private Vector2 m_Position;

    public Node(int number, Vector2 position)
    {
        m_StateNode = number;
        m_Position = position;
    }

    public void AddNeighbors(Node nodeNeighbors)
    {
        m_Neighbors.Add(nodeNeighbors);
    }

    /// <summary>
    /// Gets the connections (neighbors).
    /// </summary>
    /// <value>The connections.</value>
    public virtual List<Node> neighbors
    {
        get
        {
            return m_Neighbors;
        }
    }

    public int stateNode
    {
        get
        {
            return m_StateNode;
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