using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface WeightedGraph<L>
{
    double Cost(Location a, Location b);
}
