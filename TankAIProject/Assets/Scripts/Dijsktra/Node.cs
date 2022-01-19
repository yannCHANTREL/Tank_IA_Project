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
    [SerializeField]
    protected List<Node> m_Connections = new List<Node>();

    private int m_StateNode;
    private Vector2 m_Position;
    private int m_posGridX;
    private int m_posGridY;

    public Node(int number, Vector2 position, int posGridI, int posGridJ)
    {
        m_StateNode = number;
        m_Position = position;
        m_posGridX = posGridI;
        m_posGridY = posGridJ;
    }

    public void AddNeighbors(Node nodeNeighbors)
    {
        m_Connections.Add(nodeNeighbors);
    }

    /// <summary>
    /// Gets the connections (neighbors).
    /// </summary>
    /// <value>The connections.</value>
    public virtual List<Node> connections
    {
        get
        {
            return m_Connections;
        }
    }

    public Node this[int index]
    {
        get
        {
            return m_Connections[index];
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
    
    public int posGridI
    {
        get
        {
            return m_posGridX;
        }
    }
    
    public int posGridJ
    {
        get
        {
            return m_posGridY;
        }
    }

}