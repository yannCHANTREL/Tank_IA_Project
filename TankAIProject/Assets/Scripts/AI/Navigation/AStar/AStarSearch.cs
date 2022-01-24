using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarSearch
{
    private WeightedGraph<Location> m_Graph;

    public Dictionary<Location, Location> m_CameFrom;

    public AStarSearch(WeightedGraph<Location> graph)
    {
        m_Graph = graph;
    }

    // Note: a generic version of A* would abstract over Location and
    // also Heuristic
    static public double Heuristic(Location a, Location b)
    {
        Vector2 posA = a.position;
        Vector2 posB = b.position;
        return Math.Abs(posA.x - posB.x) + Math.Abs(posA.y - posB.y);
    }

    public FinalPath GetShortestPath(Location start, Location goal)
    {
        
        // We don't accept null arguments
        if ( start == null || goal == null )
        {
            throw new ArgumentNullException ();
        }
        
        // The final path
        FinalPath path = new FinalPath ();
        
        // If the start and end are same node, we can return the start node
        if ( start == goal )
        {
            path.locations.Add ( start );
            return path;
        }
        
        m_CameFrom = new Dictionary<Location, Location>();
        Dictionary<Location, double> costSoFar = new Dictionary<Location, double>();

        var frontier = new PriorityQueue<Location>();
        frontier.Enqueue(start, 0);
        
        costSoFar[start] = 0;
        
        while (frontier.count > 0)
        {
            var current = frontier.Dequeue();

            // When the current node is equal to the goal node, then we can break and return the path
            if (current.Equals(goal))
            {
                // Construct the shortest path
                while (m_CameFrom.ContainsKey(current))
                {
                    // Insert the node into the final result
                    path.locations.Insert ( 0, current );
                    
                    // Traverse from start to end
                    current = m_CameFrom[current];
                }
                // Insert the source onto the final result
                path.locations.Insert ( 0, current );
                break;
            }

            foreach (var next in current.neighbors)
            {
                double newCost = costSoFar[current] + m_Graph.Cost(current, next);
                if (!costSoFar.ContainsKey(next)
                    || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;
                    double priority = newCost + Heuristic(next, goal);
                    frontier.Enqueue(next, priority);
                    m_CameFrom[next] = current;
                }
            }
        }
        return path;
    }
}
