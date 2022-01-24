using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface WeightedGraph<L>
{
    double Cost(Node a, Node b);
}
