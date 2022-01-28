using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The Path.
/// </summary>
public class Path
{

    /// <summary>
    /// The nodes.
    /// </summary>
    protected List<Node> m_Nodes = new List<Node> ();
	
    /// <summary>
    /// The length of the path.
    /// </summary>
    protected float m_Length = 0f;

    /// <summary>
    /// Gets the nodes.
    /// </summary>
    /// <value>The nodes.</value>
    public virtual List<Node> nodes
    {
        get
        {
            return m_Nodes;
        }
    }

    /// <summary>
    /// Gets the length of the path.
    /// </summary>
    /// <value>The length.</value>
    public virtual float length
    {
        get
        {
            return m_Length;
        }
    }

    /// <summary>
    /// Bake the path.
    /// Making the path ready for usage, Such as caculating the length.
    /// </summary>
    public virtual void Bake ()
    {
        List<Node> calculated = new List<Node> ();
        m_Length = 0f;
        for ( int i = 0; i < m_Nodes.Count; i++ )
        {
            Node node = m_Nodes [ i ];
            for ( int j = 0; j < node.neighbors.Count; j++ )
            {
                Node connection = node.neighbors [ j ];
				
                // Don't calcualte calculated nodes
                if ( m_Nodes.Contains ( connection ) && !calculated.Contains ( connection ) )
                {
					
                    // Calculating the distance between a node and connection when they are both available in path nodes list
                    m_Length += Vector2.Distance ( node.position, connection.position );
                }
            }
            calculated.Add ( node );
        }
    }

}