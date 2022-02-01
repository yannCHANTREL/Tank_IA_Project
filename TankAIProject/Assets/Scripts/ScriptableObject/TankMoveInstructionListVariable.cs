using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/TankMoveInstructionList")]
public class TankMoveInstructionListVariable : ListVariable
{
    public List<bool> m_Move;
    public List<bool> m_Turn;
    public List<bool> m_Follow;
    public List<bool> m_MoveToFireRange;
    public List<bool> m_MoveForward;
    public List<bool> m_UseTargetPoint;

    public override void IncrementTankSize()
    {
        m_Move.Add(false);
        m_Turn.Add(false);
        m_Follow.Add(true);
        m_MoveToFireRange.Add(true);
        m_MoveForward.Add(true);
        m_UseTargetPoint.Add(false);
    }

    public override void IncrementTeamSize()
    {

    }

    public override void Reset()
    {
        m_Move = new List<bool>();
        m_Turn = new List<bool>();
        m_Follow = new List<bool>();
        m_MoveToFireRange = new List<bool>();
        m_MoveForward = new List<bool>();
        m_UseTargetPoint = new List<bool>();
    }
}
