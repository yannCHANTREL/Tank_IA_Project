using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove : MonoBehaviour
{
    public TankMoveInstructionListVariable m_MoveInstructions;
    public float m_FirePlacementRange = 13;
    public float m_AnleClamp = 20;
    public float m_RadiusTolerance = 0.5f;
    public float m_AngularTolerancePercentage = 0.1f;
    public TankIndexManager m_TankIndexManager;
    public FloatListVariable m_MoveAxis;
    public FloatListVariable m_TurnAxis;
    public Vector3ListVariableVariable m_TargetPosContainer;
    public PointVariableVariable m_TargetPointContainer;

    private Transform m_Transform;

    private void Start()
    {
        m_Transform = transform;
    }

    void Update()
    {
        int tankIndex = m_TankIndexManager.m_TankIndex;
        ProceedMovements(m_MoveInstructions.m_MoveToFireRange[tankIndex] ? m_FirePlacementRange : 0f, tankIndex);
    }

    private void ProceedMovements(float targetDistanceToTarget, int tankIndex)
    {
        Vector3 tankPos = m_Transform.position;
        Vector3 tankForward = m_Transform.forward;
        Vector3 tankRight = m_Transform.right;
        Vector3 targetPos = m_MoveInstructions.m_UseTargetPoint[tankIndex] ? m_TargetPointContainer.m_Point.m_CenterPos : m_TargetPosContainer.m_List.m_Values[tankIndex];
        Vector3 distance = targetPos - tankPos;

        if (Mathf.Abs(distance.magnitude - targetDistanceToTarget) > m_RadiusTolerance && m_MoveInstructions.m_Move[tankIndex]) { Move(distance, tankForward, tankIndex); }
        else { StopMove(tankIndex); }

        if (distance.magnitude > m_RadiusTolerance && m_MoveInstructions.m_Turn[tankIndex]) { Turn(distance, tankForward, tankRight, tankIndex); }
        else { StopTurn(tankIndex); }
    }

    private void Move(Vector3 distance, Vector3 tankForward, int tankIndex)
    {
        m_MoveAxis.m_Values[tankIndex] = Mathf.Min(Vector3.Dot(distance, tankForward) >= 0 != (m_MoveInstructions.m_MoveToFireRange[tankIndex] && distance.magnitude - m_FirePlacementRange < 0)? 1 : -1, m_MoveInstructions.m_Follow[tankIndex] ? 1 : 0);
    }
    
    private void Turn(Vector3 distance, Vector3 tankForward, Vector3 tankRight, int tankIndex)
    {
        if (!m_MoveInstructions.m_MoveForward[tankIndex]) { distance *= -1; }
        if (!m_TankIndexManager || !m_TurnAxis) return;
        float angleSign = Vector3.Dot(tankRight, distance) >= 0 ? 1 : -1;
        float angleToTarget = Mathf.Clamp(Vector3.Angle(distance, tankForward) * angleSign / m_AnleClamp, -1f, 1f);
        m_TurnAxis.m_Values[tankIndex] = Mathf.Abs(angleToTarget) > m_AngularTolerancePercentage ? angleToTarget : 0;
    }

    private void StopMove(int tankIndex)
    {
        m_MoveAxis.m_Values[tankIndex] = 0;
    }

    private void StopTurn(int tankIndex)
    {
        m_TurnAxis.m_Values[tankIndex] = 0;
    }
}
