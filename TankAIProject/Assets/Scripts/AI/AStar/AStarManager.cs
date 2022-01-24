using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AStarManager : MonoBehaviour
{
    public VirtualGrid m_ClassGrid;             // Reference Grid
    public Vector2Int m_start;
    public Vector2Int m_end;
    public bool m_activate;
    
    private Dictionary<Vector2Int, Location> m_Locations;
    private AStarSearch m_AStar;
    
    void Start()
    {
        if (m_activate)
        {
            // preparation AStar features
            PreparationForAStarFeatures();

            // Algorithm of research of the shortest path (AStar)
            StartCoroutine(LaunchThreadWithAStar());
        }
    }
    
    private void PreparationForAStarFeatures()
    {
        SquareGrid grid = new SquareGrid(10, 10);
        m_Locations = new Dictionary<Vector2Int,Location>();
        
        // create locations
        /*for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Vector2 vect2 = m_ClassGrid.GetVector2WorldPositionByIndex(new Vector2Int(i, j));
                m_Locations.Add(new Vector2Int(i,j),new Location(vect2));
            }
        }*/
        for (int i = 0; i < m_ClassGrid.gridSize; i++)
        {
            for (int j = 0; j < m_ClassGrid.gridSize; j++)
            {
                if (m_ClassGrid.grid[i, j] == 0)
                {
                    Vector2 vect2 = m_ClassGrid.GetVector2WorldPositionByIndex(new Vector2Int(i, j));
                    m_Locations.Add(new Vector2Int(i, j), new Location(vect2));
                }
            }
        }
        
        // add state walls and forest and several neighbors
        foreach (var location in m_Locations)
        {
            int posX = location.Key.x;
            int posY = location.Key.y;
            // place wall
            if (posX >= 1 && posX <= 3 && posY >= 7 && posY <= 8)
            {
                location.Value.ChangeStateLocation(-1);
            }
            // place forest
            if (posX >= 4 && posX <= 6 && posY >= 5 && posY <= 8)
            {
                location.Value.ChangeStateLocation(1);
            }
            
            Location neighbors1, neighbors2, neighbors3, neighbors4;
            if (m_Locations.TryGetValue(new Vector2Int(posX, posY - 1), out neighbors1))
            {
                location.Value.AddNeighbors(neighbors1);
            }
            if (m_Locations.TryGetValue(new Vector2Int(posX + 1, posY), out neighbors2))
            {
                location.Value.AddNeighbors(neighbors2);
            }
            if (m_Locations.TryGetValue(new Vector2Int(posX, posY + 1), out neighbors3))
            {
                location.Value.AddNeighbors(neighbors3);
            }
            if (m_Locations.TryGetValue(new Vector2Int(posX - 1, posY), out neighbors4))
            {
                location.Value.AddNeighbors(neighbors4);
            }
        }
        m_AStar = new AStarSearch(grid);
    }
    
    private IEnumerator LaunchThreadWithAStar()
    {
        // Launch one thread each second, or when the previous is finish
        while (true)
        {
            Thread t = new Thread(ImplementedAStar);
            var temp = Time.realtimeSinceStartup;
            t.Start();
                
            // wait end execution thread
            // OR 1 second difference between now and the start of the thread
            while(t.IsAlive || (Time.realtimeSinceStartup - temp) < 1.0f)
            {
                //Debug.Log("B");
                yield return null;
            }
            //Debug.Log("time execution thread : " + (Time.realtimeSinceStartup - temp));
        }
    } 

    private void ImplementedAStar()
    {
        // try move since A point to B point
        Vector2Int start = m_ClassGrid.GetIndexByWorldPosition(m_start);
        Vector2Int end = m_ClassGrid.GetIndexByWorldPosition(m_end);
        if (m_Locations.ContainsKey(start) && m_Locations.ContainsKey(end))
        {
            FinalPath path = m_AStar.GetShortestPath(m_Locations[start], m_Locations[end]);
            m_ClassGrid.DrawAStarPath(m_Locations, m_AStar, path);
        }
        else 
        {
            if (!m_Locations.ContainsKey(start))
            {
                Debug.Log("Error AStar start incorrect");
            }
            if (!m_Locations.ContainsKey(end))
            {
                Debug.Log("Error AStar end incorrect");
            }
        }
    }
}
