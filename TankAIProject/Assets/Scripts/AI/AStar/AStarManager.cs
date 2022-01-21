using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarManager
{

    public void TestAStar(VirtualGrid classGrid)
    {
        var grid = new SquareGrid(10, 10);
        
        // create locations
        Dictionary<Vector2Int,Location> listLocation = new Dictionary<Vector2Int,Location>();
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Vector2 vect2 = classGrid.GetVector2WorldPositionByIndex(new Vector2Int(i, j));
                listLocation.Add(new Vector2Int(i,j),new Location(vect2));
            }
        }

        // add state walls and forest and several neighbors
        foreach (var location in listLocation)
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
            if (listLocation.TryGetValue(new Vector2Int(posX, posY - 1), out neighbors1))
            {
                location.Value.AddNeighbors(neighbors1);
            }
            if (listLocation.TryGetValue(new Vector2Int(posX + 1, posY), out neighbors2))
            {
                location.Value.AddNeighbors(neighbors2);
            }
            if (listLocation.TryGetValue(new Vector2Int(posX, posY + 1), out neighbors3))
            {
                location.Value.AddNeighbors(neighbors3);
            }
            if (listLocation.TryGetValue(new Vector2Int(posX - 1, posY), out neighbors4))
            {
                location.Value.AddNeighbors(neighbors4);
            }
        }

        // Run A*
        var astar = new AStarSearch(grid, listLocation[new Vector2Int(1,4)],
            listLocation[new Vector2Int(8,5)]);
        classGrid.launch = true;
        classGrid.DrawAStarPath(grid, listLocation, astar);
    }
}
