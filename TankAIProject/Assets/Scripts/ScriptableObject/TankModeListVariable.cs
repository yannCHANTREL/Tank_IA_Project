using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Mode { Defense, Attack, Health };

[CreateAssetMenu(menuName = "Variables/TankModeListVariable")]
public class TankModeListVariable : GenericListVariable<Mode>
{
}
