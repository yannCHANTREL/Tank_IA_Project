using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareGrid : WeightedGraph<Location>
{
    // Implementation notes: I made the fields public for convenience,
    // but in a real project you'll probably want to follow standard
    // style and make them private.

    public int width, height;
    public HashSet<Location> walls = new HashSet<Location>();
    public HashSet<Location> forests = new HashSet<Location>();

    public SquareGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public bool Passable(Location id)
    {
        return !walls.Contains(id);
    }

    public double Cost(Location a, Location b)
    {
        return forests.Contains(b) ? 5 : 1;
    }
}
