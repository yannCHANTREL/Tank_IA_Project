using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareGrid : WeightedGraph<Location>
{
    // Implementation notes: I made the fields public for convenience,
    // but in a real project you'll probably want to follow standard
    // style and make them private.
    
    public static readonly Location[] DIRS = new []
    {
        new Location(1, 0),
        new Location(0, -1),
        new Location(-1, 0),
        new Location(0, 1)
    };

    public int width, height;
    public HashSet<Location> walls = new HashSet<Location>();
    public HashSet<Location> forests = new HashSet<Location>();

    public SquareGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public bool InBounds(Location id)
    {
        return 0 <= id.x && id.x < width
                         && 0 <= id.y && id.y < height;
    }

    public bool Passable(Location id)
    {
        return !walls.Contains(id);
    }

    public double Cost(Location a, Location b)
    {
        return forests.Contains(b) ? 5 : 1;
    }
    
    public IEnumerable<Location> Neighbors(Location id)
    {
        foreach (var dir in DIRS) {
            Location next = new Location(id.x + dir.x, id.y + dir.y);
            if (InBounds(next) && Passable(next)) {
                yield return next;
            }
        }
    }
}
