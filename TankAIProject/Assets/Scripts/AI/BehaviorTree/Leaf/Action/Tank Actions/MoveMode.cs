using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Action/TankAction/MoveMode")]
public class MoveMode : Action
{
    public bool m_Move;
    public bool m_Turn;
    public bool m_Follow;
    public bool m_MoveToFireRange;
    public bool m_MoveForward;
    public bool m_UsePathfinding;
    public TargetType m_TargetType;
    public Vector3ListVariable m_TargetPos;
    public PointVariable m_TargetPoint;
    public Vector3ListVariableVariable m_TargetPosContainer;
    public PointVariableVariable m_TargetPointContainer;
    public TankMoveInstructionListVariable m_MoveInstructions;
    public override void AddAITank(int teamIndex, int tankIndex = 0)
    {

    }

    public override Status BHUpdate(int teamIndex, int tankIndex = 0)
    {
        m_MoveInstructions.m_Move[tankIndex] = m_Move;
        m_MoveInstructions.m_Turn[tankIndex] = m_Turn;
        m_MoveInstructions.m_Follow[tankIndex] = m_Follow;
        m_MoveInstructions.m_MoveToFireRange[tankIndex] = m_MoveToFireRange;
        m_MoveInstructions.m_MoveForward[tankIndex] = m_MoveForward;
        m_MoveInstructions.m_UsePathfinding[tankIndex] = m_UsePathfinding;
        m_MoveInstructions.m_TargetType[tankIndex] = m_TargetType;

        if (m_TargetType == TargetType.point && m_TargetPoint) { m_TargetPosContainer.m_List = m_TargetPos; }
        else if (m_TargetType == TargetType.tank && m_TargetPos) { m_TargetPointContainer.m_Point = m_TargetPoint; }
        return Status.success;
    }

    public override void OnInitialize()
    {
        
    }

    public override void OnTerminate(Status status)
    {
        
    }

    public override void RemoveAITank(int teamIndex, int tankIndex = 0)
    {
        
    }
}
